using System.ComponentModel;
using GitCredentialManager;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Auth;

[Description(
    $$$"""
    Authenticate to OpenAI. 
    
    Supports API key autentication using the Git Credential Manager for storage.

    Switch easily between keys by just specifying the project name after initial login with `--with-token`.

    Alternatively, {{{ThisAssembly.Project.ToolCommandName}}} will use the authentication token found in environment variables with the name `OPENAI_API_KEY`.
    This method is most suitable for "headless" use such as in automation.

    For example, to use {{{ThisAssembly.Project.ToolCommandName}}} in GitHub Actions, add `OPENAI_API_KEY: ${{ secrets.OPENAI_API_KEY }}` to "env".
    """)]
[Service]
public class LoginCommand(IAnsiConsole console, ICredentialStore store) : Command<LoginCommand.LoginSettings>
{
    public override int Execute(CommandContext context, LoginSettings settings)
    {
        var token = store.Get("https://api.openai.com", settings.Project)?.Password;

        if (settings.WithToken)
        {
            using var reader = new StreamReader(Console.OpenStandardInput());
            token = reader.ReadToEnd().Trim();

            if (string.IsNullOrEmpty(token))
            {
                console.MarkupLine("[red]No token found via standard input.[/]");
                return -1;
            }
        }
        else if (string.IsNullOrEmpty(token))
        {
            console.MarkupLine("[red]No token found for project '{0}'. Run with --with-token instead.[/]", settings.Project);
            return -2;
        }

        store.AddOrUpdate("https://api.openai.com", settings.Project, token);
        // Last login sets the current one.
        store.AddOrUpdate("https://api.openai.com", "_CURRENT_", token);

        return 0;
    }

    public class LoginSettings : CommandSettings
    {
        [Description("OpenAI project the API key belongs to")]
        [CommandArgument(0, "<project>")]
        public required string Project { get; set; }

        [Description("Read token from standard input")]
        [DefaultValue(false)]
        [CommandOption("--with-token")]
        public bool WithToken { get; set; }
    }
}
