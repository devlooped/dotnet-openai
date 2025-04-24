using System.ComponentModel;
using Devlooped.Sponsors;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;
using static Devlooped.OpenAI.Sponsors.CheckCommand;

namespace Devlooped.OpenAI.Sponsors;

[Description("Checks the current sponsorship status with [lime]devlooped[/], entirely offline")]
[Service]
public class CheckCommand(IAnsiConsole console) : Command<CheckSettings>
{
    public class CheckSettings : CommandSettings
    {
        [CommandOption("-q|--quiet", IsHidden = true)]
        public bool Quiet { get; set; }
    }

    public override int Execute(CommandContext context, CheckSettings settings)
    {
        // Don't render anything if not interactive, so we don't disrupt usage in CI for example.
        // In GH actions, console input/output is redirected, for example, and output is redirected 
        // when using the app in a powershell pipeline or capturing its output in a variable.
        if (App.IsNonInteractive)
            return 0;

        // Check if we have a manifest at all
        var jwtPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".sponsorlink", "github", "devlooped.jwt");

        var project = $"[blueviolet][link=https://github.com/devlooped/dotnet-openai]{ThisAssembly.Info.Product}[/][/]";
        var link = "[link=https://github.com/devlooped/#sponsorlink]devlooped[/]";

        if (!System.IO.File.Exists(jwtPath))
            return MarkupLine(ThisAssembly.Strings.Unknown.Message(project, link));

        var manifest = SponsorLink.GetManifest("devlooped", ThisAssembly.Metadata.Funding.GitHub.devlooped, true);
        if (manifest.Status == ManifestStatus.Unknown || manifest.Status == ManifestStatus.Invalid)
            return MarkupLine(ThisAssembly.Strings.Unknown.Message(project, link));

        if (manifest.Status == ManifestStatus.Expired)
            return MarkupLine(ThisAssembly.Strings.Expired.Message);

        if (settings.Quiet)
            return 0;

        if (manifest.Principal.IsInRole("team"))
            return MarkupLine(ThisAssembly.Strings.Team.Message(link));

        if (manifest.Principal.IsInRole("user"))
            return MarkupLine(ThisAssembly.Strings.Sponsor.Message(project));

        if (manifest.Principal.IsInRole("contrib"))
            return MarkupLine(ThisAssembly.Strings.Contributor.Message(link));

        if (manifest.Principal.IsInRole("org"))
            return MarkupLine(ThisAssembly.Strings.Sponsor.Message(project));

        if (manifest.Principal.IsInRole("oss"))
            return MarkupLine(ThisAssembly.Strings.OpenSource.Message);

        return MarkupLine(ThisAssembly.Strings.Unknown.Message(project, link));
    }

    int MarkupLine(string message)
    {
        console.WriteLine();
        console.MarkupLine(message);
        console.WriteLine();
        return 0;
    }
}
