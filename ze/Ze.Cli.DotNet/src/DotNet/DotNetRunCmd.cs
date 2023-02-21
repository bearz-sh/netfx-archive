using Bearz.Cli.Execution;
using Bearz.Extra.Strings;
using Bearz.Std;

namespace Ze.Cli.DotNet;

public class DotNetRunCmd : DotNetCmd
{
    public DotNetRunCmd(CliStartInfo? startInfo = null)
        : base("run", startInfo)
    {
    }

    public string? Project { get; set; }

    public Dictionary<string, string> Properties { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public string? LaunchProfile { get; set; }

    public bool NoLaunchProfile { get; set; }

    public string? Framework { get; set; }

    public string? Configuration { get; set; }

    public string? Runtime { get; set; }

    public string? Verbosity { get; set; }

    public string? Os { get; set; }

    public string? Arch { get; set; }

    public bool NoBuild { get; set; }

    public bool NoRestore { get; set; }

    public bool SelfContained { get; set; }

    public bool NoSelfContained { get; set; }

    public CommandArgs RemainingArgs { get; } = new();

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

        if (!this.Project.IsNullOrWhiteSpace())
            args.Add("--project", this.Project);

        if (!this.Framework.IsNullOrWhiteSpace())
            args.Add("--framework", this.Framework);

        if (!this.Configuration.IsNullOrWhiteSpace())
            args.Add("--configuration", this.Configuration);

        if (!this.Runtime.IsNullOrWhiteSpace())
            args.Add("--runtime", this.Runtime);

        if (!this.Os.IsNullOrWhiteSpace())
            args.Add("--os", this.Os);

        if (!this.Arch.IsNullOrWhiteSpace())
            args.Add("--arch", this.Arch);

        if (!this.LaunchProfile.IsNullOrWhiteSpace())
            args.Add("--launch-profile", this.LaunchProfile);

        if (this.NoLaunchProfile)
            args.Add("--no-launch-profile");

        if (this.NoBuild)
            args.Add("--no-build");

        if (this.NoRestore)
            args.Add("--no-restore");

        if (!this.Verbosity.IsNullOrWhiteSpace())
            args.Add("--verbosity", this.Verbosity);

        if (this.SelfContained)
            args.Add("--self-contained");

        if (this.NoSelfContained)
            args.Add("--no-self-contained");

        foreach (var kvp in this.Properties)
            args.Add($"-p:{kvp.Key}=\"{kvp.Value}\"");

        if (this.RemainingArgs.Count > 0)
        {
            args.Add("--");

            foreach (var arg in this.RemainingArgs)
            {
                args.Add(arg);
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