using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using OpenAI.VectorStores;
using Spectre.Console;
using Spectre.Console.Cli;
using static Devlooped.OpenAI.Vectors.ModifyCommand;

namespace Devlooped.OpenAI.Vectors;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
[Description("Modify a vector store")]
[Service]
public class ModifyCommand(OpenAIClient oai, IAnsiConsole console, VectorIdMapper mapper, CancellationTokenSource cts) : AsyncCommand<ModifySettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, ModifySettings settings)
    {
        var options = new VectorStoreModificationOptions();
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

        var response = await oai.GetVectorStoreClient().ModifyVectorStoreAsync(settings.Store, options, cts.Token);
        if (response.Value is null)
        {
            console.MarkupLine($":cross_mark: Failed to modify vector store");
            return -1;
        }

        while (response.Value.Status != VectorStoreStatus.Completed)
        {
            await Task.Delay(200, cts.Token);
            await oai.GetVectorStoreClient().ModifyVectorStoreAsync(settings.Store, options, cts.Token);
        }

        if (settings.Name.IsSet)
            mapper.SetId(response.Value.Name, response.Value.Id);

        if (settings.Json)
            return console.RenderJson(response.GetRawResponse(), settings, cts.Token);

        console.Write(response.Value.AsTable());
        return 0;
    }

    public class ModifySettings(VectorIdMapper mapper) : StoreCommandSettings(mapper)
    {
        [Description("The name of the vector store")]
        [CommandOption("-n|--name [NAME]")]
        public FlagValue<string> Name { get; set; } = new();

        [Description("Sets the expiration policy to number of days after last active.")]
        [CommandOption("-e|--expires [DAYS]")]
        public FlagValue<int> ExpiresAfter { get; set; } = new();

        [Description("Metadata to add to the vector store as KEY=VALUE")]
        [CommandOption("-m|--meta")]
        public IDictionary<string, string> Meta { get; set; } = new Dictionary<string, string>();
    }
}
