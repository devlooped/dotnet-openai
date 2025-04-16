using System.ComponentModel;
using OpenAI;
using OpenAI.VectorStores;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Vectors;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
[Description("Creates a vector store")]
class CreateCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : AsyncCommand<CreateCommand.CreateSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, CreateSettings settings)
    {
        var options = new VectorStoreCreationOptions();
        if (settings.Name.IsSet)
            options.Name = settings.Name.Value;

        foreach (var item in settings.Meta)
        {
            options.Metadata[item.Key] = item.Value;
        }

        if (settings.ExpiresAfter.IsSet)
        {
            options.ExpirationPolicy = new VectorStoreExpirationPolicy(VectorStoreExpirationAnchor.LastActiveAt, settings.ExpiresAfter.Value);
        }

        foreach (var id in settings.Files)
        {
            options.FileIds.Add(id);
        }


        var store = await console.Status().StartAsync("Creating vector store", async ctx =>
        {
            var result = await oai.GetVectorStoreClient().CreateVectorStoreAsync(settings.WaitForCompletion, options, cts.Token);
            return result;
        });

        var json = store.GetRawResponse().Content.ToString();
        if (store.Value is null)
        {
            console.MarkupLine($":cross_mark: Failed to create vector store:");
            console.RenderJson(json, "", settings.Monochrome, cts.Token);
            return -1;
        }

        if (settings.Json)
            return console.RenderJson(json, settings, cts.Token);

        console.Write(store.Value.Id);
        return 0;
    }

    public class CreateSettings : JsonCommandSettings
    {
        [Description("The name of the vector store")]
        [CommandOption("-n|--name [NAME]")]
        public FlagValue<string> Name { get; set; } = new();

        [Description("Sets the expiration policy to number of days after last active.")]
        [CommandOption("-e|--expires [DAYS]")]
        public FlagValue<int> ExpiresAfter { get; set; } = new();

        [Description("File IDs to add to the vector store")]
        [CommandOption("-f|--file")]
        public List<string> Files { get; set; } = new();

        [Description("Metadata to add to the vector store as KEY=VALUE")]
        [CommandOption("-m|--meta")]
        public IDictionary<string, string> Meta { get; set; } = new Dictionary<string, string>();

        [Description("Wait for the store to be created")]
        [DefaultValue(true)]
        [CommandOption("--wait")]
        public bool WaitForCompletion { get; set; } = true;
    }
}
