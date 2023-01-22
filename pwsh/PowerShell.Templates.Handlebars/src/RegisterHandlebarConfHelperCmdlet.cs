using System.Management.Automation;

using Bearz.Templates.Handlebars;

using HandlebarsDotNet;

using Microsoft.Extensions.Configuration;

namespace Ze.PowerShell.Templates.Handlebars;

[Alias("add_hbs_conf_helper")]
[Cmdlet(VerbsLifecycle.Register, "HandlebarConfHelper")]
[OutputType(typeof(void))]
public class RegisterHandlebarConfHelperCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0)]
    public IConfiguration? Configuration { get; set; }

    [Parameter(ValueFromPipeline = true)]
    public IHandlebars? Handlebar { get; set; }

    protected override void ProcessRecord()
    {
        if (this.Configuration is null)
            throw new PSArgumentNullException(nameof(this.Configuration));

        if (this.Handlebar is not null)
        {
            this.Handlebar.RegisterConfHelpers(this.Configuration);
            return;
        }

        ConfHelpers.RegisterConfHelpers(null, this.Configuration);
    }
}