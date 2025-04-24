using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Vectors;

public class StoreCommandSettings(VectorIdMapper mapper) : JsonCommandSettings
{
    [Description("The ID or name of the vector store")]
    [CommandArgument(0, "<STORE>")]
    public required string Store { get; set; }

    public override ValidationResult Validate()
    {
        Store = mapper.MapName(Store);
        return base.Validate();
    }
}
