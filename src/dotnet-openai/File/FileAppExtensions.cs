using Devlooped.OpenAI.File;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI;

static class FileAppExtensions
{
    public static ICommandApp UseFiles(this ICommandApp app)
    {
        app.Configure(config =>
        {
            config.AddBranch("file", group =>
            {
                group.AddCommand<UploadCommand>("upload");
                group.AddCommand<DeleteCommand>("delete");
                group.AddCommand<ListCommand>("list")
                    .WithExample("file list --jq '.[].id'")
                    .WithExample("file list --jq \".[] | select(.sizeInBytes > 100000) | .id\"");
                group.AddCommand<ViewCommand>("view");
            });
        });
        return app;
    }
}
