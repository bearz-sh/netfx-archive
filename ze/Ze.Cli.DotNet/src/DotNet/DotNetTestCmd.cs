using Bearz.Cli.Execution;
using Bearz.Extra.Strings;
using Bearz.Std;

namespace Ze.Cli.DotNet;

public class DotNetTestCmd : DotNetTargetCmd
{
    public DotNetTestCmd(CliStartInfo? startInfo = null)
        : base("test", startInfo)
    {
    }

    public string? Settings { get; set; }

    public string? Os { get; set; }

    public string? Arch { get; set; }

    public bool ListTests { get; set; }

    public Dictionary<string, string> Environment { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public bool NoBuild { get; set; }

    public bool NoRestore { get; set; }

    public string? Filter { get; set; }

    public string? TestAdapterPath { get; set; }

    public string? Logger { get; set; }

    public string? Diag { get; set; }

    public string? ResultsDirectory { get; set; }

    public string? Collect { get; set; }

    public bool Blame { get; set; }

    public bool BlameCrash { get; set; }

    public string? BlameCrashCollectAlways { get; set; }

    public string? BlameCrashDumpType { get; set; }

    public bool BlameHang { get; set; }

    public string? BlameHangDumpType { get; set; }

    public TimeSpan? BlameHangTimeout { get; set; }

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

        if (!this.ResultsDirectory.IsNullOrWhiteSpace())
        {
            args.Add("--results-directory");
            args.Add(this.ResultsDirectory);
        }

        if (!this.Diag.IsNullOrWhiteSpace())
        {
            args.Add("--diag");
            args.Add(this.Diag);
        }

        if (!this.Logger.IsNullOrWhiteSpace())
        {
            args.Add("--logger");
            args.Add(this.Logger);
        }

        if (!this.Verbosity.IsNullOrWhiteSpace())
        {
            args.Add("--verbosity");
            args.Add(this.Verbosity);
        }

        if (!this.Collect.IsNullOrWhiteSpace())
        {
            args.Add("--collect");
            args.Add(this.Collect);
        }

        if (!this.Settings.IsNullOrWhiteSpace())
        {
            args.Add("--settings");
            args.Add(this.Settings);
        }

        if (!this.TestAdapterPath.IsNullOrWhiteSpace())
        {
            args.Add("--test-adapter-path");
            args.Add(this.TestAdapterPath);
        }

        if (!this.Filter.IsNullOrWhiteSpace())
        {
            args.Add("--filter");
            args.Add(this.Filter);
        }

        if (!this.BlameHangDumpType.IsNullOrWhiteSpace())
        {
            args.Add("--blame-hang-dump-type");
            args.Add(this.BlameHangDumpType);
        }

        if (!this.BlameCrashDumpType.IsNullOrWhiteSpace())
        {
            args.Add("--blame-crash-dump-type");
            args.Add(this.BlameCrashDumpType);
        }

        if (!this.BlameCrashCollectAlways.IsNullOrWhiteSpace())
        {
            args.Add("--blame-crash-collect-always");
            args.Add(this.BlameCrashCollectAlways);
        }

        if (this.BlameHangTimeout.HasValue)
        {
            args.Add("--blame-hang-timeout");
            args.Add(this.BlameHangTimeout.Value.ToString());
        }

        if (this.NoLogo)
            args.Add("--no-logo");

        if (this.NoRestore)
            args.Add("--no-restore");

        if (this.NoBuild)
            args.Add("--no-build");

        if (this.ListTests)
            args.Add("--list-tests");

        if (this.Blame)
            args.Add("--blame");

        if (this.BlameCrash)
            args.Add("--blame-crash");

        if (this.BlameHang)
            args.Add("--blame-hang");

        foreach (var kvp in this.Environment)
        {
            args.Add("-e");
            args.Add($"{kvp.Key}=\"{kvp.Value}\"");
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