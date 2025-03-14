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
            config.AddBranch("auth", auth =>
            {
                auth.AddCommand<LoginCommand>("login");
                auth.AddCommand<LogoutCommand>("logout");
                auth.AddCommand<StatusCommand>("status");
                auth.AddCommand<TokenCommand>("token");
            });
        });
        return app;
    }
}
