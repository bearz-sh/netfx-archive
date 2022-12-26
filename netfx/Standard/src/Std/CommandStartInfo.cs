namespace Bearz.Std;

public class CommandStartInfo
{
    public CommandArgs Args { get; set; } = new CommandArgs();

    public string? Cwd { get; set; }

    public Dictionary<string, string?>? Env { get; set; }

    public Stdio StdOut { get; set; }

    public Stdio StdErr { get; set; }

    public Stdio StdIn { get; set; }
}