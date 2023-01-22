using System.IO;
using System.Management.Automation;

namespace Ze.PowerShell.Yaml;

public class OutHostAsYamlCmdlet : PSCmdlet
{
    [Parameter(ValueFromPipeline = true)]
    public object? InputObject { get; set; }

    public SerializationOptions Option { get; set; } = SerializationOptions.Roundtrip;

    protected override void ProcessRecord()
    {
        var cmd = this.InvokeCommand.GetCmdlet("Write-Host");
        using var ps = System.Management.Automation.PowerShell.Create(RunspaceMode.CurrentRunspace);
        if (this.InputObject is null)
        {
            ps.AddCommand(cmd).AddParameter("Object", null);
            ps.Invoke();
            return;
        }

        using var sw = new StringWriter();
        PsYamlWriter.WriteYaml(this.InputObject, this.Option, sw);

        var value = sw.ToString();
        if (string.IsNullOrWhiteSpace(value))
        {
            value = null;
        }

        ps.AddCommand(cmd).AddParameter("Object", value);
        ps.Invoke();
    }
}