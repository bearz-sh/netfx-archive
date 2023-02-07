namespace Bearz.Extensions.Console;

public interface ISystemConsole : IConsole, IConsoleColor
{
    IConsoleKeys Keys { get;  }

    IConsoleBuffer Buffer { get;  }

    IConsoleWindow Window { get; }

    IConsoleCursor Cursor { get; }

    void SetOut(TextWriter writer);

    void SetError(TextWriter writer);

    void SetIn(TextReader reader);

    void Beep();
}