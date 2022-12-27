namespace Bearz.Console;

public interface IConsoleKeys
{
    bool KeyAvailable { get; }

    bool CapsLock { get; }

    bool NumLock { get; }

    ConsoleKeyInfo ReadKey();

    void Deconstruct(out bool capsLock, out bool numLock);
}