using Bearz.Std;

using Path = System.IO.Path;

namespace Bearz.Cli.Execution;

public abstract class CliScriptCommand : ICliCommand
{
    private string? fileName;

    protected CliScriptCommand(string script, CliStartInfo? cliStartInfo = null)
    {
        this.Script = script;
        this.CliStartInfo = cliStartInfo ?? new CliStartInfo();
    }

    public string Script { get; set; }

    public string? CommandName => null;

    public CliStartInfo CliStartInfo { get; protected set; }

    public virtual string TempFileName
    {
        get
        {
            if (this.fileName is null)
            {
                var tmpDir = Path.GetTempPath();
                var tmpFile = Path.Combine(tmpDir, $"{Path.GetRandomFileName()}.{this.Extension}");
                if (!File.Exists(tmpFile))
                {
                    File.WriteAllText(tmpFile, this.Script);
                }

                this.fileName = tmpFile;
            }

            return this.fileName;
        }
    }

    protected abstract string Extension { get; }

    protected CommandArgs Args => this.CliStartInfo.Args;

    public virtual CommandStartInfo Build()
    {
        var args = this.CliStartInfo.Args;
        args.Add(this.TempFileName);
        return new CommandStartInfo()
        {
            Args = this.CliStartInfo.Args,
            Cwd = this.CliStartInfo.Cwd,
            Env = this.CliStartInfo.Env,
            StdIn = this.CliStartInfo.StdIn,
            StdOut = this.CliStartInfo.StdOut,
            StdErr = this.CliStartInfo.StdErr,
        };
    }
}