using System.Management.Automation;

using Bearz.Std;

namespace Ze.PowerShell.Core;

[Alias("hasev")]
[Cmdlet(VerbsDiagnostic.Test, "EnvironmentVariable")]
public class TestEnvironmentVariableCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0)]
    public string[]? Name { get; set; }

    protected override void ProcessRecord()
    {
        if (this.Name is null || this.Name.Length == 0)
        {
            this.WriteObject(false);
            return;
        }

        foreach (var name in this.Name)
        {
            if (!Env.Has(name))
                this.WriteObject(false);
        }

        this.WriteObject(true);
        base.ProcessRecord();
    }
}