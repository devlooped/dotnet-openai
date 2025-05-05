using Spectre.Console;
using Spectre.Console.Rendering;

namespace Devlooped.OpenAI;

public class PrefixProgressColumn(ProgressColumn inner, string prefix) : ProgressColumn
{
    public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime)
    {
        var progress = inner.Render(options, task, deltaTime);
        return new Columns([new Markup(prefix), progress]);
    }
}