using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Devlooped.OpenAI;
using Spectre.Console.Cli;

// Some users reported not getting emoji on Windows, so we force UTF-8 encoding.
// This not great, but I couldn't find a better way to do it.
if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    Console.InputEncoding = Console.OutputEncoding = Encoding.UTF8;

// Alias -? to -h for help
if (args.Contains("-?"))
    args = args.Select(x => x == "-?" ? "-h" : x).ToArray();

#if DEBUG
if (args.Contains("--debug"))
{
    Debugger.Launch();
    args = [.. args.Where(x => x != "--debug")];
}
#endif

var app = App.Create();

#if DEBUG
app.Configure(c => c.PropagateExceptions());
#else
if (args.Contains("--exceptions"))
{
    app.Configure(c => c.PropagateExceptions());
    args = [.. args.Where(x => x != "--exceptions")];
}
#endif

app.Configure(config => config.SetApplicationName(ThisAssembly.Project.ToolCommandName));

if (args.Contains("--version"))
{
    app.ShowVersion();
#if DEBUG
    await app.ShowUpdatesAsync(args);
#endif
    return 0;
}

#if DEBUG
return await app.RunAsync(args);
#else
return await app.RunWithUpdatesAsync(args);
#endif