using System.ComponentModel;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Models;

[Description("List available models")]
class ListCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : Command<JsonCommandSettings>
{
    public override int Execute(CommandContext context, JsonCommandSettings settings)
    {
        var result = oai.GetOpenAIModelClient().GetModels(cts.Token);
        if (result is null)
        {
            console.MarkupLine($":cross_mark: Failed to list models");
            return -1;
        }

        return console.RenderJson(result.Value, settings, cts.Token);
    }
}
