using System.Management.Automation;

using Spectre.Console;

namespace Ze.PowerShell.SpectreConsole;

public static class AnsiWriter
{
    static AnsiWriter()
    {
        Console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Ansi = AnsiSupport.Detect,
            ColorSystem = ColorSystemSupport.Detect,
            Out = new AnsiConsoleOutput(ConsoleWriter),
        });
    }

    public static IAnsiConsole Console { get; set; }

    public static PsHostWriter ConsoleWriter => new PsHostWriter(NullPsHost.Instace);
}