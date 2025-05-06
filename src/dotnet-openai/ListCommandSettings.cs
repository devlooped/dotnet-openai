using System.ClientModel.Primitives;
using System.ComponentModel;
using System.Text.Json.Nodes;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI;

public class ListCommandSettings : JsonCommandSettings
{
    [Description("C# range expression to flexibly slice results")]
    [CommandOption("--range [EXPRESSION]")]
    public FlagValue<string> Range { get; set; } = new();

    [Description("Number of items to skip from the results")]
    [CommandOption("--skip [SKIP]")]
    public FlagValue<int> Skip { get; set; } = new();

    [Description("Number of items to take from the results")]
    [CommandOption("--take [TAKE]")]
    public FlagValue<int> Take { get; set; } = new();

    public override ValidationResult Validate()
    {
        if (Range.IsSet && (Skip.IsSet || Take.IsSet))
        {
            return ValidationResult.Error("Cannot use --range with --skip or --take");
        }

        if (Range.IsSet)
        {
            // Attempt compilation with an arbitrary type to ensure expression is valid
            try
            {
                Compile<int>(Range.Value);
            }
            catch (CompilationErrorException ex)
            {
                return ValidationResult.Error("Invalid range expression: " + string.Join(", ", ex.Diagnostics));
            }
        }

        return base.Validate();
    }

    public List<T> ApplyFilters<T>(IEnumerable<T> values) => ApplyFilters(values.ToList());

    public JsonObject ApplyFilters(CollectionResult values)
    {
        var nodes = new List<JsonNode?>();
        var result = new JsonObject();
        var first = true;

        foreach (var page in values.GetRawPages())
        {
            var json = page.GetRawResponse().Content.ToString();
            var node = JsonNode.Parse(json);
            if (node is null || node.Root.GetValueKind() != System.Text.Json.JsonValueKind.Object ||
                node["object"]?.ToString() != "list")
                throw new ArgumentException("Expected a response JSON object with \"object\": \"list\"");

            if (first)
                result["object"] = node["object"]!.DeepClone();

            if (first && result["search_query"] is { } query)
                result["search_query"] = query.DeepClone();

            var data = node["data"];
            if (data is null || data.GetValueKind() != System.Text.Json.JsonValueKind.Array)
                throw new ArgumentException("Expected a response JSON object with \"data\": []");

            nodes.AddRange(((JsonArray)data).AsEnumerable());
            first = false;

            // See if we can apply the filter here without further loading.
            if (!Range.IsSet &&
                (!Skip.IsSet || Skip.Value <= nodes.Count) &&
                Take.IsSet && (Take.Value + Skip.Value <= nodes.Count))
            {
                // We can apply the filter here without further loading.
                nodes = [.. nodes.Skip(Skip.Value).Take(Take.Value)];
                result["data"] = new JsonArray([.. nodes.Where(x => x != null).Select(x => x!.DeepClone())]);
                return result;
            }
        }

        nodes = ApplyFilters(nodes);
        result["data"] = new JsonArray([.. nodes.Where(x => x != null).Select(x => x!.DeepClone())]);

        return result;
    }

    public List<T> ApplyFilters<T>(List<T> collection)
    {
        if (Range.IsSet)
        {
            return ApplyRangeAsync(collection, Range.Value).GetAwaiter().GetResult();
        }
        if (Skip.IsSet)
        {
            collection = [.. collection.Skip(Skip.Value)];
        }
        if (Take.IsSet)
        {
            collection = [.. collection.Take(Take.Value)];
        }
        return collection;
    }

    public string ApplyFilters(PipelineResponse response) => ApplyFilters(response.Content.ToString());

    public string ApplyFilters(string json)
    {
        var node = JsonNode.Parse(json);
        if (node is null || node.Root.GetValueKind() != System.Text.Json.JsonValueKind.Object)
            throw new ArgumentException("Expected a response JSON object");

        var data = node["data"];
        if (data is null || data.GetValueKind() != System.Text.Json.JsonValueKind.Array)
            throw new ArgumentException("Expected a response JSON object with \"data\": []");

        var list = ApplyFilters([.. ((JsonArray)data).AsEnumerable()]);
        return new JsonArray([.. list.Select(x => x.DeepClone())]).ToJsonString();
    }

    static ScriptRunner<List<T>> Compile<T>(string range)
        => CSharpScript.Create<List<T>>(
                $"return Collection[{range}];",
                ScriptOptions.Default.WithImports("System", "System.Collections.Generic"),
                globalsType: typeof(ScriptGlobals<T>)
            ).CreateDelegate();

    public static async Task<List<T>> ApplyRangeAsync<T>(List<T> collection, string range)
    {
        var script = Compile<T>(range);

        return await script(new ScriptGlobals<T> { Collection = collection });
    }

    public class ScriptGlobals<T>
    {
        public required List<T> Collection { get; set; }
    }
}
