using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Vectors;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
[Description("Add or modify file attributes in vector store")]
[Service]
public class FileModifyCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : FileAddCommand(oai, console, cts)
{
    public override async Task<int> ExecuteAsync(CommandContext context, FileAddSettings settings)
    {
        // We can't really modify the existing file association, so we remove and re-add it, 
        // preserving any existing attributes.
        var vectors = oai.GetVectorStoreClient();
        var response = await vectors.GetFileAssociationAsync(settings.Store, settings.FileId, cts.Token);
        var attributes = new Dictionary<string, object>();

        foreach (var existing in response.Value.Attributes)
        {
            var value = existing.Value.ToString().Trim('\"');
            if (double.TryParse(value, out var number))
                attributes[existing.Key] = number;
            else if (bool.TryParse(value, out var boolean))
                attributes[existing.Key] = boolean;
            else
                attributes[existing.Key] = value;
        }

        // New values override existing ones
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

        await vectors.RemoveFileFromStoreAsync(settings.Store, settings.FileId, cts.Token);
        return await AddAsync(settings, attributes);
    }
}
