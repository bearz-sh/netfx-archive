using System;
using System.IO;
using System.Management.Automation;

using Bearz.Extra.Strings;
using Bearz.Std;
using Bearz.Text.DotEnv;

using Microsoft.Extensions.Configuration;

namespace Ze.PowerShell.Conf;

[Cmdlet(VerbsCommon.New, "ConfBuilder")]
[OutputType(typeof(IConfigurationBuilder))]
public class NewConfBuilderCmdlet : PSCmdlet
{
    [Parameter]
    public string? BasePath { get; set; }

    [Parameter(Position = 0, ValueFromPipelineByPropertyName = true)]
    public string[]? Config { get; set; }
    
    [Parameter(ValueFromPipelineByPropertyName = true)]
    public SwitchParameter AddEnvironmentVariables { get; set; }
    
    [Parameter(ValueFromPipelineByPropertyName = true)]
    public string? EnvironmentVariablePrefix { get; set; }

    [Parameter(ValueFromPipelineByPropertyName = true)]
    public SwitchParameter ReloadOnChange { get; set; }

    protected override void ProcessRecord()
    {
        var builder = PsConfigurationBuilder.Create(
            this.BasePath,
            this.Config,
            this.AddEnvironmentVariables.ToBool(),
            this.ReloadOnChange.ToBool(),
            this.EnvironmentVariablePrefix);

        this.WriteObject(builder);
    }
}