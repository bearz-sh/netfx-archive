using Bearz.Cli.Execution;
using Bearz.Extra.Strings;
using Bearz.Std;

using FluentBuilder;

namespace Ze.Cli.DotNet;

[AutoGenerateBuilder]
public class DotNetBuildCmd : DotNetTargetCmd
{
    public DotNetBuildCmd(CliStartInfo? startInfo = null)
        : base("build", startInfo)
    {
    }

    public Dictionary<string, string> Properties { get; set; } = new();

    public bool UseCurrentRuntime { get; set; }

    public string? Os { get; set; }

    public string? Arch { get; set; }

    public bool Debug { get; set; }

    public bool NoIncremental { get; set; }

    public bool NoRestore { get; set; }

    public bool NoDependencies { get; set; }

    public bool SelfContained { get; set; }

    public bool NoSelfContained { get; set; }

    public bool DisableBuildServers { get; set; }

    public override CommandStartInfo Build()
    {
        var args = this.Args;
        var si = this.CliStartInfo;

        args.Add("build");

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

        if (!this.Os.IsNullOrWhiteSpace())
        {
            args.Add("--os");
            args.Add(this.Os);
        }

        if (!this.Arch.IsNullOrWhiteSpace())
        {
            args.Add("--arch");
            args.Add(this.Arch);
        }

        if (!this.Output.IsNullOrWhiteSpace())
        {
            args.Add("--output");
            args.Add(this.Output);
        }

        if (!this.Verbosity.IsNullOrWhiteSpace())
        {
            args.Add("--verbosity");
            args.Add(this.Verbosity);
        }

        if (this.NoDependencies)
            args.Add("--no-dependencies");

        if (this.NoIncremental)
            args.Add("--no-incremental");

        if (this.NoLogo)
            args.Add("--nologo");

        if (this.NoRestore)
            args.Add("--no-restore");

        if (this.SelfContained)
            args.Add("--self-contained");

        if (this.NoSelfContained)
            args.Add("--no-self-contained");

        if (this.DisableBuildServers)
            args.Add("--disable-build-servers");

        if (this.Debug)
            args.Add("--debug");

        if (this.Properties.Count > 0)
        {
            foreach (var kvp in this.Properties)
            {
                args.Add($"-p:{kvp.Key}=\"{kvp.Value}\"");
            }
        }

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