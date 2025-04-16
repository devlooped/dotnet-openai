using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Devlooped.OpenAI;

public class RangeTests
{
    [Fact]
    public async Task ApplyRange()
    {
        var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var rangeString = "2..5";
        var result = await ApplyRangeAsync(list, rangeString);

        Assert.Equal(new List<int> { 3, 4, 5 }, result);
    }

    [Fact]
    public async Task ApplyRangeFromEnd()
    {
        var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var rangeString = "5..^2";
        var result = await ApplyRangeAsync(list, rangeString);

        Assert.Equal(new List<int> { 6, 7, 8 }, result);
    }

    [Fact]
    public async Task ApplyRangeToEnd()
    {
        var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var rangeString = "5..";
        var result = await ApplyRangeAsync(list, rangeString);

        Assert.Equal(new List<int> { 6, 7, 8, 9, 10 }, result);
    }

    [Fact]
    public async Task ThrowsInvalidRangeExpression()
    {
        var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var rangeString = "invalid..range";

        var ex = await Assert.ThrowsAsync<CompilationErrorException>(async () => await ApplyRangeAsync(list, rangeString));
    }

    public static async Task<List<T>> ApplyRangeAsync<T>(List<T> collection, string rangeString)
    {
        var script = CSharpScript.Create<List<T>>(
            $"return Collection[{rangeString}];",
            ScriptOptions.Default.WithImports("System", "System.Collections.Generic"),
            globalsType: typeof(ScriptGlobals<T>)
        ).CreateDelegate();

        var globals = new ScriptGlobals<T> { Collection = collection };
        return await script(globals);
    }

    public class ScriptGlobals<T>
    {
        public required List<T> Collection { get; set; }
    }
}
