using System.ComponentModel;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Vectors;

public class FileCommandSettings : JsonCommandSettings
{
    [Description("The ID of the vector store")]
    [CommandArgument(0, "<STORE_ID>")]
    public required string StoreId { get; init; }

    [Description("File ID to add to the vector store")]
    [CommandArgument(1, "<FILE_ID>")]
    public required string FileId { get; init; }
}
