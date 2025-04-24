using System.ComponentModel;
using GitCredentialManager;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Auth;

[Description("Log out of api.openai.com")]
[Service]
public class LogoutCommand(ICredentialStore store, IAnsiConsole console) : Command
{
    public override int Execute(CommandContext context)
    {
        foreach (var account in store.GetAccounts("https://api.openai.com"))
        {
            store.Remove("https://api.openai.com", account);
            if (account != "_CURRENT_")
                console.MarkupLine($"  :check_mark:  Logged out {account}");
        }

        return 0;
    }
}
