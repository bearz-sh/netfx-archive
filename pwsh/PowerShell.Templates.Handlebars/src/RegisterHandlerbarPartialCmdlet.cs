using System.Management.Automation;

using Bearz.Extra.Strings;

using HandlebarsDotNet;

namespace Ze.PowerShell.Templates.Handlebars;

[Alias("add_hbs_partial")]
[Cmdlet(VerbsLifecycle.Register, "HandlebarPartial")]
[OutputType(typeof(void))]
public class RegisterHandlerbarPartialCmdlet : PSCmdlet
{
    [Alias("n")]
    [Parameter(Position = 0)]
    [ValidateNotNullOrEmpty]
    public string? Name { get; set; }

    [Parameter(Position = 1)]
    [ValidateNotNullOrEmpty]
    public string? Partial { get; set; }

    [Alias("hb", "Handlebars")]
    [Parameter(ValueFromPipeline = true)]
    public IHandlebars? Handlebar { get; set; }

    protected override void ProcessRecord()
    {
        if (this.Partial.IsNullOrWhiteSpace())
        {
            throw new PSArgumentNullException(nameof(this.Partial));
        }

        if (this.Name.IsNullOrWhiteSpace())
        {
            throw new PSArgumentNullException(nameof(this.Name));
        }

        if (this.Handlebar is not null)
        {
            this.Handlebar.RegisterTemplate(this.Name, this.Partial);
            return;
        }

        HandlebarsDotNet.Handlebars.RegisterTemplate(this.Name, this.Partial);
    }
}