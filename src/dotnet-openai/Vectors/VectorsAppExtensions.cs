using Devlooped.OpenAI.Vectors;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI;

static class VectorsAppExtensions
{
    public static ICommandApp UseVectors(this ICommandApp app)
    {
        app.Configure(config =>
        {
            config.AddBranch("vector", group =>
            {
                group.AddCommand<CreateCommand>("create")
                    .WithExample("vector create --name mystore --meta 'key1=value1' --meta 'key2=value' --file asdf123 --file qwer456");

                group.AddCommand<ModifyCommand>("modify");
                group.AddCommand<DeleteCommand>("delete");
                group.AddCommand<ListCommand>("list");
                group.AddCommand<ViewCommand>("view");
                group.AddCommand<SearchCommand>("search")
                    .WithExample("vector search mystore \"what's the return policy on headphones?\" --score 0.7 --filter region=us")
                    .WithExample("vector search mystore \"most popular stores?\" -f region=us -f popularity>=80");

                group.AddBranch("file", file =>
                {
                    file.SetDescription("Vector store files operations");
                    file.AddCommand<FileAddCommand>("add")
                        .WithExample("file add mystore store.md -a region=us")
                        .WithExample("file add mystore nypop.md -a region=us -a popularity=90");
                    file.AddCommand<FileRemoveCommand>("delete");
                    file.AddCommand<FileListCommand>("list");
                    file.AddCommand<FileViewCommand>("view");
                });
            });
        });
        return app;
    }
}
