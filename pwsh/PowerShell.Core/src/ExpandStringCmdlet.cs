using System.Collections;
using System.Management.Automation;

namespace Ze.PowerShell.Core;

public class ExpandStringCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
    public string? InputObject { get; set; }

    [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
    public IDictionary? Variables { get; set; }

    protected override void BeginProcessing()
    {
        if (this.Variables is not null)
        {
            foreach (var key in this.Variables)
            {
                if (key is string name)
                {
                    this.SessionState.PSVariable.Set(name, this.Variables[key]);
                }
            }
        }
    }

    protected override void ProcessRecord()
    {
        var result = this.InvokeCommand.ExpandString(this.InputObject);
        this.WriteObject(result ?? string.Empty);
    }

    protected override void EndProcessing()
    {
        if (this.Variables is not null)
        {
            foreach (var key in this.Variables)
            {
                if (key is string name)
                {
                    this.SessionState.PSVariable.Remove(name);
                }
            }
        }
    }
}