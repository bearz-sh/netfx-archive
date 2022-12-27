namespace Bearz.Console;

public class ConsoleKeys : IConsoleKeys
{
    public bool KeyAvailable => System.Console.KeyAvailable;

    public bool CapsLock => System.Console.CapsLock;

    public bool NumLock => System.Console.NumberLock;

    public ConsoleKeyInfo ReadKey()
        => System.Console.ReadKey();

    public void Deconstruct(out bool capsLock, out bool numLock)
    {
        capsLock = System.Console.CapsLock;
        numLock = System.Console.NumberLock;
    }
}