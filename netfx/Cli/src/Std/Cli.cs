using Bearz.Console;

namespace Bearz.Std;

public static partial class Cli
{
    public static ISystemConsole Console { get; set; } = new SystemConsole();

    public static IConsoleHost Host { get; set; } = new ConsoleHost();

    public static void Echo(string value)
    {
        Console.Out.WriteLine(value);
    }
}