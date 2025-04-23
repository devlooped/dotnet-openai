using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using OpenAI.Files;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.File;

[Description("Upload a local file, specifying its purpose.")]
[Service]
class UploadCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : AsyncCommand<UploadCommand.UploadSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, UploadSettings settings)
    {
        using var file = System.IO.File.OpenRead(settings.File);

        var response = await oai.GetOpenAIFileClient().UploadFileAsync(
            file, Path.GetFileName(settings.File),
            new FileUploadPurpose(settings.Purpose));

        if (settings.Json)
        {
            return console.RenderJson(response.GetRawResponse(), settings, cts.Token);
        }
        else
        {
            console.Write(response.Value.Id);
            return 0;
        }
    }

    // https://platform.openai.com/docs/api-reference/files/create#files-create-purpose
    enum UploadPurpose
    {
        [Description("assistants")] Assistants,
        [Description("batch")] Batch,
        [Description("evals")] Evaluations,
        [Description("fine-tune")] FineTune,
        [Description("user_data")] UserData,
        [Description("vision")] Vision,
    }

    public class UploadSettings : JsonCommandSettings
    {
        [Description("File to upload")]
        [CommandArgument(0, "<FILE>")]
        [Required]
        public required string File { get; init; }

        [DisplayValueDescription<UploadPurpose>("Purpose of the file")]
        [DefaultValue("assistants")]
        [CommandOption("-p|--purpose <PURPOSE>")]
        public string Purpose { get; set; } = "assistants";
    }
}
