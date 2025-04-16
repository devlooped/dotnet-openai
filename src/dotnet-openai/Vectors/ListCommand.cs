using System.ComponentModel;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Vectors;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
[Description("List vector stores")]
class ListCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : Command<ListCommandSettings>
{
    public override int Execute(CommandContext context, ListCommandSettings settings)
    {
        var result = oai.GetVectorStoreClient().GetVectorStores();
        if (result is null)
        {
            console.MarkupLine($":cross_mark: Failed to list vector stores");
            return -1;
        }

        return console.RenderJson(result, settings, cts.Token);

        // TODO: provide table rendering for non-json output
        //console.Write(store.Id);
        //return 0;
    }
}
