using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Models;

[Description("View model details")]
[Service]
class ViewCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : Command<ViewCommand.Settings>
{
    public override int Execute(CommandContext context, Settings settings)
    {
        var result = oai.GetOpenAIModelClient().GetModel(settings.Id, cts.Token);

        return console.RenderJson(result.GetRawResponse(), settings, cts.Token);
    }

    public class Settings : JsonCommandSettings
    {
        [Description("The model ID")]
        [CommandArgument(0, "<ID>")]
        public required string Id { get; init; }
    }
}
