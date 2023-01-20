using System;
using System.Management.Automation;

using Bearz.Std;

namespace Ze.PowerShell.Core;

[Cmdlet(VerbsDiagnostic.Test, "EnvironmentPath")]
[OutputType(typeof(bool))]
public class TestEnvironmentPathCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0)]
    public string[] Path { get; set; } = Array.Empty<string>();

    protected override void ProcessRecord()
    {
        foreach (var path in this.Path)
        {
            if (!EnvPath.Has(path))
            {
                this.WriteObject(false);
                return;
            }
        }

        this.WriteObject(true);
    }
}