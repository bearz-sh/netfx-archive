using System.Management.Automation;

using Bearz.Extra.Strings;

using Microsoft.Extensions.Configuration;

namespace Ze.PowerShell.Conf;

public class GetConfConnectionStringCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    public IConfiguration? InputObject { get; set; }

    [Parameter(Position = 1)]
    [ValidateNotNullOrEmpty]
    public string? Key { get; set; }

    protected override void ProcessRecord()
    {
        if (this.InputObject is null)
            throw new PSArgumentNullException(nameof(this.InputObject));

        if (this.Key.IsNullOrWhiteSpace())
            throw new PSArgumentNullException(nameof(this.Key));

        this.WriteObject(this.InputObject.GetConnectionString(this.Key));
    }
}