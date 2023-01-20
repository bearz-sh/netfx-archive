using System;
using System.Management.Automation;

using Bearz.Std;

namespace Ze.PowerShell.Core;

[Cmdlet(VerbsCommon.Remove, "EnvironmentPath")]
[OutputType(typeof(void))]
public class RemoveEnvironmentPathCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    public string[] Path { get; set; } = Array.Empty<string>();

    [Parameter]
    public EnvironmentVariableTarget Target { get; set; } = EnvironmentVariableTarget.Process;

    protected override void ProcessRecord()
    {
        foreach (var path in this.Path)
        {
            if (string.IsNullOrWhiteSpace(path))
                continue;

            EnvPath.Delete(path);
        }
    }
}