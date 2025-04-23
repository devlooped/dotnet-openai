using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Vectors;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
[Description("View file association to a vector store")]
[Service]
class FileViewCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : Command<FileCommandSettings>
{
    public override int Execute(CommandContext context, FileCommandSettings settings)
    {
        var response = oai.GetVectorStoreClient().GetFileAssociation(settings.StoreId, settings.FileId, cts.Token);

        return console.RenderJson(response.GetRawResponse(), settings, cts.Token);
    }
}