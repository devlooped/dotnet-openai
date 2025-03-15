using Devlooped.OpenAI.Models;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI;

static class ModelAppExtensions
{
    public static ICommandApp UseModels(this ICommandApp app)
    {
        app.Configure(config =>
        {
            config.AddBranch("model", group =>
            {
                group.AddCommand<ListCommand>("list");
                group.AddCommand<ViewCommand>("view");
            });
        });
        return app;
    }
}
