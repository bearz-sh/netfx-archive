using Bearz.Cli.Execution;
using Bearz.Extra.Strings;
using Bearz.Std;

namespace Ze.Cli.DotNet;

public class DotNetNewUninstallCmd : DotNetCmd
{
    public DotNetNewUninstallCmd(CliStartInfo? startInfo = null)
        : base("new uninstall", startInfo)
    {
    }

    public string Package { get; set; } = string.Empty;

    public string? Verbosity { get; set; }

    public bool Diagnostics { get; set; }


    public override CommandStartInfo Build()
    {
        var args = this.Args;
        var si = this.CliStartInfo;

        args.Add(this.CommandName);

        if (this.Help)
        {
            args.Add("--help");
            return new CommandStartInfo()
            {
                Args = args,
                Cwd = si.Cwd,
                Env = si.Env,
                StdIn = si.StdIn,
                StdOut = si.StdOut,
                StdErr = si.StdErr,
            };
        }

        if (this.Package.IsNullOrWhiteSpace())
            throw new InvalidOperationException("Package is required");

        if (this.Diagnostics)
            args.Add("--diagnostics");

        if (!this.Verbosity.IsNullOrWhiteSpace())
            args.Add("--verbosity", this.Verbosity);

        return new CommandStartInfo()
        {
            Args = args,
            Cwd = si.Cwd,
            Env = si.Env,
            StdIn = si.StdIn,
            StdOut = si.StdOut,
            StdErr = si.StdErr,
        };
    }
}