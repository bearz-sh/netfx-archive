namespace Bearz.Extensions.Console;

public interface IConsoleCursor
{
    int Left { get; set; }

    int Top { get; set; }

    int Size { get; set; }

    bool Visible { get; set; }

    void SetPosition(int left, int top);

    void Deconstruct(out int left, out int top, out int size);

    void Deconstruct(out int left, out int top, out int size, out bool visible);
}