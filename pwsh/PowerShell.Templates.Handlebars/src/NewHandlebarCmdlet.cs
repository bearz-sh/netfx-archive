using System.Management.Automation;

using HandlebarsDotNet;

namespace Ze.PowerShell.Templates.Handlebars;

[Alias("new_hbs")]
[Cmdlet(VerbsCommon.New, "Handlebar")]
[OutputType(typeof(IHandlebars))]
public class NewHandlebarCmdlet : PSCmdlet
{
    [Parameter(Position = 0)]
    public HandlebarsConfiguration? Configuration { get; set; }

    protected override void ProcessRecord()
    {
        var hb = HandlebarsDotNet.Handlebars.Create(this.Configuration);
        this.WriteObject(hb);
    }
}