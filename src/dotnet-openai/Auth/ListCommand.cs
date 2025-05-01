using System.ComponentModel;
using GitCredentialManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Auth;

[Description("Lists projects that have been authenticated and can be used with the login command")]
[Service]
public class ListCommand(IAnsiConsole console, IConfiguration configuration, ICredentialStore store, CancellationTokenSource cts) : Command<ListCommand.ListSettings>
{
    public override int Execute(CommandContext context, ListSettings settings)
    {
        var currentKey = store.Get(ThisAssembly.Constants.ServiceUri, "_CURRENT_")?.Password
            ?? configuration["OPENAI_API_KEY"]
            ?? "";

        var projects = store
            .GetAccounts(ThisAssembly.Constants.ServiceUri)
            .Where(x => x != "_CURRENT_")
            .Select(x => new { Project = x, Token = store.Get(ThisAssembly.Constants.ServiceUri, x)?.Password });

        if (settings.Json)
        {
            if (settings.ShowToken)
                return console.RenderJson(projects, settings, cts.Token);
            else
                return console.RenderJson(projects.Select(x => x.Project), settings, cts.Token);
        }

        // render as a table otherwise
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[lime]Project[/]");

        if (settings.ShowToken)
        {
            table.AddColumn("[lime]Token[/]");
            foreach (var project in projects)
            {
                table.AddRow([project.Token == currentKey ? $"[bold]{project.Project}[/]" : project.Project, project.Token ?? ""]);
            }
        }
        else
        {
            foreach (var project in projects)
            {
                table.AddRow(project.Token == currentKey ? $"[bold]{project.Project}[/]" : project.Project);
            }
        }

        if (table.Rows.Count == 0)
            console.MarkupLine("[red]No projects have been authenticated yet.[/]");
        else
            console.Write(table);

        return 0;
    }

    public class ListSettings : JsonCommandSettings
    {
        [Description("Display the auth token of each project")]
        [DefaultValue(false)]
        [CommandOption("--show-token")]
        public bool ShowToken { get; set; }
    }
}
