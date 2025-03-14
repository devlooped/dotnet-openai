using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI;

public class JsonCommandSettings : CommandSettings
{
    [Description("Filter JSON output using a jq expression")]
    [CommandOption("--jq [expression]")]
    public FlagValue<string> JQ { get; set; } = new();

    [Description("Output as JSON. Implied when using --jq")]
    [DefaultValue(false)]
    [CommandOption("--json")]
    public bool Json { get; set; }

    [Description("Pretty-print the JSON to the console")]
    [DefaultValue(false)]
    [CommandOption("--pretty")]
    public bool PrettyPrint { get; set; }

    public override ValidationResult Validate()
    {
        if (JQ.IsSet && !string.IsNullOrEmpty(JQ.Value))
            Json = true;

        return base.Validate();
    }
}
