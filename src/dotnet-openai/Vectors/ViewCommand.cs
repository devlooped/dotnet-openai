using System.ComponentModel;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Vectors;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
[Description("View a store by its ID.")]
class ViewCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : Command<ViewCommand.ViewSettings>
{
    public override int Execute(CommandContext context, ViewSettings settings)
    {
        var response = oai.GetVectorStoreClient().GetVectorStore(settings.ID, cts.Token);

        return console.RenderJson(response.Value, settings, cts.Token);
    }

    public class ViewSettings : JsonCommandSettings
    {
        [Description("The ID of the vector store")]
        [CommandArgument(0, "<ID>")]
        public required string ID { get; init; }
    }
}