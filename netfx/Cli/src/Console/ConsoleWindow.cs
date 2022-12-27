namespace Bearz.Console;

public class ConsoleWindow : IConsoleWindow
{
    public string Title
    {
        get => System.Console.Title;
        set => System.Console.Title = value;
    }

    public int Height
    {
        get => System.Console.WindowHeight;
        set => System.Console.WindowHeight = value;
    }

    public int Width
    {
        get => System.Console.WindowWidth;
        set => System.Console.WindowWidth = value;
    }

    public int Top
    {
        get => System.Console.WindowTop;
        set => System.Console.WindowTop = value;
    }

    public int Left
    {
        get => System.Console.WindowLeft;
        set => System.Console.WindowLeft = value;
    }

    public void SetPosition(int left, int top)
        => System.Console.SetWindowPosition(left, top);

    public void SetSize(int width, int height)
        => System.Console.SetWindowSize(width, height);

    public void Deconstruct(out int left, out int top, out int width, out int height)
    {
        left = System.Console.WindowHeight;
        top = System.Console.WindowTop;
        width = System.Console.WindowWidth;
        height = System.Console.WindowHeight;
    }
}