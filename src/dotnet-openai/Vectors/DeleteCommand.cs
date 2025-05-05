using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;
using static Devlooped.OpenAI.Vectors.DeleteCommand;

namespace Devlooped.OpenAI.Vectors;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
[Description("Delete a vector store by ID or name.")]
[Service]
public class DeleteCommand(OpenAIClient oai, IAnsiConsole console, VectorIdMapper mapper, CancellationTokenSource cts) : AsyncCommand<DeleteSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, DeleteSettings settings)
    {
        // Whether we need to delete all files too.
        if (settings.Files)
        {
            if (App.IsNonInteractive)
            {
                await Parallel.ForEachAsync(oai.GetVectorStoreClient().GetFileAssociationsAsync(settings.Store, cancellationToken: cts.Token), cts.Token,
                    async (file, token) => await oai.GetOpenAIFileClient().DeleteFileAsync(file.FileId, cancellationToken: token));
            }
            else
            {
                await console.Progress()
                    .Columns(
                    [
                        new TaskDescriptionColumn(),
                        new ProgressBarColumn(),
                        new PercentageColumn(),
                        new PrefixProgressColumn(new RemainingTimeColumn(), Emoji.Known.HourglassNotDone),
                        new PrefixProgressColumn(new ElapsedTimeColumn(), Emoji.Known.HourglassDone),
                    ])
                    .StartAsync(async ctx =>
                    {
                        var task = ctx.AddTask($"Deleting files associated with {settings.Store}");
                        task.MaxValue(oai.GetVectorStoreClient().GetVectorStore(settings.Store, cts.Token).Value.FileCounts.Total);
                        await Parallel.ForEachAsync(oai.GetVectorStoreClient().GetFileAssociationsAsync(settings.Store, cancellationToken: cts.Token), cts.Token,
                            async (file, token) =>
                            {
                                await oai.GetOpenAIFileClient().DeleteFileAsync(file.FileId, cancellationToken: token);
                                lock (task)
                                {
                                    task.Description($"Deleting {task.Value + 1} of {task.MaxValue} files associated with {settings.Store}");
                                    task.Increment(1);
                                }
                            });
                    });
            }
        }

        var response = oai.GetVectorStoreClient().DeleteVectorStore(settings.Store, cts.Token);
        var json = response.GetRawResponse();

        if (response.GetRawResponse().IsError)
        {
            console.MarkupLine($":cross_mark: Failed to delete vector store:");
            console.RenderJson(json, settings.Monochrome, cts.Token);
            return -1;
        }

        mapper.RemoveId(response.Value.VectorStoreId);

        if (settings.Json)
        {
            return console.RenderJson(response.GetRawResponse().Content.ToString(), settings, cts.Token);
        }
        else
        {
            console.WriteLine(response.Value.Deleted.ToString().ToLowerInvariant());
            return 0;
        }
    }

    public class DeleteSettings(VectorIdMapper mapper) : StoreCommandSettings(mapper)
    {
        [Description("Delete files associated with the vector store too.")]
        [CommandOption("--files")]
        public bool Files { get; set; }
    }
}