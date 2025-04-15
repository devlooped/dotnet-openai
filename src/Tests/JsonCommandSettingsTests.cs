using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI;

public class JsonCommandSettingsTests(ITestOutputHelper output)
{
    [Fact]
    public void WhenValidatingMultipleMembersThenRendersOptions()
    {
        var settings = new ArgumentOptionSettings
        {
            Argument = 11,
            Option = "abcde"
        };

        var result = settings.Validate();

        Assert.False(result.Successful);
        output.WriteLine(result.Message);
    }

    public class ArgumentOptionSettings : JsonCommandSettings
    {
        [CommandArgument(0, "<ARG>")]
        [Range(1, 10)]
        public int Argument { get; set; }
        [CommandOption("-o|--option <OPTION>")]
        [MaxLength(4)]
        public string Option { get; set; } = "";
    }
}
