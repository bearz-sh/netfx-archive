using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;

using Bearz.Diagnostics;
using Bearz.Extra.Collections;
using Bearz.Extra.Strings;
using Bearz.Std;
using Bearz.Std.Unix;

namespace Ze.PowerShell.Core;

[Cmdlet(VerbsLifecycle.Invoke, "Process")]
public class InvokeProcessCommandCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0)]
    public string? Executable { get; set; }

    [Parameter]
    public CommandArgs? Arguments { get; set; }

    [Parameter]
    public Stdio StdOut { get; set; } = Stdio.Inherit;

    [Parameter]
    public Stdio StdError { get; set; } = Stdio.Inherit;

    [Parameter]
    public SwitchParameter AsSudo { get; set; }

    [Parameter]
    public ActionPreference CommandAction { get; set; } = ActionPreference.Continue;

    [Alias("Env", "e")]
    [Parameter]
    public Hashtable? Environment { get; set; }

    [Alias("Cwd", "Wd")]
    [Parameter]
    public string? WorkingDirectory { get; set; }

    public IEnumerable<IProcessCapture> StdOutCapture { get; set; } = Array.Empty<IProcessCapture>();

    public IEnumerable<IProcessCapture> StdErrorCapture { get; set; } = Array.Empty<IProcessCapture>();

    protected override void ProcessRecord()
    {
        Dictionary<string, string?>? env = null;

        if (this.Executable.IsNullOrWhiteSpace())
            throw new PSArgumentNullException(nameof(this.Executable));

        var args = this.Arguments;
        args ??= new CommandArgs();

        var exe = Process.Which(this.Executable);
        if (exe is null)
            throw new NotFoundOnPathException(this.Executable);

        if (this.AsSudo.ToBool() && !Env.IsWindows() && !UnixUser.IsRoot)
        {
            args.Unshift(exe);
            exe = "sudo";
        }

        if (this.Environment != null)
        {
            env = new Dictionary<string, string?>();
            foreach (var key in this.Environment.Keys)
            {
                if (key is string name)
                {
                    var value = this.Environment[name] as string;
                    env[name] = value;
                }
            }
        }

        var ci = new CommandStartInfo()
        {
            Args = args,
            Env = env,
            Cwd = this.WorkingDirectory,
            StdOut = this.StdOut,
            StdErr = this.StdError,
        };

        foreach (var capture in this.StdOutCapture)
        {
            ci.RedirectTo(capture);
        }

        foreach (var capture in this.StdErrorCapture)
        {
            ci.RedirectErrorTo(capture);
        }

        var cmd = Process.CreateCommand(exe, ci);

        if (this.CommandAction != ActionPreference.SilentlyContinue && this.CommandAction != ActionPreference.Ignore)
        {
            Utils.WriteCommand(exe, args);
        }

        var result = cmd.Output();
        this.SessionState.PSVariable.Set("LASTEXITCODE", result.ExitCode);
        this.WriteObject(result);
    }
}