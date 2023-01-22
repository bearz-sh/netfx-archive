using System.IO;
using System.Management.Automation;

namespace Ze.PowerShell.Yaml;

public class ConvertToYamlCmdlet : PSCmdlet
{
    public object? InputObject { get; set; }

    public SerializationOptions Option { get; set; } = SerializationOptions.Roundtrip;

    protected override void ProcessRecord()
    {
        if (this.InputObject is null)
        {
            this.WriteObject(null);
            return;
        }

        using var sw = new StringWriter();
        PsYamlWriter.WriteYaml(this.InputObject, this.Option, sw);

        var value = sw.ToString();
        if (string.IsNullOrWhiteSpace(value))
        {
            this.WriteObject(null);
            return;
        }

        this.WriteObject(value);
    }
}