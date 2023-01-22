using System.Management.Automation;

using Bearz.Extra.Strings;

using Microsoft.Extensions.Configuration;

namespace Ze.PowerShell.Conf;

[Cmdlet(VerbsCommon.Get, "ConfSection")]
[OutputType(typeof(IConfigurationSection))]
public class GetConfSectionCmdlet : PSCmdlet
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

        this.WriteObject(this.InputObject.GetSection(this.Key));
        base.ProcessRecord();
    }
}