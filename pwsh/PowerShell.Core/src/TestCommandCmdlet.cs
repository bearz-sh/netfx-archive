using System;
using System.Linq;
using System.Management.Automation;

namespace Ze.PowerShell.Core;

[Alias("tcmd")]
[Cmdlet(VerbsDiagnostic.Test, "Command")]
[OutputType(typeof(bool))]
public class TestCommandCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    public string[] Command { get; set; } = Array.Empty<string>();

    protected override void ProcessRecord()
    {
        using var ps = System.Management.Automation.PowerShell.Create(RunspaceMode.CurrentRunspace);
        var commandInfo = this.InvokeCommand.GetCommand("Get-Command", CommandTypes.Cmdlet);
        foreach (var c in this.Command)
        {
            ps.AddCommand(commandInfo);
            ps.AddParameter("Name", c);
            ps.AddParameter("ErrorAction", ActionPreference.SilentlyContinue);
            var result = ps.Invoke<CommandInfo?>();
            if (result is null || result.Count == 0 || result[0] is null)
            {
               this.WriteObject(false);
               return;
            }
        }

        this.WriteObject(true);
    }
}