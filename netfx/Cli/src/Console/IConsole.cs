namespace Bearz.Console;

public interface IConsole
{
    bool IsInputRedirected { get; }

    bool IsOutputRedirected { get; }

    bool IsErrorRedirected { get; }

    TextReader In { get; }

    TextWriter Out { get; }

    TextWriter Error { get; }
}