using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        => RenderJson(console, value, settings.JQ, settings.Monochrome, cancellation);

    public static int RenderJson<T>(this IAnsiConsole console, T value, FlagValue<string> jq, bool monochrome = false, CancellationToken cancellation = default)
        => RenderJson(console, value, jq.IsSet ? jq.Value : default, monochrome, cancellation);

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
        var info = new ProcessStartInfo(JQ.Path)
        {
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            RedirectStandardInput = true,
            UseShellExecute = false,
        };

        info.StandardInputEncoding = info.StandardErrorEncoding = info.StandardOutputEncoding = Encoding.UTF8;
        info.ArgumentList.Add("-r");
        info.ArgumentList.Add("--monochrome-output");

        var normalized = jq.ReplaceLineEndings().Trim();
        if (normalized.Contains(Environment.NewLine))
        {
            // get sha256 of the query, make a temp file with a windows-friendly filename derived from it
            // and persist the query. use the temp file as the query file input instead of a simple arg
            var hash = BitConverter.ToString(SHA256.HashData(Encoding.UTF8.GetBytes(normalized)));
            var queryFile = Path.Combine(Path.GetTempPath(), $"{hash}.jq");
            if (!System.IO.File.Exists(queryFile))
                System.IO.File.WriteAllText(queryFile, normalized);

            info.ArgumentList.Add("-f");
            info.ArgumentList.Add(queryFile);
        }
        else
        {
            info.ArgumentList.Add(jq.Trim());
        }

        var process = Process.Start(info);
        if (process == null)
        {
            console.MarkupLine($":cross_mark: Could not start JQ from {JQ.Path}");
            return -1;
        }

        if (!process.HasExited)
        {
            try
            {
                using var writer = process.StandardInput;
                writer.Write(json);
                writer.Close();
            }
            catch (IOException ioe)
            {
                console.WriteException(ioe);
                // The process might exit due to parsing of the query
            }
        }

        while (!cancellation.IsCancellationRequested && !process.WaitForExit(100))
        {
        }

        if (cancellation.IsCancellationRequested)
        {
            if (!process.HasExited)
                process.Kill();

            return -1;
        }

        var output = process.ExitCode != 0 ?
            process.StandardError.ReadToEnd() :
            process.StandardOutput.ReadToEnd();

        if (!string.IsNullOrEmpty(output))
        {
            WriteJson(console, monochrome, output);
        }

        return process.ExitCode;
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
