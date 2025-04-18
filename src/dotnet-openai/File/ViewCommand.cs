using System.ComponentModel;
using Humanizer;
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

        if (settings.Json)
            return console.RenderJson(response.GetRawResponse(), settings, cts.Token);

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[lime]ID[/]")
            .AddColumn("[lime]Name[/]")
            .AddColumn("[lime]Status[/]")
            .AddColumns("[lime]Size[/]");

        table.AddRow(response.Value.Id, response.Value.Filename,
            response.Value.Status.ToString(),
            response.Value.SizeInBytes.GetValueOrDefault().Bytes().Humanize());

        console.Write(table);

        return 0;
    }

    public class ViewSettings : JsonCommandSettings
    {
        [Description("The ID of the file to show")]
        [CommandArgument(0, "<ID>")]
        public required string ID { get; init; }
    }
}