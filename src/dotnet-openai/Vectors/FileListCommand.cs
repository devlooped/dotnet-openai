using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using OpenAI;
using OpenAI.VectorStores;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Vectors;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
[Description("List files associated with vector store")]
class FileListCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : Command<FileListCommand.Settings>
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

        var result = oai.GetVectorStoreClient().GetFileAssociations(settings.StoreId, options, cts.Token);
        if (result is null)
        {
            console.MarkupLine($":cross_mark: Failed to list vector stores");
            return -1;
        }

        var payload = result
            .GetRawPages()
            .SelectMany(x => JsonSerializer.Deserialize<VectorStoreFileAssociationData>(
                x.GetRawResponse().Content.ToString(), jsonOptions)!.Data)
            .ToList();

        return console.RenderJson(payload, settings, cts.Token);

        // TODO: provide table rendering for non-json output
        //console.Write(store.Id);
        //return 0;
    }

    record VectorStoreFileAssociationData(List<JsonNode> Data);

    public class Settings : JsonCommandSettings
    {
        [Description("The ID of the vector store")]
        [CommandArgument(0, "<STORE_ID>")]
        public required string StoreId { get; init; }

        [Description("Filter by status (in_progress, completed, failed, cancelled)")]
        [DefaultValue("completed")]
        [CommandOption("-f|--filter")]
        public required string Filter { get; set; } = "completed";
    }
}
