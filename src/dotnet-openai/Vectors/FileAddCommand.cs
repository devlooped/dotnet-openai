using System.ComponentModel;
using System.Threading.Tasks;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Vectors;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
[Description("Add file to vector store")]
class FileAddCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : Command<FileCommandSettings>
{
    public override int Execute(CommandContext context, FileCommandSettings settings)
    {
        var response = oai.GetVectorStoreClient().AddFileToVectorStore(settings.StoreId, settings.FileId, true, cts.Token);

        return console.RenderJson(response.Value, settings, cts.Token);
    }
}