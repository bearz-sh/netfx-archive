using System.Management.Automation;

namespace Ze.PowerShell.Yaml;

public class ReadYamlCmdlet : PSCmdlet
{
    public string? File { get; set; }

    public SwitchParameter AsHashtable { get; set; }

    public SwitchParameter Merge { get; set; }

    public SwitchParameter All { get; set; }

    protected override void ProcessRecord()
    {
        if (this.File.IsNullOrWhiteSpace())
            throw new PSArgumentNullException(nameof(this.File));

        var content = System.IO.File.ReadAllText(this.File);

        var result = PsYamlReader.ReadYaml(
            content,
            this.Merge.ToBool(),
            this.All.ToBool(),
            this.AsHashtable.ToBool());

        this.WriteObject(result);
    }
}