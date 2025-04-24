using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Vectors;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
[Description("Delete a vector store by ID or name.")]
[Service]
public class DeleteCommand(OpenAIClient oai, IAnsiConsole console, VectorIdMapper mapper, CancellationTokenSource cts) : Command<StoreCommandSettings>
{
    public override int Execute(CommandContext context, StoreCommandSettings settings)
    {
        var response = oai.GetVectorStoreClient().DeleteVectorStore(settings.Store, cts.Token);
        var json = response.GetRawResponse();

        if (response.GetRawResponse().IsError)
        {
            console.MarkupLine($":cross_mark: Failed to delete vector store:");
            console.RenderJson(json, settings.Monochrome, cts.Token);
            return -1;
        }

        mapper.RemoveId(response.Value.VectorStoreId);

        if (settings.Json)
        {
            return console.RenderJson(response.GetRawResponse().Content.ToString(), settings, cts.Token);
        }
        else
        {
            console.WriteLine(response.Value.Deleted.ToString().ToLowerInvariant());
            return 0;
        }
    }
}