using System.Management.Automation;

namespace Ze.PowerShell.Templates.Handlebars;

[Alias("new_hbs_config")]
[Cmdlet(VerbsCommon.New, "HandlebarConfiguration")]
[OutputType(typeof(HandlebarsDotNet.HandlebarsConfiguration))]
public class NewHandlerConfigurationCmdlet : PSCmdlet
{
    protected override void ProcessRecord()
    {
        this.WriteObject(new HandlebarsDotNet.HandlebarsConfiguration());
    }
}