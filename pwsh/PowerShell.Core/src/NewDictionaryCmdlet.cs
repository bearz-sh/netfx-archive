using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Ze.PowerShell.Core;

[Cmdlet(VerbsCommon.New, "Dictionary")]
[OutputType(typeof(Dictionary<,>), typeof(Dictionary<string, object>))]
public class NewDictionaryCmdlet : PSCmdlet
{
    [Parameter(Position = 0)]
    [ValidateNotNull]
    public Type KeyType { get; set; } = typeof(string);

    [Parameter(Position = 1)]
    [ValidateNotNull]
    public Type ValueType { get; set; } = typeof(object);

    protected override void ProcessRecord()
    {
        var dictType = typeof(Dictionary<,>).MakeGenericType(this.KeyType, this.ValueType);
        var dict = Activator.CreateInstance(dictType);
        this.WriteObject(dict);
    }
}