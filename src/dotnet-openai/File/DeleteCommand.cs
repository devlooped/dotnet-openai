using System.ComponentModel;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.File;

[Description("Delete a file by its ID.")]
class DeleteCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : AsyncCommand<DeleteCommand.DeleteSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, DeleteSettings settings)
    {
        var response = await oai.GetOpenAIFileClient().DeleteFileAsync(settings.ID);
        if (settings.Json)
        {
            return console.RenderJson(response.Value, settings, cts.Token);
        }
        else
        {
            console.Write(response.Value.Deleted.ToString().ToLowerInvariant());
            return 0;
        }
    }

    public class DeleteSettings : JsonCommandSettings
    {
        [Description("The ID of the file to delete")]
        [CommandArgument(0, "<ID>")]
        public required string ID { get; init; }
    }
}
