using System.Management.Automation;

using Bearz.Std;

namespace Ze.PowerShell.Core;

[Cmdlet(VerbsCommon.Set, "UserInteractive")]
[OutputType(typeof(void))]
public class SetUserInteractiveCmdlet : PSCmdlet
{
    [Parameter(Position = 0)]
    public bool Interactive { get; set; }

    protected override void ProcessRecord()
    {
        ModuleState.ShellInteractive = this.Interactive;
        Env.IsUserInteractive = this.Interactive;
        Env.Set("POWERSHELL_INTERACTIVE", this.Interactive.ToString());
    }
}