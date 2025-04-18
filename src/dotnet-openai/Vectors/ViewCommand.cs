using System.ComponentModel;
using Humanizer;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Vectors;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
[Description("View a store by its ID.")]
class ViewCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : Command<ViewCommand.ViewSettings>
{
    public override int Execute(CommandContext context, ViewSettings settings)
    {
        var response = oai.GetVectorStoreClient().GetVectorStore(settings.ID, cts.Token);

        if (settings.Json)
            return console.RenderJson(response.GetRawResponse(), settings, cts.Token);

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[lime]ID[/]")
            .AddColumn("[lime]Name[/]")
            .AddColumn("[lime]Files[/]", x => x.RightAligned())
            .AddColumn("[lime]Size[/]", x => x.RightAligned())
            .AddColumn("[lime]Last Active[/]");

        table.AddRow(
            response.Value.Id,
            response.Value.Name,
            response.Value.FileCounts.Total.ToString(),
            response.Value.UsageBytes.Bytes().Humanize(),
            response.Value.LastActiveAt?.ToString("yyyy-MM-dd T HH:mm") ?? ""
        );

        console.Write(table);
        return 0;
    }

    public class ViewSettings : JsonCommandSettings
    {
        [Description("The ID of the vector store")]
        [CommandArgument(0, "<ID>")]
        public required string ID { get; init; }
    }
}