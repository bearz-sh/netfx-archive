using Bearz.Std;

using Path = System.IO.Path;

namespace Ze.Cli;

public abstract class CliScriptCommand : ICliCommand
{
    private string? fileName;

    public string Script { get; set; } = string.Empty;

    public string? CommandName => null;

    public CliStartInfo CliStartInfo { get; protected set; } = new CliStartInfo();

    protected internal virtual string FileName
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
        args.Add(this.FileName);
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