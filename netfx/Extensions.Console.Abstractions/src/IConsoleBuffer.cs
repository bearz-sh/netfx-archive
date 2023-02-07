namespace Bearz.Extensions.Console;

public interface IConsoleBuffer
{
    int Width { get; set; }

    int Height { get; set; }

    void SetSize(int width, int height);

    void MoveArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop);

    void MoveArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor);

    void Deconstruct(out int width, out int height);

    void Clear();
}