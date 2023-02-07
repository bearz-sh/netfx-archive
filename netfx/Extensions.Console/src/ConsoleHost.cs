namespace Bearz.Extensions.Console;

public class ConsoleHost : IConsoleHost
{
    private readonly Stack<string> groups = new Stack<string>();

    public void Write(string value)
        => System.Console.Write(value);

    public void Write(string value, params object?[]? args)
        => System.Console.Write(value, args);

    public void WriteLine(string value)
        => System.Console.WriteLine(value);

    public void WriteLine(string value, params object?[]? args)
        => System.Console.WriteLine(value, args);

    public void WriteLine()
        => System.Console.WriteLine();

    public void Group(string title)
    {
        this.groups.Push(title);
        System.Console.WriteLine($"################## {title,-40} ##################");
    }

    public void GroupEnd()
    {
        if (this.groups.Count > 0)
        {
            var title = this.groups.Pop();
            System.Console.WriteLine($"################## end {title,-36} ##################");
        }
    }

    public void Info(string? message, Exception? exception, params object?[]? args)
    {
        var f = System.Console.ForegroundColor;
        System.Console.ForegroundColor = ConsoleColor.DarkCyan;
        try
        {
            System.Console.Write("[INFO ]: ");
            if (message != null)
            {
                System.Console.Write(message, args);
            }

            if (exception != null)
            {
                System.Console.Write(" " + exception.ToString());
            }
        }
        finally
        {
            System.Console.WriteLine();
            System.Console.ForegroundColor = f;
        }
    }

    public void Trace(string? message, Exception? exception, params object?[]? args)
    {
        var f = System.Console.ForegroundColor;
        System.Console.ForegroundColor = ConsoleColor.DarkGray;
        try
        {
            System.Console.Write("[TRACE]: ");
            if (message != null)
            {
                System.Console.Write(message, args);
            }

            if (exception != null)
            {
                System.Console.Write(" " + exception.ToString());
            }
        }
        finally
        {
            System.Console.WriteLine();
            System.Console.ForegroundColor = f;
        }
    }

    public void Debug(string? message, Exception? exception, params object?[]? args)
    {
        var f = System.Console.ForegroundColor;
        System.Console.ForegroundColor = ConsoleColor.Gray;
        try
        {
            System.Console.Write("[DEBUG]: ");
            if (message != null)
            {
                System.Console.Write(message, args);
            }

            if (exception != null)
            {
                System.Console.Write(" " + exception.ToString());
            }
        }
        finally
        {
            System.Console.WriteLine();
            System.Console.ForegroundColor = f;
        }
    }

    public void Warn(string? message, Exception? exception, params object?[]? args)
    {
        var f = System.Console.ForegroundColor;
        System.Console.ForegroundColor = ConsoleColor.DarkYellow;
        try
        {
            System.Console.Write("[WARN ]: ");
            if (message != null)
            {
                System.Console.Write(message, args);
            }

            if (exception != null)
            {
                System.Console.Write(" " + exception.ToString());
            }
        }
        finally
        {
            System.Console.WriteLine();
            System.Console.ForegroundColor = f;
        }
    }

    public void Error(string? message, Exception? exception, params object?[]? args)
    {
        var f = System.Console.ForegroundColor;
        System.Console.ForegroundColor = ConsoleColor.DarkRed;
        try
        {
            System.Console.Error.Write("[ERROR]: ");
            if (message != null)
            {
                if (args is { Length: > 0 })
                {
                    System.Console.Error.Write(message, args);
                }
                else
                {
                    System.Console.Error.Write(message);
                }
            }

            if (exception != null)
            {
                System.Console.Error.Write(" " + exception.ToString());
            }
        }
        finally
        {
            System.Console.Error.WriteLine();
            System.Console.ForegroundColor = f;
        }
    }
}