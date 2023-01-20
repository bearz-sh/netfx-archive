using System.Collections.Generic;
using System.Management.Automation;

using Bearz.Diagnostics;

namespace Ze.PowerShell.Core;

public class NewProcessCollectionCaptureCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0)]
    public ICollection<string>? InputObject { get; set; }

    protected override void ProcessRecord()
    {
        if (this.InputObject is null)
            throw new PSArgumentNullException(nameof(this.InputObject));

        this.WriteObject(new CollectionCapture(this.InputObject));
    }
}