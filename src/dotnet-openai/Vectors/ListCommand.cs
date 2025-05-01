using System.ClientModel.Primitives;
using System.ComponentModel;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Vectors;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
[Description("List vector stores")]
[Service]
public class ListCommand(OpenAIClient oai, IAnsiConsole console, VectorIdMapper mapper, CancellationTokenSource cts) : Command<ListCommandSettings>
{
    public override int Execute(CommandContext context, ListCommandSettings settings)
    {
        CollectionResult result = oai.GetVectorStoreClient().GetVectorStores();
        if (result is null)
        {
            console.MarkupLine($":cross_mark: Failed to list vector stores");
            return -1;
        }

        var nodes = settings.ApplyFilters(result);

        if (settings.Json)
            return console.RenderJson(nodes, settings, cts.Token);

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[lime]ID[/]")
            .AddColumn("[lime]Name[/]")
            .AddColumn("[lime]Files[/]", x => x.RightAligned())
            .AddColumn("[lime]Size[/]", x => x.RightAligned())
            .AddColumn("[lime]Last Active[/]");

        // Adding live for better user feedback on amount of data.
        console.Live(table)
            // Clear the "progress" table since we render the full one at the end.
            .AutoClear(true)
            .Start(ctx =>
            {
                ctx.UpdateTarget(table);
                foreach (var node in nodes["data"]!.AsArray().AsEnumerable().Where(x => x != null))
                {
                    var id = node!["id"]!.ToString();
                    var name = node["name"]!.ToString();

                    mapper.SetId(name, id);

                    table.AddRow(
                        id,
                        name,
                        node["file_counts"]!["total"]!.ToString(),
                        int.Parse(node["usage_bytes"]!.ToString()).Bytes().Humanize(),
                        DateTimeOffset.FromUnixTimeSeconds(long.Parse(node["last_active_at"]!.ToString())).ToString("yyyy-MM-dd T HH:mm")
                    );
                }
            });

        if (table.Rows.Count > 0)
            console.Write(table);

        return 0;
    }
}
