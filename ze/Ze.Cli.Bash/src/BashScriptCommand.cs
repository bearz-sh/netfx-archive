using System.Runtime.InteropServices;

using Bearz.Cli.Execution;
using Bearz.Extra.Strings;
using Bearz.Std;

using FluentBuilder;

namespace Ze.Cli.Bash;

[AutoGenerateBuilder]
public class BashScriptCommand : CliScriptCommand
{
    private bool? isWslBash;
    private string? fileName;

    public BashScriptCommand()
    {
        this.Args.Add("--noprofile", "--norc", "-e", "-o", "pipefail");
    }

    public BashScriptCommand(string script)
        : this()
    {
        this.Script = script;
    }

    public BashScriptCommand(string script, CliStartInfo startInfo)
    {
        this.Script = script;
        this.CliStartInfo = startInfo;
        this.Args.Add("--noprofile", "--norc", "-e", "-o", "pipefail");
    }

    public override string TempFileName
    {
        get
        {
            if (this.fileName is not null)
                return this.fileName;

            var tmpDir = System.IO.Path.GetTempPath();
            var tmpFile = System.IO.Path.Combine(tmpDir, $"{System.IO.Path.GetRandomFileName()}.{this.Extension}");
            if (!File.Exists(tmpFile))
            {
                File.WriteAllText(tmpFile, this.Script);
            }

            this.fileName = tmpFile;
            if (this.IsWslBash)
            {
                // WSL Bash needs to be called with the full path to the script file using the mnt path and linux file path.
                this.fileName = $"/mnt/c{this.fileName.Substring(1).Replace(":", string.Empty).Replace("\\", "/")}";
            }

            return this.fileName;
        }
    }

    protected bool IsWslBash
    {
        get
        {
            if (this.isWslBash == null)
            {
                this.isWslBash = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) &&
                                 Process.Which("bash")?.EqualsIgnoreCase("c:\\windows\\system32\\bash.exe") == true;
            }

            return this.isWslBash.Value;
        }
    }

    protected override string Extension => ".sh";

    public static implicit operator BashScriptCommand(string script)
    {
        return new BashScriptCommand(script);
    }
}