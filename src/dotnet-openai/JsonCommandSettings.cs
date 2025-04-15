using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;
using DataAnnotations = System.ComponentModel.DataAnnotations;

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

    [Description("Disable colors when rendering JSON to the console")]
    [DefaultValue(false)]
    [CommandOption("--monochrome")]
    public bool Monochrome { get; set; }

    public override ValidationResult Validate()
    {
        if (JQ.IsSet && !string.IsNullOrEmpty(JQ.Value))
            Json = true;

        var context = new DataAnnotations.ValidationContext(this);
        var results = new List<DataAnnotations.ValidationResult>();

        if (!DataAnnotations.Validator.TryValidateObject(this, context, results, true))
        {
            var errorMessages = new List<string>();
            foreach (var result in results)
            {
                if (result.MemberNames.Any() && result.ErrorMessage != null &&
                    result.MemberNames.First() is { } memberName &&
                    result.ErrorMessage.IndexOf(memberName) is var index &&
                    index >= 0 &&
                    GetType().GetProperty(memberName) is { } property &&
                    property.GetCustomAttributesData()
                            .FirstOrDefault(attr => attr.AttributeType == typeof(CommandArgumentAttribute) ||
                                                    attr.AttributeType == typeof(CommandOptionAttribute))
                            ?.ConstructorArguments
                            .FirstOrDefault(arg => arg.ArgumentType == typeof(string))
                            .Value?.ToString() is { } template)
                {
                    errorMessages.Add(template + ":" + result.ErrorMessage[(index + memberName.Length)..]);
                }
                else if (result.ErrorMessage != null)
                {
                    errorMessages.Add(result.ErrorMessage);
                }
            }
            return ValidationResult.Error(string.Join("\n", errorMessages));
        }

        return base.Validate();
    }
}
