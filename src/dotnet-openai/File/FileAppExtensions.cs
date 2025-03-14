using Devlooped.OpenAI.File;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI;

static class FileAppExtensions
{
    public static ICommandApp UseFiles(this ICommandApp app)
    {
        app.Configure(config =>
        {
            config.AddBranch("file", auth =>
            {
                auth.AddCommand<UploadCommand>("upload");
                auth.AddCommand<DeleteCommand>("delete");
                auth.AddCommand<ListCommand>("list")
                    .WithExample("file list --jq '.[].id'")
                    .WithExample("file list --jq \".[] | { id: .id, name: .filename, purpose: .purpose }\"")
                    .WithExample("file list --jq \".[] | select(.sizeInBytes > 100000) | .id\"");
                auth.AddCommand<ViewCommand>("view");
            });
        });
        return app;
    }
}
