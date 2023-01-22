using System.Management.Automation;

namespace Ze.PowerShell.Yaml;

public class WriteYamlCmdlet : PSCmdlet
{
    [Parameter(Position = 0, ValueFromPipelineByPropertyName = true)]
    public object? InputObject { get; set; }

    [Parameter(Position = 1, ValueFromPipelineByPropertyName = true)]
    public string? File { get; set; }

    public SerializationOptions Option { get; set; } = SerializationOptions.Roundtrip;

    protected override void ProcessRecord()
    {
        if (this.File.IsNullOrWhiteSpace())
            throw new PSArgumentNullException(nameof(this.File));

        if (this.InputObject is null)
        {
            System.IO.File.WriteAllText(this.File, string.Empty);
            return;
        }

        using var sw = System.IO.File.CreateText(this.File);
        PsYamlWriter.WriteYaml(this.InputObject, this.Option, sw);
        sw.Flush();
    }
}