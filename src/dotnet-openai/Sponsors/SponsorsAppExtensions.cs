using System.ComponentModel;
using Devlooped.OpenAI.Sponsors;
using DotNetConfig;
using Spectre.Console.Cli;

namespace Devlooped.Sponsors;

static class SponsorsAppExtensions
{
    public static ICommandApp UseSponsors(this ICommandApp app)
    {
        app.Configure(config =>
        {
            config.AddBranch("sponsor", group =>
            {
                group.AddCommand<CheckCommand>("check");
                group.AddCommand<ConfigCommand>("config");
                group.AddCommand<DevloopedViewCommand>("view");
                group.AddCommand<DevloopedSyncCommand>("sync");
                group.AddCommand<WelcomeCommand>("welcome").IsHidden();
            });
        });
        return app;
    }

    static bool ShouldRunWelcome(ICommandApp app, Config config, ToSSettings settings)
    {
        // If we don't have ToS acceptance, we don't run any command other than welcome.
        var tos = config.TryGetBoolean("sponsorlink", "tos", out var completed) && completed;
        if (!tos && settings.ToS == true)
        {
            // Implicit acceptance on first run of another tool, like `sponsor sync --tos`
            app.Run(["sponsor", "config", ToSSettings.ToSOption, "--quiet"]);
            return false;
        }

        return tos == false;
    }

    [Description("Validates and displays the sponsor manifest for [lime]devlooped[/], if present")]
    class DevloopedViewCommand(ICommandApp app, Config config, IHttpClientFactory http) : ViewCommand<DevloopedViewCommand.DevloopedViewSettings>(http)
    {
        public class DevloopedViewSettings : ViewSettings, ISponsorableSettings
        {
            public string[]? Sponsorable { get; set; } = ["devlooped"];
        }

        public override async Task<int> ExecuteAsync(CommandContext context, DevloopedViewSettings settings)
        {
            if (ShouldRunWelcome(app, config, settings))
            {
                if (await app.RunAsync(["sponsor", "welcome"]) is var result && result != 0)
                    return result;
            }

            settings.Sponsorable = ["devlooped"];
            return await base.ExecuteAsync(context, settings);
        }
    }

    [Description("Synchronizes the sponsorship manifest for [lime]devlooped[/]")]
    public class DevloopedSyncCommand(ICommandApp app, Config config, IGraphQueryClient client, IGitHubAppAuthenticator authenticator, IHttpClientFactory httpFactory)
        : SyncCommand<DevloopedSyncCommand.DevloopedSyncSettings>(config, client, authenticator, httpFactory)
    {
        public class DevloopedSyncSettings : SyncSettings, ISponsorableSettings
        {
            public string[]? Sponsorable { get; set; } = ["devlooped"];
        }

        public override async Task<int> ExecuteAsync(CommandContext context, DevloopedSyncSettings settings)
        {
            if (ShouldRunWelcome(app, config, settings))
            {
                if (await app.RunAsync(["sponsor", "welcome"]) is var result && result != 0)
                    return result;
            }

            settings.Sponsorable = ["devlooped"];
            return await base.ExecuteAsync(context, settings);
        }
    }
}