using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI;

public static class ICommandExtensions
{
    static readonly CommandContext EmptyContext = new(Array.Empty<string>(), RemainingArguments.Empty, string.Empty, null);

    public static Task<int> ExecuteAsync<TSettings>(this ICommand<TSettings> command, TSettings settings)
        where TSettings : CommandSettings
    {
        if (settings.Validate() is { Successful: false } validation)
            throw new Exception(validation.Message);

        return command.Execute(EmptyContext, settings);
    }

    public static Task<int> ExecuteAsync<TSettings>(this ICommand<TSettings> command, Action<TSettings>? configure = default)
        where TSettings : CommandSettings, new()
    {
        var settings = new TSettings();
        configure?.Invoke(settings);
        if (settings.Validate() is { Successful: false } validation)
            throw new Exception(validation.Message);

        return command.Execute(EmptyContext, settings);
    }

    class RemainingArguments : IRemainingArguments
    {
        public static IRemainingArguments Empty { get; } = new RemainingArguments();
        RemainingArguments() { }
        public ILookup<string, string?> Parsed => Enumerable.Empty<string>().ToLookup(x => x, x => default(string));
        public IReadOnlyList<string> Raw => [];
    }
}