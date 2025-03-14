using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using ClosedAI = OpenAI;

namespace Devlooped.OpenAI;

// https://platform.openai.com/docs/api-reference/files/object#files/object-purpose

[TypeConverter(typeof(DisplayValueConverter<FilePurpose>))]
public enum FilePurpose
{
    [DisplayValue("assistants")] Assistants,
    [DisplayValue("assistants_output")] AssistantsOutput,
    [DisplayValue("batch")] Batch,
    [DisplayValue("batch_output")] BatchOutput,
    [DisplayValue("evals")] Evaluations,
    [DisplayValue("fine-tune")] FineTune,
    [DisplayValue("fine-tune-results")] FineTuneResults,
    [DisplayValue("user_data")] UserData,
    [DisplayValue("vision")] Vision,
}

/// <summary>
/// Converts from the OpenAI-enum to our own enum's display value
/// </summary>
public class FilePurposeJsonConverter : JsonConverter<ClosedAI.Files.FilePurpose>
{
    public override void Write(Utf8JsonWriter writer, ClosedAI.Files.FilePurpose value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(DisplayValue.ToString(value switch
        {
            ClosedAI.Files.FilePurpose.Assistants => FilePurpose.Assistants,
            ClosedAI.Files.FilePurpose.AssistantsOutput => FilePurpose.AssistantsOutput,
            ClosedAI.Files.FilePurpose.Batch => FilePurpose.Batch,
            ClosedAI.Files.FilePurpose.BatchOutput => FilePurpose.BatchOutput,
            ClosedAI.Files.FilePurpose.Evaluations => FilePurpose.Evaluations,
            ClosedAI.Files.FilePurpose.FineTune => FilePurpose.FineTune,
            ClosedAI.Files.FilePurpose.FineTuneResults => FilePurpose.FineTuneResults,
            ClosedAI.Files.FilePurpose.UserData => FilePurpose.UserData,
            ClosedAI.Files.FilePurpose.Vision => FilePurpose.Vision,
            _ => throw new InvalidOperationException($"Invalid purpose '{value}'"),
        }));
    }

    public override ClosedAI.Files.FilePurpose Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var display = reader.GetString();
        if (display is null)
            throw new JsonException("Expected string value");

        return DisplayValue.TryParse<FilePurpose>(display, out var result) ? result switch
        {
            FilePurpose.Assistants => ClosedAI.Files.FilePurpose.Assistants,
            FilePurpose.AssistantsOutput => ClosedAI.Files.FilePurpose.AssistantsOutput,
            FilePurpose.Batch => ClosedAI.Files.FilePurpose.Batch,
            FilePurpose.BatchOutput => ClosedAI.Files.FilePurpose.BatchOutput,
            FilePurpose.Evaluations => ClosedAI.Files.FilePurpose.Evaluations,
            FilePurpose.FineTune => ClosedAI.Files.FilePurpose.FineTune,
            FilePurpose.FineTuneResults => ClosedAI.Files.FilePurpose.FineTuneResults,
            FilePurpose.UserData => ClosedAI.Files.FilePurpose.UserData,
            FilePurpose.Vision => ClosedAI.Files.FilePurpose.Vision,
            _ => throw new InvalidOperationException($"Invalid purpose '{display}'"),
        } : throw new InvalidOperationException($"Invalid purpose '{display}'");

    }
}
