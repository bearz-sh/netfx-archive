namespace Bearz.Extensions.Console;

public class ConsoleBuffer : IConsoleBuffer
{
    public int Width
    {
        get => System.Console.BufferWidth;
        set => System.Console.BufferWidth = value;
    }

    public int Height
    {
        get => System.Console.BufferHeight;
        set => System.Console.BufferHeight = value;
    }

    public void SetSize(int width, int height)
        => System.Console.SetBufferSize(width, height);

    public void MoveArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop)
        => System.Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop);

    public void MoveArea(
        int sourceLeft,
        int sourceTop,
        int sourceWidth,
        int sourceHeight,
        int targetLeft,
        int targetTop,
        char sourceChar,
        ConsoleColor sourceForeColor,
        ConsoleColor sourceBackColor)
        => System.Console.MoveBufferArea(
            sourceLeft,
            sourceTop,
            sourceWidth,
            sourceHeight,
            targetLeft,
            targetTop,
            sourceChar,
            sourceForeColor,
            sourceBackColor);

    public void Deconstruct(out int width, out int height)
    {
        width = System.Console.BufferWidth;
        height = System.Console.BufferHeight;
    }

    public void Clear()
        => System.Console.Clear();
}