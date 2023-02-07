namespace Bearz.Extensions.Console;

public interface IConsoleColor
{
    ConsoleColor ForegroundColor { get; set; }

    ConsoleColor BackgroundColor { get; set; }

    void ResetColor();
}