using System.ClientModel;
using System.ComponentModel;
using GitCredentialManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Auth;

[Description("Shows the current authentication status")]
[Service]
public class StatusCommand(IAnsiConsole console, IConfiguration configuration, ICredentialStore store, OpenAIClient client) : AsyncCommand<StatusCommand.StatusSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, StatusSettings settings)
    {
        var apikey = store.Get("https://api.openai.com", "_CURRENT_")?.Password
            ?? configuration["OPENAI_API_KEY"]
            ?? "";

        if (string.IsNullOrEmpty(apikey))
        {
            console.MarkupLine($"  :cross_mark: You are not logged in. Run {ThisAssembly.Project.ToolCommandName} auth login to authenticate.");
            return -1;
        }

        // get available models to test the API
        try
        {
            await client.GetOpenAIModelClient().GetModelsAsync();

            console.MarkupLine($"  :check_mark_button: Logged in to api.openai.com");
            if (settings.ShowToken)
                console.MarkupLine($"  :check_mark_button: Token: {apikey}");

            return 0;
        }
        catch (ClientResultException cre)
        {
            var lines = cre.Message.ReplaceLineEndings().Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
            console.MarkupLine($"  :cross_mark: Authentication failed: {lines[0]}");
            foreach (var line in lines.Skip(1))
                console.MarkupLine($"     {line}");

            return -2;
        }
    }

    public class StatusSettings : CommandSettings
    {
        [Description("Display the auth token")]
        [DefaultValue(false)]
        [CommandOption("--show-token")]
        public bool ShowToken { get; set; }
    }
}
