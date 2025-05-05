using System.Diagnostics.CodeAnalysis;
using DotNetConfig;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;

namespace Devlooped.OpenAI.Vectors;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
[Service]
public class VectorIdMapper(OpenAIClient oai)
{
    ConfigSection section = Config.Build(ConfigLevel.Global).GetSection(ThisAssembly.Project.ToolCommandName, "vectors");

    public void RemoveId(string id)
    {
        foreach (var entry in Enumerate().Where(x => x.Value == id))
            section = section.Unset(entry.Key, ConfigLevel.Global);
    }

    public void SetId(string name, string id)
    {
        var existing = Enumerate().Where(x => x.Value == id).Select(x => x.Key).FirstOrDefault();
        if (existing != null && existing != name)
            section = section.Unset(existing, ConfigLevel.Global);

        // Never try to set an empty id. It should equal to "unset"
        if (string.IsNullOrEmpty(name))
            return;

        if (!section.TryGetString(name, out var saved) || saved != id)
            section = section.SetString(name, id, ConfigLevel.Global);
    }

    public bool TryGetId(string name, [NotNullWhen(true)] out string? id)
    {
        if (section.TryGetString(name, out id))
            return true;

        var stores = oai.GetVectorStoreClient().GetVectorStores();
        foreach (var store in stores.Where(x => x.Name is not null))
        {
            section = section.SetString(store.Name, store.Id, ConfigLevel.Global);
        }

        if (section.TryGetString(name, out id))
            return true;

        return false;
    }

    public IEnumerable<KeyValuePair<string, string>> Enumerate()
    {
        foreach (var entry in Config.Build(ConfigLevel.Global).AsEnumerable()
            .Where(x => x.Section == ThisAssembly.Project.ToolCommandName && x.Subsection == "vectors"))
        {
            yield return KeyValuePair.Create(entry.Variable, entry.GetString());
        }
    }
}

public static class VectorIdMapperExtensions
{
    /// <summary>
    /// If the <paramref name="name"/> does not start with <c>vs_</c>, 
    /// attempts to map it to a vector store ID using the <paramref name="mapper"/>, 
    /// otherwise, returns the name as-is.
    /// </summary>
    public static string MapName(this VectorIdMapper mapper, string name)
    {
        if (name.StartsWith("vs_"))
            return name;

        if (mapper.TryGetId(name, out var id))
            return id;

        return name;
    }
}