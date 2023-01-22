using System;
using System.Management.Automation;

using Bearz.Templates.Handlebars;

using HandlebarsDotNet;

namespace Ze.PowerShell.Templates.Handlebars;

[Alias("add_hbs_builtin_helper")]
[Cmdlet(VerbsLifecycle.Register, "HandlebarBuiltInHelper")]
[OutputType(typeof(void))]
public class RegisterHandlebarBuiltinHelperCmdlet : PSCmdlet
{
    [Alias("hb")]
    [Parameter(ValueFromPipeline = true)]
    public IHandlebars? Handlebar { get; set; }

    [Alias("n")]
    [Parameter(Position = 0, Mandatory = true)]
    [ValidateSet("datetime", "json", "strings", "regex", "env")]
    public string[] Name { get; set; } = Array.Empty<string>();

    protected override void ProcessRecord()
    {
        foreach (var name in this.Name)
        {
            switch (name)
            {
                case "env":
                    if (this.Handlebar is null)
                    {
                        EnvHelpers.RegisterEnvHelpers(null);
                    }
                    else
                    {
                        this.Handlebar.RegisterEnvHelpers();
                    }

                    break;

                case "datetime":
                    if (this.Handlebar is null)
                    {
                        DateTimeHelpers.RegisterDateTimeHelpers(null);
                    }
                    else
                    {
                        this.Handlebar.RegisterDateTimeHelpers();
                    }

                    break;

                case "json":
                    if (this.Handlebar is null)
                    {
                        JsonHelpers.RegisterJsonHelpers(null);
                    }
                    else
                    {
                        this.Handlebar.RegisterJsonHelpers();
                    }

                    break;

                case "strings":
                    if (this.Handlebar is null)
                    {
                        StringHelpers.RegisterStringHelpers(null);
                    }
                    else
                    {
                        this.Handlebar.RegisterStringHelpers();
                    }

                    break;

                case "regex":
                    if (this.Handlebar is null)
                    {
                        RegexHelpers.RegisterRegexHelpers(null);
                    }
                    else
                    {
                        this.Handlebar.RegisterRegexHelpers();
                    }

                    break;
            }
        }
    }
}