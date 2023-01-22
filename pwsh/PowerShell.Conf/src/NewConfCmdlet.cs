using System.Management.Automation;

using Microsoft.Extensions.Configuration;

namespace Ze.PowerShell.Conf;

[Cmdlet(VerbsCommon.New, "Conf")]
[OutputType(typeof(IConfigurationRoot))]
public class NewConfCmdlet : PSCmdlet
{
    [Parameter(Position = 0, ValueFromPipeline = true)]
    public ConfigurationBuilder? InputObject { get; set; }

    [Parameter(ValueFromPipelineByPropertyName = true)]
    public string? BasePath { get; set; }

    [Parameter(Position = 1, ValueFromPipelineByPropertyName = true)]
    public string[]? Config { get; set; }

    [Parameter(ValueFromPipelineByPropertyName = true)]
    public SwitchParameter AddEnvironmentVariables { get; set; }

    [Parameter(ValueFromPipelineByPropertyName = true)]
    public string? EnvironmentVariablePrefix { get; set; }

    [Parameter(ValueFromPipelineByPropertyName = true)]
    public SwitchParameter ReloadOnChange { get; set; }

    protected override void ProcessRecord()
    {
        var builder = this.InputObject ?? new ConfigurationBuilder();
        PsConfigurationBuilder.Configure(
            builder,
            this.BasePath,
            this.Config,
            this.AddEnvironmentVariables.ToBool(),
            this.ReloadOnChange.ToBool(),
            this.EnvironmentVariablePrefix);

        this.WriteObject(builder.Build());
    }
}