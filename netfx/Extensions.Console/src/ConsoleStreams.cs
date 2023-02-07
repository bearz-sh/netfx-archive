namespace Bearz.Extensions.Console;

public class ConsoleStreams : IConsole
{
    public bool IsInputRedirected => System.Console.IsInputRedirected;

    public bool IsOutputRedirected => System.Console.IsOutputRedirected;

    public bool IsErrorRedirected => System.Console.IsErrorRedirected;

    public TextReader In => System.Console.In;

    public TextWriter Out => System.Console.Out;

    public TextWriter Error => System.Console.Error;
}