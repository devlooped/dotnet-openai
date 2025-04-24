using System.ComponentModel;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Vectors;

public class FileCommandSettings(VectorIdMapper mapper) : StoreCommandSettings(mapper)
{
    [Description("File ID to add to the vector store")]
    [CommandArgument(1, "<FILE_ID>")]
    public required string FileId { get; init; }
}
