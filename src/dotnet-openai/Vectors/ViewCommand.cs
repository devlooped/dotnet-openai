using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Vectors;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
[Description("View a store by its ID or name.")]
[Service]
public class ViewCommand(OpenAIClient oai, IAnsiConsole console, VectorIdMapper mapper, CancellationTokenSource cts) : Command<StoreCommandSettings>
{
    public override int Execute(CommandContext context, StoreCommandSettings settings)
    {
        var response = oai.GetVectorStoreClient().GetVectorStore(settings.Store, cts.Token);

        if (!response.GetRawResponse().IsError)
            mapper.SetId(response.Value.Name, response.Value.Id);

        if (settings.Json)
            return console.RenderJson(response.GetRawResponse(), settings, cts.Token);

        console.Write(response.Value.AsTable());
        return 0;
    }
}