using System.ComponentModel;
using GitCredentialManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Auth;

[Description($"Print the auth token {ThisAssembly.Project.ToolCommandName} is configured to use")]
[Service]
public class TokenCommand(IConfiguration configuration, ICredentialStore store, IAnsiConsole console) : Command
{
    public override int Execute(CommandContext context)
    {
        var apikey = store.Get("https://api.openai.com", "_CURRENT_")?.Password
            ?? configuration["OPENAI_API_KEY"]
            ?? "";

        if (string.IsNullOrEmpty(apikey))
        {
            console.MarkupLine($"no auth token");
            return -1;
        }

        console.WriteLine(apikey);
        return 0;
    }
}
