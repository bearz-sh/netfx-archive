using Bearz.Cli.Execution;
using Bearz.Extra.Strings;
using Bearz.Std;

namespace Ze.Cli.DotNet;

public class DotNetNewCmd : DotNetDiagnosticsCmd
{
    public DotNetNewCmd(CliStartInfo? startInfo = null)
        : base("new", startInfo)
    {
    }

    /// <summary>
    /// A short name of the template to create.
    /// </summary>
    public string Template { get; set; } = string.Empty;

    /// <summary>
    /// Template specific options to use.
    /// </summary>
    public CommandArgs TemplateArgs { get; set; } = new CommandArgs();

    /// <summary>
    /// The project that should be used for context evaluation.
    /// </summary>
    public string? Project { get; set; }

    /// <summary>
    /// The name for the output being created. If no name is specified, the name of the output directory is used.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Location to place the generated output.
    /// </summary>
    public string? Output { get; set; }

    /// <summary>
    /// Displays a summary of what would happen if the given command line were run if it would result in a template creation.
    /// </summary>
    public bool DryRun { get; set; }

    /// <summary>
    /// Forces content to be generated even if it would change existing files.
    /// </summary>
    public bool Force { get; set; }

    /// <summary>
    /// Disables checking for the template package updates when instantiating a template.
    /// </summary>
    public bool NoUpdateCheck { get; set; }

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

        if (this.Template.IsNullOrWhiteSpace())
            throw new InvalidOperationException("Template is required");

        if (!this.Name.IsNullOrWhiteSpace())
            args.Add("-n", this.Name);

        if (!this.Output.IsNullOrWhiteSpace())
            args.Add("-o", this.Output);

        if (this.DryRun)
            args.Add("--dry-run");

        if (this.Force)
            args.Add("--force");

        if (this.Diagnostics)
            args.Add("--diagnostics");

        if (this.NoUpdateCheck)
            args.Add("--no-update-check");

        if (!this.Verbosity.IsNullOrWhiteSpace())
            args.Add("--verbosity", this.Verbosity);

        if (!this.Project.IsNullOrWhiteSpace())
            args.Add("--project", this.Project);

        foreach (var next in this.TemplateArgs)
            args.Add(next);

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