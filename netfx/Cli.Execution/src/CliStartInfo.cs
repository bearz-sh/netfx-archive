using Bearz.Extra.Collections;
using Bearz.Std;

namespace Bearz.Cli.Execution;

public class CliStartInfo
{
    public bool UseSudo { get; set; }

    public Stdio StdOut { get; set; }

    public Stdio StdErr { get; set; }

    public Stdio StdIn { get; set; }

    public string? Cwd { get; set; }

    public IDictionary<string, string>? Env { get; set; }

    protected internal CommandArgs Args { get; set; } = new CommandArgs();

    internal CommandStartInfo ToCommandStartInfo()
    {
        return new CommandStartInfo
        {
            Args = this.Args,
            StdOut = this.StdOut,
            StdErr = this.StdErr,
            StdIn = this.StdIn,
            Cwd = this.Cwd,
            Env = this.Env,
        };
    }
}