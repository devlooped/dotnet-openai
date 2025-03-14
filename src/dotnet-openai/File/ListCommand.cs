using System.ComponentModel;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;
using ClosedAI = OpenAI;

namespace Devlooped.OpenAI.File;

[Description("List files")]
class ListCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : AsyncCommand<ListCommand.ListSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, ListSettings settings)
    {
        if (!settings.Purpose.IsSet)
        {
            var unfiltered = await oai.GetOpenAIFileClient().GetFilesAsync();
            return console.RenderJson(unfiltered.Value, settings, cts.Token);
        }

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

        var filtered = await oai.GetOpenAIFileClient().GetFilesAsync(purpose);
        return console.RenderJson(filtered.Value, settings, cts.Token);
    }

    public class ListSettings : JsonCommandSettings
    {
        [DisplayValueDescription<FilePurpose>("Purpose of the file")]
        [CommandOption("-p|--purpose [PURPOSE]")]
        public FlagValue<FilePurpose> Purpose { get; set; } = new();
    }

    //public enum Order
    //{
    //    [Description("asc")] Ascending,
    //    [Description("desc")] Descending
    //}
}
