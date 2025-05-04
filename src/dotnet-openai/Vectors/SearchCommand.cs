using System.ClientModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;
using static Devlooped.OpenAI.Vectors.SearchCommand;

namespace Devlooped.OpenAI.Vectors;

[Description("Performs semantic search against a vector store")]
[Service]
public partial class SearchCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : AsyncCommand<SearchSettings>
{
    static readonly JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true
    };

    record Filter(string Type, string Key, object Value);

    readonly Dictionary<string, string> operators = new()
    {
        ["=="] = "eq",
        ["="] = "eq",
        ["!="] = "ne",
        [">"] = "gt",
        [">="] = "gte",
        ["<"] = "lt",
        ["<="] = "lte"
    };

    public override async Task<int> ExecuteAsync(CommandContext context, SearchSettings settings)
    {
        var message = oai.Pipeline.CreateMessage();
        message.Request.Method = "POST";
        message.Request.Uri = new Uri($"https://api.openai.com/v1/vector_stores/{settings.Store}/search");

        var content = new Dictionary<string, object>
        {
            ["query"] = settings.Query,
            ["ranking_options"] = new { score_threshold = settings.Score }
        };

        if (settings.Rewrite)
            content["rewrite_query"] = settings.Rewrite;

        if (settings.Filters.Length > 0)
        {
            var filters = new List<Filter>();
            foreach (var item in settings.Filters)
            {
                var match = FilterExpression().Match(item);
                if (match.Success)
                {
                    var key = match.Groups["key"].Value;
                    var op = match.Groups["operator"].Value;
                    var value = match.Groups["value"].Value;

                    // Map the operator to its string equivalent
                    if (operators.TryGetValue(op, out var operatorType))
                    {
                        // Parse value to appropriate type
                        object parsedValue;
                        if (double.TryParse(value, out var number))
                            parsedValue = number;
                        else if (bool.TryParse(value, out var boolean))
                            parsedValue = boolean;
                        else
                            parsedValue = value;

                        filters.Add(new(operatorType, key, parsedValue));
                    }
                }
            }
            content["filters"] = new
            {
                type = "and",
                filters
            };
        }

        message.Request.Content = BinaryContent.Create(BinaryData.FromString(JsonSerializer.Serialize(content, options)));

        await oai.Pipeline.SendAsync(message);
        Debug.Assert(message.Response != null);

        var node = JsonNode.Parse(message.Response.Content.ToString());
        if (node!["data"] is not JsonArray data)
        {
            console.RenderJson(message.Response, settings.Monochrome, cts.Token);
            return -1;
        }

        if (settings.Json)
            return console.RenderJson(node, settings, cts.Token);

        console.MarkupLine($"[lime]Query:[/] [grey]{node!["search_query"]![0]!.ToString()}[/]");
        if (data.Count == 0)
        {
            console.MarkupLine("[red]No results found.[/]");
            return -2;
        }

        var table = new Table().Border(TableBorder.Rounded)
                               .AddColumn("[lime]File[/]")
                               .AddColumn("[lime]Text[/]")
                               .HideHeaders();

        // Adding live for better user feedback on amount of data.
        console.Live(table)
            // Clear the "progress" table since we render the full one at the end.
            .AutoClear(true)
            .Start(ctx =>
            {
                foreach (var node in data)
                {
                    var fileId = node!["file_id"]?.ToString() ?? "N/A";
                    var fileName = node["filename"]?.ToString() ?? "N/A";
                    var score = node["score"]?.ToString() ?? "N/A";
                    var text = node["content"]!.AsArray()[0]?["type"]?.ToString() == "text" ?
                        node["content"]!.AsArray()[0]!["text"]?.ToString() ?? "" :
                        node["content"]?.ToJsonString() ?? "";

                    table.AddRow(
                    [
                        // Use a more compact representation since typically the 
                        // text will be far longer and can use the additional width
                        new Table().HideHeaders().NoBorder().AddColumns("", "")
                            .AddRow("[lime]ID[/]", fileId)
                            .AddRow("[lime]Name[/]", fileName)
                            .AddRow("[lime]Score[/]", score),
                        new Paragraph(text)
                    ]);

                    // Set initial live target after we have some data
                    if (table.Rows.Count == 1)
                        ctx.UpdateTarget(table);

                    // refresh every 10 items
                    if (table.Rows.Count % 10 == 0)
                    {
                        ctx.Refresh();
                    }
                }
            });

        console.Write(table);
        return 0;
    }

    public class SearchSettings(VectorIdMapper mapper) : StoreCommandSettings(mapper)
    {
        [Description("The query to search for")]
        [CommandArgument(1, "<QUERY>")]
        public required string Query { get; init; }

        [Description("Vector file attributes to filter as KEY[[=|!=|>|>=|<|<=]]VALUE")]
        [CommandOption("-f|--filter")]
        public string[] Filters { get; set; } = [];

        [Description("Automatically rewrite your queries for optimal performance")]
        [DefaultValue(true)]
        [CommandOption("-r|--rewrite")]
        public bool Rewrite { get; init; } = true;

        [Description("The minimum score to include in results")]
        [DefaultValue(0.5)]
        [CommandOption("-s|--score <SCORE>")]
        public double Score { get; set; } = 0.5;
    }

    [GeneratedRegex("""
        ^\s*
        (?<key>[^\s=!<>]+)              # Match the key (anything not whitespace or operator chars)
        \s*
        (?<operator>==|=|!=|>=|<=|>|<)  # Match any of the operators
        \s*
        (?<value>.+?)                   # Match the value (non-greedy)
        \s*$
        """, RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex FilterExpression();
}
