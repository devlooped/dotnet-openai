using System;
using System.ClientModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using NuGet.ContentModel;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;
using static Devlooped.OpenAI.Vectors.SearchCommand;

namespace Devlooped.OpenAI.Vectors;

class SearchCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : AsyncCommand<SearchSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, SearchSettings settings)
    {
        var message = oai.Pipeline.CreateMessage();
        message.Request.Method = "POST";
        message.Request.Uri = new Uri($"https://api.openai.com/v1/vector_stores/{settings.Id}/search");

        var content = new Dictionary<string, object>
        {
            ["query"] = settings.Query,
        };

        if (settings.Rewrite)
            content["rewrite_query"] = settings.Rewrite;

        message.Request.Content = BinaryContent.Create(BinaryData.FromString(JsonSerializer.Serialize(content)));

        await oai.Pipeline.SendAsync(message);
        Debug.Assert(message.Response != null);

        var node = JsonNode.Parse(message.Response.Content.ToString());
        var data = node!["data"]!.AsArray();
        foreach (var result in data.ToList())
        {
            if (result!["score"]!.GetValue<double>() < settings.Score)
                data.Remove(result);
        }

        if (settings.Json)
            return console.RenderJson(node, settings, cts.Token);

        console.MarkupLine($"[lime]Query:[/] [grey]{node!["search_query"]![0]!.ToString()}[/]");

        var table = new Table().Border(TableBorder.Rounded)
                               .AddColumn("[lime]File ID[/]")
                               .AddColumn("[lime]File Name[/]")
                               .AddColumn("[lime]Score[/]")
                               .AddColumn("[lime]Text[/]");

        // Adding live for better user feedback on amount of data.
        console.Live(table)
            // Clear the "progress" table since we render the full one at the end.
            .AutoClear(true)
            .Start(ctx =>
            {
                ctx.UpdateTarget(table);
                foreach (var node in data)
                {
                    var fileId = node!["file_id"]?.ToString() ?? "N/A";
                    var fileName = node["filename"]?.ToString() ?? "N/A";
                    var score = node["score"]?.ToString() ?? "N/A";
                    var text = node["content"]!.AsArray()[0]?["type"]?.ToString() == "text" ?
                        node["content"]!.AsArray()[0]!["text"]?.ToString() ?? "" :
                        node["content"]?.ToJsonString() ?? "";

                    table.AddRow(fileId, fileName, score, text);
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

    public class SearchSettings : JsonCommandSettings
    {
        [Description("The ID of the vector store")]
        [CommandArgument(0, "<ID>")]
        public required string Id { get; init; }

        [Description("The query to search for")]
        [CommandArgument(1, "<QUERY>")]
        public required string Query { get; init; }

        //[Description("Vector file attributes to filter  as KEY=VALUE")]
        //[CommandOption("-a|--attribute")]
        //public string[] Attributes { get; set; } = [];

        [Description("Automatically rewrite your queries for optimal performance")]
        [DefaultValue(true)]
        [CommandOption("-r|--rewrite")]
        public required bool Rewrite { get; init; } = true;

        [Description("The minimum score to include in results")]
        [DefaultValue(0.5)]
        [CommandOption("-s|--score <SCORE>")]
        public required double Score { get; set; } = 0.5;
    }
}
