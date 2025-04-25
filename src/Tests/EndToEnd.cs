using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Devlooped.OpenAI.Vectors;
using DotNetConfig;
using GitCredentialManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using Spectre.Console;
using Spectre.Console.Advanced;
using Spectre.Console.Cli;
using Spectre.Console.Rendering;
using Spectre.Console.Testing;
using static System.Collections.Specialized.BitVector32;

namespace Devlooped.OpenAI;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
public class EndToEnd : IDisposable
{
    readonly ITestOutputHelper output;
    string? fileId = default;
    string? storeId = default;
    IServiceProvider services;
    TestConsole console = new();

    public EndToEnd(ITestOutputHelper output)
    {
        this.output = output;
        App.Create(console.Wrap(), out var services);
        this.services = (IServiceProvider)services;

        // Clear all vector stores before starting
        var oai = this.services.GetRequiredService<OpenAIClient>();
        var vectors = oai.GetVectorStoreClient();

        Task.WaitAll(vectors
            .GetVectorStores()
            .Select(store => vectors.DeleteVectorStoreAsync(store.Id))
            .ToArray());

        Config.Build(ConfigLevel.Global).RemoveSection("openai", "vectors");
    }

    public void Dispose()
    {
        var oai = this.services.GetRequiredService<OpenAIClient>();
        var vectors = oai.GetVectorStoreClient();

        Task.WaitAll(vectors
            .GetVectorStores()
            .Select(store => vectors.DeleteVectorStoreAsync(store.Id))
            .ToArray());

        var files = oai.GetOpenAIFileClient();
        Task.WaitAll(files.GetFiles().Value
            .Select(file => files.DeleteFileAsync(file.Id))
            .ToArray());

        output.WriteLine(console.Output);
        Config.Build(ConfigLevel.Global).RemoveSection("openai", "vectors");
    }

    [LocalFact]
    public async Task FullScenario()
    {
        Assert.Equal(0, await services.GetRequiredService<Auth.StatusCommand>().ExecuteAsync());

        var upload = services.GetRequiredService<File.UploadCommand>();
        Assert.Equal(0, await upload.ExecuteAsync(new File.UploadCommand.UploadSettings()
        {
            File = Path.GetFullPath("./Content.txt"),
            Purpose = "assistants"
        }));

        fileId = upload.FileId;
        Assert.NotNull(fileId);

        var create = services.GetRequiredService<Vectors.CreateCommand>();
        Assert.Equal(0, await create.ExecuteAsync(new Vectors.CreateCommand.CreateSettings()
        {
            Name =
            {
                IsSet = true,
                Value = "foo",
            },
            Files = new[] { fileId },
        }));

        storeId = create.StoreId;
        Assert.NotNull(storeId);

        // Ensure mapping is persisted to config
        var mapper = services.GetRequiredService<VectorIdMapper>();
        Assert.True(mapper.TryGetId("foo", out var id));
        Assert.Equal(storeId, id);

        Assert.Equal(0, await services.GetRequiredService<Vectors.ViewCommand>().ExecuteAsync(new StoreCommandSettings(mapper)
        {
            Store = "foo",
        }));

        // Rename store
        Assert.Equal(0, await services.GetRequiredService<Vectors.ModifyCommand>().ExecuteAsync(new Vectors.ModifyCommand.ModifySettings(mapper)
        {
            Store = "foo",
            Name =
            {
                IsSet = true,
                Value = "bar",
            },
        }));

        Assert.Equal(0, await services.GetRequiredService<Vectors.ViewCommand>().ExecuteAsync(new StoreCommandSettings(mapper)
        {
            Store = "bar",
        }));

        Assert.False(mapper.TryGetId("foo", out id));

        var search = services.GetRequiredService<Vectors.SearchCommand>();

        // Perform a search
        Assert.Equal(0, await search.ExecuteAsync(new SearchCommand.SearchSettings(mapper)
        {
            Store = "bar",
            Query = "where does kzu live?",
        }));

        Assert.Contains("Argentina", console.Output);
        Assert.DoesNotContain("Analia", console.Output);

        Assert.Equal(0, await upload.ExecuteAsync(new File.UploadCommand.UploadSettings()
        {
            File = Path.GetFullPath("./Content2.txt"),
            Purpose = "assistants",
        }));

        Assert.NotNull(upload.FileId);

        Assert.Equal(0, await services.GetRequiredService<Vectors.FileAddCommand>().ExecuteAsync(new Vectors.FileAddCommand.FileAddCommandSettings(mapper)
        {
            Store = "bar",
            FileId = upload.FileId,
            Attributes = ["truthness=100"]
        }));

        Assert.Equal(0, await upload.ExecuteAsync(new File.UploadCommand.UploadSettings()
        {
            File = Path.GetFullPath("./Content3.txt"),
            Purpose = "assistants",
        }));

        Assert.Equal(0, await services.GetRequiredService<Vectors.FileAddCommand>().ExecuteAsync(new Vectors.FileAddCommand.FileAddCommandSettings(mapper)
        {
            Store = "bar",
            FileId = upload.FileId,
            Attributes = ["truthness=20"]
        }));

        // Perform a search
        Assert.Equal(0, await services.GetRequiredService<Vectors.SearchCommand>().ExecuteAsync(new SearchCommand.SearchSettings(mapper)
        {
            Store = "bar",
            Query = "who's kzu's wife?",
            Filters = ["truthness>=80"]
        }));

        Assert.Contains("Analia", console.Output);
        Assert.DoesNotContain("Grok", console.Output);

        Assert.Equal(0, await services.GetRequiredService<Vectors.SearchCommand>().ExecuteAsync(new SearchCommand.SearchSettings(mapper)
        {
            Store = "bar",
            Query = "who's kzu's wife?",
            Filters = ["truthness<50"]
        }));

        Assert.Contains("Grok", console.Output);
    }
}
