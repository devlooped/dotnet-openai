using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Models;

[Description("List available models")]
[Service]
public class ListCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : Command<ListCommandSettings>
{
    public override int Execute(CommandContext context, ListCommandSettings settings)
    {
        var result = oai.GetOpenAIModelClient().GetModels(cts.Token);
        if (result is null)
        {
            console.MarkupLine($":cross_mark: Failed to list models");
            return -1;
        }

        if (settings.Json)
            return console.RenderJson(settings.ApplyFilters(result.GetRawResponse()), settings, cts.Token);

        var table = new Table().Border(TableBorder.Rounded)
                               .AddColumn("[lime]ID[/]")
                               .AddColumn("[lime]Owner[/]");

        foreach (var model in settings.ApplyFilters(result.Value).OrderBy(x => x.Id))
        {
            table.AddRow(model.Id, model.OwnedBy);
        }

        console.Write(table);
        return 0;
    }
}
