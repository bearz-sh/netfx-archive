namespace Bearz.Console;

public interface IConsoleHost
{
    void Write(string value);

    void Write(string value, params object?[]? args);

    void WriteLine(string value);

    void WriteLine(string value, params object?[]? args);

    void WriteLine();

    void Group(string title);

    void GroupEnd();

    void Info(string? message, Exception? exception, params object?[]? args);

    void Trace(string? message, Exception? exception, params object?[]? args);

    void Debug(string? message, Exception? exception, params object?[]? args);

    void Warn(string? message, Exception? exception, params object?[]? args);

    void Error(string? message, Exception? exception, params object?[]? args);
}