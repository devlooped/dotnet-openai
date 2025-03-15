using System.ComponentModel;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Vectors;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
[Description("Delete a vector store by ID.")]
class DeleteCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : Command<DeleteCommand.DeleteSettings>
{
    public override int Execute(CommandContext context, DeleteSettings settings)
    {
        var response = oai.GetVectorStoreClient().DeleteVectorStore(settings.ID, cts.Token);

        return console.RenderJson(response.Value, settings, cts.Token);
    }

    public class DeleteSettings : JsonCommandSettings
    {
        [Description("The ID of the vector store")]
        [CommandArgument(0, "<ID>")]
        public required string ID { get; init; }
    }
}