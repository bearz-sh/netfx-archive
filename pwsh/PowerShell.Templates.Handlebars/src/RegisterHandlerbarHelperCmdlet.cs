using System.Collections.Generic;
using System.Management.Automation;

using Bearz.Extra.Strings;

using HandlebarsDotNet;

namespace Ze.PowerShell.Templates.Handlebars;

[Alias("add_hbs_helper")]
[Cmdlet(VerbsLifecycle.Register, "HandlebarHelper")]
[OutputType(typeof(void))]
public class RegisterHandlerbarHelperCmdlet : PSCmdlet
{
    [Alias("n")]
    [Parameter(Mandatory = true, Position = 0)]
    public string? Name { get; set; }

    [Alias("")]
    [Parameter(Mandatory = true, Position = 1)]
    public ScriptBlock? Helper { get; set; }

    [Alias("uw")]
    [Parameter]
    public SwitchParameter UseWriter { get; set; }

    [Alias("hb", "Handlebars")]
    [Parameter(ValueFromPipeline = true)]
    public IHandlebars? Handlebar { get; set; }

    protected override void ProcessRecord()
    {
        if (this.Name.IsNullOrWhiteSpace())
            throw new PSArgumentNullException(nameof(this.Name));

        if (this.Helper is null)
            throw new PSArgumentNullException(nameof(this.Helper));

        if (this.UseWriter.ToBool())
        {
            if (this.Handlebar is not null)
            {
                this.Handlebar.RegisterHelper(this.Name, (writer, context, arguments) =>
                {
                    this.Helper.Invoke(this.Handlebar.Configuration, writer, context, arguments);
                });
            }
            else
            {
                HandlebarsDotNet.Handlebars.RegisterHelper(this.Name, (writer, context, arguments) =>
                {
                    this.Helper.Invoke(HandlebarsDotNet.Handlebars.Configuration, writer, context, arguments);
                });
            }
        }
        else
        {
            if (this.Handlebar is not null)
            {
                this.Handlebar.RegisterHelper(this.Name, (context, arguments) =>
                {
                    return this.Helper.InvokeReturnAsIs(HandlebarsDotNet.Handlebars.Configuration, context, arguments);
                });
            }
            else
            {
                HandlebarsDotNet.Handlebars.RegisterHelper(this.Name, (writer, context, arguments) =>
                {
                    return this.Helper.InvokeReturnAsIs(HandlebarsDotNet.Handlebars.Configuration, context, arguments);
                });
            }
        }
    }
}