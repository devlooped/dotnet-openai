using System.ClientModel;
using System.ComponentModel;
using Humanizer;
using OpenAI;
using OpenAI.Files;
using Spectre.Console;
using Spectre.Console.Cli;
using ClosedAI = OpenAI;

namespace Devlooped.OpenAI.File;

[Description("List files")]
class ListCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : AsyncCommand<ListCommand.ListSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, ListSettings settings)
    {
        ClientResult<OpenAIFileCollection> result;

        if (!settings.Purpose.IsSet)
        {
            result = await oai.GetOpenAIFileClient().GetFilesAsync();
        }
        else
        {
            var purpose = settings.Purpose.Value switch
            {
                FilePurpose.Assistants => ClosedAI.Files.FilePurpose.Assistants,
                FilePurpose.AssistantsOutput => ClosedAI.Files.FilePurpose.AssistantsOutput,
                FilePurpose.Batch => ClosedAI.Files.FilePurpose.Batch,
                FilePurpose.BatchOutput => ClosedAI.Files.FilePurpose.BatchOutput,
                FilePurpose.Evaluations => ClosedAI.Files.FilePurpose.Evaluations,
                FilePurpose.FineTune => ClosedAI.Files.FilePurpose.FineTune,
                FilePurpose.FineTuneResults => ClosedAI.Files.FilePurpose.FineTuneResults,
                FilePurpose.UserData => ClosedAI.Files.FilePurpose.UserData,
                FilePurpose.Vision => ClosedAI.Files.FilePurpose.Vision,
                var value => throw new InvalidOperationException($"Invalid purpose '{DisplayValue.ToString(value)}'"),
            };

            result = await oai.GetOpenAIFileClient().GetFilesAsync(purpose);
        }

        if (settings.Json)
            return console.RenderJson(settings.ApplyFilters(result.GetRawResponse()), settings, cts.Token);

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[lime]ID[/]")
            .AddColumn("[lime]Name[/]")
            .AddColumn("[lime]Status[/]")
            .AddColumns("[lime]Size[/]");

        // Adding live for better user feedback on amount of data.
        console.Live(table)
            // Clear the "progress" table since we render the full one at the end.
            .AutoClear(true)
            .Start(ctx =>
            {
                ctx.UpdateTarget(table);
                foreach (var file in settings.ApplyFilters(result.Value.ToList()))
                {
                    table.AddRow(file.Id, file.Filename, file.Status.ToString(), file.SizeInBytes.GetValueOrDefault().Bytes().Humanize());
                    // refresh every 20 items
                    if (table.Rows.Count % 20 == 0)
                    {
                        ctx.Refresh();
                    }
                }
            });

        console.Write(table);

        return 0;
    }

    public class ListSettings : ListCommandSettings
    {
        [DisplayValueDescription<FilePurpose>("Purpose of the file")]
        [CommandOption("-p|--purpose [PURPOSE]")]
        public FlagValue<FilePurpose> Purpose { get; set; } = new();
    }
}
