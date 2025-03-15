// See https://aka.ms/new-console-template for more information
using Devlooped.OpenAI.Auth;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI;

static class AuthAppExtensions
{
    public static ICommandApp UseAuth(this ICommandApp app)
    {
        app.Configure(config =>
        {
            config.AddBranch("auth", group =>
            {
                group.AddCommand<LoginCommand>("login");
                group.AddCommand<LogoutCommand>("logout");
                group.AddCommand<StatusCommand>("status");
                group.AddCommand<TokenCommand>("token");
            });
        });
        return app;
    }
}
