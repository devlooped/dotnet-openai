using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Devlooped.OpenAI;

public static class IAnsiConsoleExtensions
{
    public static IAnsiConsole Wrap(this IAnsiConsole console) => new UndisposableConsole(console);

    class UndisposableConsole(IAnsiConsole console) : IAnsiConsole
    {
        public Profile Profile => console.Profile;
        public IAnsiConsoleCursor Cursor => console.Cursor;
        public IAnsiConsoleInput Input => console.Input;
        public IExclusivityMode ExclusivityMode => console.ExclusivityMode;
        public RenderPipeline Pipeline => console.Pipeline;
        public void Clear(bool home) => console.Clear(home);
        public void Write(IRenderable renderable) => console.Write(renderable);
    }
}
