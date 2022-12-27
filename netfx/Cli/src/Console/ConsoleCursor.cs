namespace Bearz.Console;

public class ConsoleCursor : IConsoleCursor
{
    public int Left
    {
        get => System.Console.CursorLeft;
        set => System.Console.CursorLeft = value;
    }

    public int Top
    {
        get => System.Console.CursorTop;
        set => System.Console.CursorTop = value;
    }

    public int Size
    {
        get => System.Console.CursorSize;
        set => System.Console.CursorSize = value;
    }

    public bool Visible
    {
        get => System.Console.CursorVisible;
        set => System.Console.CursorVisible = value;
    }

    public void SetPosition(int left, int top)
        => System.Console.SetCursorPosition(left, top);

    public void Deconstruct(out int left, out int top, out int size)
    {
        left = System.Console.CursorLeft;
        top = System.Console.CursorTop;
        size = System.Console.CursorSize;
    }

    public void Deconstruct(out int left, out int top, out int size, out bool visible)
    {
        left = System.Console.CursorLeft;
        top = System.Console.CursorTop;
        size = System.Console.CursorSize;
        visible = System.Console.CursorVisible;
    }
}