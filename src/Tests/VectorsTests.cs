using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console.Testing;

namespace Devlooped.OpenAI;

public class VectorsTests(ITestOutputHelper output)
{
    [Fact]
    public async Task ViewBadId()
    {
        var console = new TestConsole();
        var app = App.Create(console.Wrap());

        Assert.Equal(-1, await app.RunAsync(["vector", "view", "--id", "non-existent-id"]));

        output.WriteLine(console.Output);
    }

    [Fact]
    public async Task ViewNonExistentId()
    {
        var console = new TestConsole();
        var app = App.Create(console.Wrap());

        Assert.Equal(-1, await app.RunAsync(["vector", "view", "--id", "vs_foo"]));

        output.WriteLine(console.Output);
    }
}
