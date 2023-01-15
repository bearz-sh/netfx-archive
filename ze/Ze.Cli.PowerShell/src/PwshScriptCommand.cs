using System.Runtime.InteropServices;

using Bearz.Std;

namespace Ze.Cli.PowerShell;

public class PwshScriptCommand : CliScriptCommand
{
    private string? fileName;

    public PwshScriptCommand()
    {
        this.Args.Add("-NoLogo", "-ExecutionPolicy", "ByPass", "-NonInteractive", "-NoProfile");
        this.Args.Add("-Command");
    }

    public PwshScriptCommand(string script)
        : this()
    {
        this.Script = script;
    }

    public PwshScriptCommand(string script, CliStartInfo startInfo)
    {
        this.Script = script;
        this.CliStartInfo = startInfo;
        this.Args.Add("-NoLogo", "-ExecutionPolicy", "ByPass", "-NonInteractive", "-NoProfile");
        this.Args.Add("-Command");
    }

    protected override string Extension => ".ps1";

    protected override string FileName
    {
        get
        {
            if (this.fileName is not null)
                return this.fileName;

            var tmpDir = System.IO.Path.GetTempPath();
            var tmpFile = System.IO.Path.Combine(tmpDir, $"{System.IO.Path.GetRandomFileName()}.{this.Extension}");
            if (!File.Exists(tmpFile))
            {
                var script = this.Script;
                script = $@"
$ErrorActionPreference = 'Stop';
{script}
if((Test-Path -LiteralPath variable:LASTEXITCODE))
{{
    exit $LASTEXITCODE;
}}
";

                File.WriteAllText(tmpFile, script);
            }

            this.fileName = tmpFile;
            return this.fileName;
        }
    }

    public override CommandStartInfo Build()
    {
        this.Args.Add($". {this.FileName}");
        return new CommandStartInfo()
        {
            Args = this.Args,
            Cwd = this.CliStartInfo.Cwd,
            Env = this.CliStartInfo.Env,
            StdIn = this.CliStartInfo.StdIn,
            StdOut = this.CliStartInfo.StdOut,
            StdErr = this.CliStartInfo.StdErr,
        };
    }
}