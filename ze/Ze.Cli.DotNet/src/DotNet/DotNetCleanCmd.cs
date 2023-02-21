using Bearz.Cli.Execution;
using Bearz.Extra.Strings;
using Bearz.Std;

namespace Ze.Cli.DotNet;

public class DotNetCleanCmd : DotNetTargetCmd
{
    public DotNetCleanCmd(CliStartInfo? startInfo = null)
        : base("clean", startInfo)
    {
    }

    public override CommandStartInfo Build()
    {
        var args = this.Args;
        var si = this.CliStartInfo;

        args.Add("clean");

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

        if (!this.Target.IsNullOrWhiteSpace())
            args.Add(this.Target);

        if (!this.Configuration.IsNullOrWhiteSpace())
        {
            args.Add("--configuration");
            args.Add(this.Configuration);
        }

        if (!this.Framework.IsNullOrWhiteSpace())
        {
            args.Add("--framework");
            args.Add(this.Framework);
        }

        if (!this.Runtime.IsNullOrWhiteSpace())
        {
            args.Add("--runtime");
            args.Add(this.Runtime);
        }

        if (!this.Verbosity.IsNullOrWhiteSpace())
        {
            args.Add("--verbosity");
            args.Add(this.Verbosity);
        }

        if (!this.Output.IsNullOrWhiteSpace())
        {
            args.Add("--output");
            args.Add(this.Output);
        }

        if (this.NoLogo)
            args.Add("--nologo");

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