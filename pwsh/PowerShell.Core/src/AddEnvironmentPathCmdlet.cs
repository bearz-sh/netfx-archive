using System;
using System.Management.Automation;

using Bearz.Std;

namespace Ze.PowerShell.Core;

[Cmdlet(VerbsCommon.Add, "EnvironmentPath")]
[OutputType(typeof(void))]
public class AddEnvironmentPathCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    public string[] Path { get; set; } = Array.Empty<string>();

    [Parameter]
    public EnvironmentVariableTarget Target { get; set; } = EnvironmentVariableTarget.Process;

    [Parameter]
    public SwitchParameter Prepend { get; set; }

    protected override void ProcessRecord()
    {
        foreach (var path in this.Path)
        {
            if (string.IsNullOrWhiteSpace(path))
                continue;

            EnvPath.Add(path, this.Prepend, this.Target);
        }

        base.ProcessRecord();
    }
}