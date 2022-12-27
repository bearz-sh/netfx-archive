namespace Bearz.Console;

public interface IConsoleWindow
{
    string Title { get; set; }

    int Height { get; set; }

    int Width { get; set; }

    int Top { get; set; }

    int Left { get; set; }

    void SetPosition(int left, int top);

    void SetSize(int width, int height);

    void Deconstruct(out int left, out int top, out int width, out int height);
}