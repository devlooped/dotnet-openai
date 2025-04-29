﻿using System.ComponentModel;
using Devlooped.Sponsors;
using DotNetConfig;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Sponsors;

[Description("Synchronizes your sponsorship manifest for [lime]devlooped[/]")]
class DevloopedSyncCommand(Config config, IGraphQueryClient client, IGitHubAppAuthenticator authenticator, IHttpClientFactory httpFactory)
    : SyncCommand<DevloopedSyncCommand.DevloopedSyncSettings>(config, client, authenticator, httpFactory)
{
    public class DevloopedSyncSettings : SyncSettings, ISponsorableSettings
    {
        public string[]? Sponsorable { get; set; } = ["devlooped"];
    }

    public override async Task<int> ExecuteAsync(CommandContext context, DevloopedSyncSettings settings)
    {
        if (context.ShouldRunWelcome(config, settings))
        {
            if (new WelcomeCommand(config).Execute(context, new WelcomeCommand.WelcomeSettings { ToS = settings.ToS }) is var result && result != 0)
                return result;
        }

        settings.Sponsorable = ["devlooped"];
        return await base.ExecuteAsync(context, settings);
    }
}
