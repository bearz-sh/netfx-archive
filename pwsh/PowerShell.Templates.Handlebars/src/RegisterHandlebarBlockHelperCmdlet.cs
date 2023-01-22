using System.Management.Automation;

using Bearz.Extra.Strings;

using HandlebarsDotNet;

namespace Ze.PowerShell.Templates.Handlebars;

[Alias("add_hbs_block_helper")]
[Cmdlet(VerbsLifecycle.Register, "HandlebarBlockHelper")]
[OutputType(typeof(void))]
public class RegisterHandlebarBlockHelperCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0)]
    public string? Name { get; set; }

    [Parameter(Mandatory = true, Position = 1)]
    public ScriptBlock? Helper { get; set; }

    public SwitchParameter UseWriter { get; set; }

    [Parameter(ValueFromPipeline = true)]
    public IHandlebars? Handlebar { get; set; }

    protected override void ProcessRecord()
    {
        if (this.Name.IsNullOrWhiteSpace())
            throw new PSArgumentNullException(nameof(this.Name));

        if (this.Helper is null)
            throw new PSArgumentNullException(nameof(this.Helper));

        if (this.Handlebar is not null)
        {
            this.Handlebar.RegisterHelper(this.Name, (output, options, context, arguments) =>
            {
                this.Helper.InvokeReturnAsIs(this.Handlebar.Configuration, output, options, context, arguments);
            });
            return;
        }

        HandlebarsDotNet.Handlebars.RegisterHelper(this.Name, (output, options, context, arguments) =>
        {
            this.Helper.InvokeReturnAsIs(HandlebarsDotNet.Handlebars.Configuration, output, options, context, arguments);
        });
    }
}