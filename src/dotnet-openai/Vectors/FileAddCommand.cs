using System.ClientModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;
using static Devlooped.OpenAI.Vectors.FileAddCommand;

namespace Devlooped.OpenAI.Vectors;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
[Description("Add file to vector store")]
class FileAddCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : AsyncCommand<FileAddCommandSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, FileAddCommandSettings settings)
    {
        var message = oai.Pipeline.CreateMessage();

        var attributes = new Dictionary<string, object>();
        foreach (var item in settings.Attributes)
        {
            var parts = item.Split(['='], 2);
            if (parts.Length == 2)
            {
                var key = parts[0].Trim();
                var value = parts[1].Trim();
                if (double.TryParse(value, out var number))
                    attributes[key] = number;
                else if (bool.TryParse(value, out var boolean))
                    attributes[key] = boolean;
                else
                    attributes[key] = value;
            }
        }

        message.Request.Method = "POST";
        message.Request.Uri = new Uri($"https://api.openai.com/v1/vector_stores/{settings.StoreId}/files");
        message.Request.Headers.Add("OpenAI-Beta", "assistants=v2");
        var request = JsonSerializer.Serialize(new { file_id = settings.FileId, attributes });
        message.Request.Content = BinaryContent.Create(BinaryData.FromString(request));

        await oai.Pipeline.SendAsync(message);
        Debug.Assert(message.Response != null);
        var vectors = oai.GetVectorStoreClient();

        var response = await vectors.GetFileAssociationAsync(settings.StoreId, settings.FileId, cts.Token);
        if (response.Value is null || response.GetRawResponse().IsError)
        {
            console.MarkupLine($":cross_mark: Failed to add file to vector store:");
            console.RenderJson(response.GetRawResponse(), "", settings.Monochrome, cts.Token);
            return -1;
        }

        while (response.Value.Status == global::OpenAI.VectorStores.VectorStoreFileAssociationStatus.InProgress)
        {
            await Task.Delay(200, cts.Token);
            response = await vectors.GetFileAssociationAsync(settings.StoreId, settings.FileId, cts.Token);
            if (response.Value is null)
            {
                console.MarkupLine($":cross_mark: Failed to add file to vector store");
                return -1;
            }
        }

        return console.RenderJson(response.GetRawResponse(), settings.JQ, settings.Monochrome, cts.Token);
    }

    public class FileAddCommandSettings : FileCommandSettings
    {
        [Description("Attributes to add to the vector file as KEY=VALUE")]
        [CommandOption("-a|--attribute")]
        public string[] Attributes { get; set; } = [];
    }
}