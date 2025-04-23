using System.Diagnostics.CodeAnalysis;
using Devlooped.Sponsors;
using GitCredentialManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI;

public static class App
{
    static readonly CancellationTokenSource shutdownCancellation = new();

    static App() => Console.CancelKeyPress += (s, e) => shutdownCancellation.Cancel();

    /// <summary>
    /// Whether the CLI app is not interactive (i.e. part of a script run, 
    /// running in CI, or in a non-interactive user session).
    /// </summary>
    public static bool IsNonInteractive => !Environment.UserInteractive
        || Console.IsInputRedirected
        || Console.IsOutputRedirected;

    public static CommandApp Create() => Create(out _);

    public static CommandApp Create([NotNull] out IServiceProvider services)
        => Create(new ServiceCollection(), out services);

    public static CommandApp Create(IAnsiConsole console, [NotNull] out IServiceProvider services)
    {
        var app = Create(new ServiceCollection().AddSingleton(console), out services);
        app.Configure(config => config.ConfigureConsole(console));
        return app;
    }

    static CommandApp Create(IServiceCollection collection, [NotNull] out IServiceProvider services)
    {
        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
#if DEBUG
            .AddUserSecrets<TypeRegistrar>()
#endif
            .Build();

        collection.AddSingleton(config)
            .AddSingleton<IConfiguration>(_ => config)
            .AddSingleton(_ => CredentialManager.Create(ThisAssembly.Project.PackageId));

        collection.AddSingleton(shutdownCancellation);

        collection.AddSingleton(services =>
        {
            var configuration = services.GetRequiredService<IConfiguration>();
            var store = services.GetRequiredService<ICredentialStore>();

            var apikey = store.Get("https://api.openai.com", "_CURRENT_")?.Password
                ?? configuration["OPENAI_API_KEY"]
                ?? "";

            return new OpenAIClient(apikey);
        });

        collection.ConfigureSponsors();
        collection.AddServices();

        var registrar = new TypeRegistrar(collection);
        var app = new CommandApp(registrar);
        registrar.Services.AddSingleton<ICommandApp>(app);

        app.Configure(config =>
        {
            // Allows emitting help markdown on build
            if (Environment.GetEnvironmentVariables().Contains("NO_COLOR"))
                config.Settings.HelpProviderStyles = null;
        });

        app.UseAuth();
        app.UseFiles();
        app.UseVectors();
        app.UseModels();
        app.UseSponsors();

        services = registrar.Services.BuildServiceProvider();

        return app;
    }
}
