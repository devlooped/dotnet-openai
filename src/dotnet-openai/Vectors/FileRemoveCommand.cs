﻿using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI.Vectors;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
[Description("Remove file from vector store")]
[Service]
public class FileRemoveCommand(OpenAIClient oai, IAnsiConsole console, CancellationTokenSource cts) : Command<FileCommandSettings>
{
    public override int Execute(CommandContext context, FileCommandSettings settings)
    {
        var response = oai.GetVectorStoreClient().RemoveFileFromStore(settings.Store, settings.FileId, cts.Token);

        return console.RenderJson(response.GetRawResponse(), settings, cts.Token);
    }
}