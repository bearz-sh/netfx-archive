using System.Management.Automation;

using Bearz.Extra.Strings;

using Microsoft.Extensions.Configuration;

namespace Ze.PowerShell.Conf;

[Cmdlet(VerbsCommon.Get, "ConfChildren")]
[OutputType(typeof(IConfigurationSection))]
public class GetConfChildrenCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    public IConfiguration? InputObject { get; set; }

    [Parameter(Position = 1)]
    public string? Key { get; set; }

    protected override void ProcessRecord()
    {
        if (this.InputObject is null)
            throw new PSArgumentNullException(nameof(this.InputObject));

        if (this.Key.IsNullOrWhiteSpace())
        {
            this.WriteObject(this.InputObject.GetChildren(), false);
            return;
        }

        var section = this.InputObject.GetSection(this.Key);
        this.WriteObject(section.GetChildren());
    }
}