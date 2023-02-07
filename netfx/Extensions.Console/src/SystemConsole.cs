namespace Bearz.Extensions.Console;

public class SystemConsole : ConsoleStreams, ISystemConsole
{
    public ConsoleColor ForegroundColor
    {
        get => System.Console.ForegroundColor;
        set => System.Console.ForegroundColor = value;
    }

    public ConsoleColor BackgroundColor
    {
        get => System.Console.BackgroundColor;
        set => System.Console.BackgroundColor = value;
    }

    public IConsoleKeys Keys { get; } = new ConsoleKeys();

    public IConsoleBuffer Buffer { get; } = new ConsoleBuffer();

    public IConsoleWindow Window { get; } = new ConsoleWindow();

    public IConsoleCursor Cursor { get; } = new ConsoleCursor();

    public void ResetColor()
        => System.Console.ResetColor();

    public void SetOut(TextWriter writer)
        => System.Console.SetOut(writer);

    public void SetError(TextWriter writer)
        => System.Console.SetError(writer);

    public void SetIn(TextReader reader)
        => System.Console.SetIn(reader);

    public void Beep()
        => System.Console.Beep();
}