using System.Diagnostics.CodeAnalysis;

using Bearz.Cli.Execution;
using Bearz.Extra.Strings;
using Bearz.Std;

namespace Ze.Cli.DotNet;


public class DotNetNewListCmd : DotNetDiagnosticsCmd
{
    public DotNetNewListCmd(CliStartInfo? startInfo = null)
        : base("new list", startInfo)
    {
    }

    /// <summary>
    /// If specified, only the templates matching the name will be shown.
    /// </summary>
    public string? TemplateName { get; set; }

    /// <summary>
    /// Filters the templates based on the template author.
    /// </summary>
    public string? Author { get; set; }

    /// <summary>
    /// Filters templates based on language.
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    /// Filters the templates based on the tag.
    /// </summary>
    public string? Tag { get; set; }

    /// <summary>
    ///  The project that should be used for context evaluation.
    /// </summary>
    public string? Project { get; set; }

    /// <summary>
    ///  Location to place the generated output.
    /// </summary>
    public string? Output { get; set; }

    /// <summary>
    /// Filters templates based on available types. Predefined values are <c>project</c> and <c>item</c>.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Displays all columns in the output.
    /// </summary>
    public bool ColumnsAll { get; set; }

    /// <summary>
    /// Specifies the columns to display in the output. [author|language|tags|type].
    /// </summary>
    public string? Columns { get; set; }

    /// <summary>
    ///  Disables checking if the template meets the constraints to be run.
    /// </summary>
    public bool IgnoreConstraints { get; set; }

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

        if (!this.TemplateName.IsNullOrWhiteSpace())
            args.Add(this.TemplateName);

        if (!this.Author.IsNullOrWhiteSpace())
            args.Add("--author", this.Author);

        if (!this.Language.IsNullOrWhiteSpace())
            args.Add("-lang", this.Language);

        if (!this.Type.IsNullOrWhiteSpace())
            args.Add("--type", this.Type);

        if (!this.Tag.IsNullOrWhiteSpace())
            args.Add("--tag", this.Tag);

        if (!this.Output.IsNullOrWhiteSpace())
            args.Add("-o", this.Output);

        if (!this.Project.IsNullOrWhiteSpace())
            args.Add("--project", this.Project);

        if (this.ColumnsAll)
            args.Add("--columns-all");

        if (this.IgnoreConstraints)
            args.Add("--ignore-constraints");

        if (!this.Columns.IsNullOrWhiteSpace())
            args.Add("--columns", this.Columns);

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