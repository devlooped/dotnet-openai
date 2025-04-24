using Humanizer;
using OpenAI.VectorStores;
using Spectre.Console;

namespace Devlooped.OpenAI.Vectors;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
static class VectorsExtensions
{
    public static Table AsTable(this VectorStore store)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[lime]ID[/]")
            .AddColumn("[lime]Name[/]")
            .AddColumn("[lime]Files[/]", x => x.RightAligned())
            .AddColumn("[lime]Size[/]", x => x.RightAligned())
            .AddColumn("[lime]Last Active[/]");

        table.AddRow(
            store.Id,
            store.Name,
            store.FileCounts.Total.ToString(),
            store.UsageBytes.Bytes().Humanize(),
            store.LastActiveAt?.ToString("yyyy-MM-dd T HH:mm") ?? ""
        );

        return table;
    }
}
