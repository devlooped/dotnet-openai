using System.ClientModel.Primitives;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Devlooped;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Json;

namespace Devlooped.OpenAI;

static class JsonOutput
{
    static readonly JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
    {
        Converters =
        {
            new FilePurposeJsonConverter(),
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        },
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true,
    };

    public static int RenderJson<T>(this IAnsiConsole console, T value, JsonCommandSettings settings, CancellationToken cancellation)
        => RenderJson(console, value, settings.JQ.IsSet ? settings.JQ.Value : default, settings.Monochrome, cancellation);

    public static int RenderJson<T>(this IAnsiConsole console, T value, string? jq = default, bool monochrome = false, CancellationToken cancellation = default)
    {
        var json = JsonSerializer.Serialize(value, options);
        if (string.IsNullOrWhiteSpace(jq))
        {
            WriteJson(console, monochrome, json);
            return 0;
        }
        return RenderJson(console, json, jq, monochrome, cancellation);
    }

    public static int RenderJson(this IAnsiConsole console, PipelineResponse response, string? jq = default, bool monochrome = false, CancellationToken cancellation = default)
        => RenderJson(console, response.Content.ToString(), "", monochrome, cancellation);

    public static int RenderJson(this IAnsiConsole console, PipelineResponse response, bool monochrome = false, CancellationToken cancellation = default)
        => RenderJson(console, response.Content.ToString(), "", monochrome, cancellation);

    public static int RenderJson(this IAnsiConsole console, PipelineResponse response, JsonCommandSettings settings, CancellationToken cancellation)
        => RenderJson(console, response.Content.ToString(), settings.JQ, settings.Monochrome, cancellation);

    public static int RenderJson(this IAnsiConsole console, string json, JsonCommandSettings settings, CancellationToken cancellation)
        => RenderJson(console, json, settings.JQ, settings.Monochrome, cancellation);

    public static int RenderJson(this IAnsiConsole console, string json, FlagValue<string> jq, bool monochrome = false, CancellationToken cancellation = default)
    {
        if (!jq.IsSet || string.IsNullOrWhiteSpace(jq.Value))
        {
            WriteJson(console, monochrome, json);
            return 0;
        }
        return RenderJson(console, json, jq.Value, monochrome, cancellation);
    }

    public static int RenderJson(this IAnsiConsole console, string json, string jq, bool monochrome = false, CancellationToken cancellation = default)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            var results = Jq.Evaluate(jq.Trim(), doc.RootElement);
            var lines = new List<string>();
            foreach (var result in results)
            {
                if (cancellation.IsCancellationRequested)
                    return -1;

                // Raw output: strings are written without JSON quoting, other values as JSON
                lines.Add(result.ValueKind == JsonValueKind.String
                    ? result.GetString() ?? string.Empty
                    : result.GetRawText());
            }

            if (lines.Count > 0)
                WriteJson(console, monochrome, string.Join(Environment.NewLine, lines));

            return 0;
        }
        catch (JqException ex)
        {
            console.MarkupLine($":cross_mark: {Markup.Escape(ex.Message)}");
            return -1;
        }
    }

    static void WriteJson(IAnsiConsole console, bool monochrome, string json)
    {
        json = json.Trim();
        if (monochrome)
        {
            console.Write(json);
        }
        else
        {
            try
            {
                console.Write(new JsonText(json));
            }
            catch
            {
                console.Write(json);
            }
        }
    }
}
