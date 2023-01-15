using Bearz.Extra.Strings;
using Bearz.Std;

namespace Ze.Cli.Bash;

public class BashCommand : CliCommand
{
    public BashCommand()
        : base(null)
    {
    }

    public bool Restricted { get; set; }

    public string? Command { get; set; }

    public bool Interactive { get; set; }

    public bool Login { get; set; }

    public bool StandardInput { get; set; }

    public string? RcFile { get; set; }

    public bool NoRc { get; set; }

    public bool NoProfile { get; set; }

    public bool Version { get; set; }

    public bool Verbose { get; set; }

    public bool NoEditing { get; set; }

    public bool DumpStrings { get; set; }

    public override CommandStartInfo Build()
    {
        var args = new CommandArgs();

        if (this.NoRc)
            args.Add("--norc");

        if (this.NoProfile)
            args.Add("--noprofile");

        if (this.Version)
            args.Add("--version");

        if (this.Verbose)
            args.Add("--verbose");

        if (this.DumpStrings)
            args.Add("--dump-strings");

        if (this.Restricted)
            args.Add("--restricted");

        if (!this.NoRc && !this.RcFile.IsNullOrWhiteSpace())
            args.Add("--rcfile", this.RcFile);

        if (this.Interactive)
            args.Add("-i");

        if (this.Login)
            args.Add("-l");

        if (this.StandardInput)
            args.Add("-s");

        if (!this.Command.IsNullOrWhiteSpace())
            args.Add("-c", this.Command);

        return new CommandStartInfo()
        {
            Args = args,
            Cwd = this.CliStartInfo.Cwd,
            Env = this.CliStartInfo.Env,
            StdOut = this.CliStartInfo.StdOut,
            StdErr = this.CliStartInfo.StdErr,
            StdIn = this.CliStartInfo.StdIn,
        };
    }
}