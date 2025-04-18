﻿using System.ComponentModel;
using OpenAI;
using OpenAI.VectorStores;
using Spectre.Console;
using Spectre.Console.Cli;
using static Devlooped.OpenAI.Vectors.ModifyCommand;

namespace Devlooped.OpenAI.Vectors;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
[Description("Modify a vector store")]
class ModifyCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : AsyncCommand<ModifySettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, ModifySettings settings)
    {
        var options = new VectorStoreModificationOptions();
        if (settings.Name.IsSet)
            options.Name = settings.Name.Value;

        foreach (var item in settings.Meta)
        {
            options.Metadata[item.Key] = item.Value;
        }

        if (settings.ExpiresAfter.IsSet)
        {
            options.ExpirationPolicy = new VectorStoreExpirationPolicy(VectorStoreExpirationAnchor.LastActiveAt, settings.ExpiresAfter.Value);
        }

        var result = await oai.GetVectorStoreClient().ModifyVectorStoreAsync(settings.Id, options, cts.Token);
        if (result.Value is null)
        {
            console.MarkupLine($":cross_mark: Failed to modify vector store");
            return -1;
        }

        // TODO: add non-JSON output?
        return console.RenderJson(result.GetRawResponse(), settings, cts.Token);
    }

    public class ModifySettings : JsonCommandSettings
    {
        [Description("The ID of the vector store")]
        [CommandArgument(0, "<ID>")]
        public required string Id { get; init; }

        [Description("The name of the vector store")]
        [CommandOption("-n|--name [NAME]")]
        public FlagValue<string> Name { get; set; } = new();

        [Description("Sets the expiration policy to number of days after last active.")]
        [CommandOption("-e|--expires [DAYS]")]
        public FlagValue<int> ExpiresAfter { get; set; } = new();

        [Description("Metadata to add to the vector store as KEY=VALUE")]
        [CommandOption("-m|--meta")]
        public IDictionary<string, string> Meta { get; set; } = new Dictionary<string, string>();
    }
}
