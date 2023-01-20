using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Ze.PowerShell.Core;

[Cmdlet(VerbsCommon.New, "List")]
[OutputType(typeof(List<>), typeof(List<string>))]
public class NewListCmdlet : PSCmdlet
{
    [Parameter]
    public Type Type { get; set; } = typeof(string);

    protected override void ProcessRecord()
    {
        var genericType = typeof(List<>).MakeGenericType(this.Type);
        this.WriteObject(Activator.CreateInstance(genericType));
    }
}