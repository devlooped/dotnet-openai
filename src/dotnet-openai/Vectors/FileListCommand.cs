using System.ClientModel.Primitives;
using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using OpenAI.VectorStores;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Vectors;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
[Description("List files associated with vector store")]
[Service]
public class FileListCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : Command<FileListCommand.Settings>
{
    static readonly JsonSerializerOptions jsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    };

    public override int Execute(CommandContext context, Settings settings)
    {
        var options = new VectorStoreFileAssociationCollectionOptions
        {
            Filter = settings.Filter,
        };

        CollectionResult result = oai.GetVectorStoreClient().GetFileAssociations(settings.Store, options, cts.Token);
        if (result is null)
        {
            console.MarkupLine($":cross_mark: Failed to list vector stores");
            return -1;
        }

        var nodes = settings.ApplyFilters(result);

        if (settings.Json)
            return console.RenderJson(nodes, settings, cts.Token);

        var table = new Table().Border(TableBorder.Rounded)
                               .AddColumn("[lime]File ID[/]")
                               .AddColumn("[lime]File Name[/]")
                               .AddColumn("[lime]Status[/]");

        // Adding live for better user feedback on amount of data.
        console.Live(table)
            // Clear the "progress" table since we render the full one at the end.
            .AutoClear(true)
            .Start(ctx =>
            {
                ctx.UpdateTarget(table);
                foreach (var node in nodes["data"]!.AsArray().AsEnumerable().Where(x => x != null))
                {
                    var fileId = node!["id"]?.ToString() ?? "N/A";
                    var fileName = node["attributes"]?["filename"]?.ToString() ?? "N/A";
                    var status = node["status"]?.ToString() ?? "N/A";

                    table.AddRow(fileId, fileName, status);
                    // refresh every 20 items
                    if (table.Rows.Count % 20 == 0)
                    {
                        ctx.Refresh();
                    }
                }
            });

        if (table.Rows.Count > 0)
            console.Write(table);

        return 0;
    }

    public class Settings(VectorIdMapper mapper) : ListCommandSettings
    {
        [Description("The ID or name of the vector store")]
        [CommandArgument(0, "<STORE>")]
        public required string Store { get; set; }

        [Description("Filter by status (in_progress, completed, failed, cancelled)")]
        [DefaultValue("completed")]
        [CommandOption("-f|--filter")]
        public required string Filter { get; set; } = "completed";

        public override ValidationResult Validate()
        {
            Store = mapper.MapName(Store);
            return base.Validate();
        }
    }
}
