using System.ComponentModel;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.File;

[Description("View a file by its ID.")]
class ViewCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : AsyncCommand<ViewCommand.ViewSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, ViewSettings settings)
    {
        var response = await oai.GetOpenAIFileClient().GetFileAsync(settings.ID);

        return console.RenderJson(response.Value, settings, cts.Token);
    }

    public class ViewSettings : JsonCommandSettings
    {
        [Description("The ID of the file to show")]
        [CommandArgument(0, "<ID>")]
        public required string ID { get; init; }
    }
}