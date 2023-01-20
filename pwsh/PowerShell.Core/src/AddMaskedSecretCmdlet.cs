using System;
using System.Management.Automation;

using Bearz.Secrets;

namespace Ze.PowerShell.Core;

public class AddMaskedSecretCmdlet : PSCmdlet
{
    [Parameter(Position = 0, Mandatory = true)]
    public string[] InputObject { get; set; } = Array.Empty<string>();

    protected override void ProcessRecord()
    {
        foreach (var secret in this.InputObject)
        {
            SecretMasker.Default.Add(secret);
        }
    }
}