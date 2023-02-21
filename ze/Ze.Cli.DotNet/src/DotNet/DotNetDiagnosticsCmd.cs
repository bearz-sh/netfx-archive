using Bearz.Cli.Execution;

namespace Ze.Cli.DotNet;

public abstract class DotNetDiagnosticsCmd : DotNetCmd
{
    protected DotNetDiagnosticsCmd(string commandName, CliStartInfo? startInfo = null)
        : base(commandName, startInfo)
    {
    }

    /// <summary>
    /// Sets the verbosity level. Allowed values are q[uiet], m[inimal], n[ormal], and diag[nostic]. [default: normal].
    /// </summary>
    public string? Verbosity { get; set; }

    /// <summary>
    /// Enables diagnostic output.
    /// </summary>
    public bool Diagnostics { get; set; }
}