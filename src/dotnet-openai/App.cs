using GitCredentialManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using Spectre.Console.Cli;

namespace Devlooped.OpenAI;

public static class App
{
    static readonly CancellationTokenSource shutdownCancellation = new();

    static App() => Console.CancelKeyPress += (s, e) => shutdownCancellation.Cancel();

    public static CommandApp Create() => Create(out _);

    public static CommandApp Create(out IServiceProvider services)
    {
        var collection = new ServiceCollection();

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

        services = registrar.Services.BuildServiceProvider();

        return app;
    }
}
