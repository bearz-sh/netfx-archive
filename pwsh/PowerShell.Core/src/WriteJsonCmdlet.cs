using System;
using System.IO;
using System.Management.Automation;

using Bearz.Extra.Strings;

namespace Ze.PowerShell.Core;

[Cmdlet(VerbsCommunications.Write, "Json")]
[OutputType(typeof(FileInfo))]
public class WriteJsonCmdlet : PSCmdlet
{
    [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
    public object? InputObject { get; set; }

    [Parameter(Position = 1)]
    [ValidateNotNullOrEmpty]
    public string? Path { get; set; }

    [Parameter]
    public int Depth { get; set; } = 10;

    [Parameter]
    public SwitchParameter Compress { get; set; } = false;

    [Parameter]
    public SwitchParameter EnumsAsStrings { get; set; } = false;

    [Alias("f")]
    [Parameter]
    public SwitchParameter Force { get; set; } = false;

    protected override void ProcessRecord()
    {
        using var ps = System.Management.Automation.PowerShell.Create(RunspaceMode.CurrentRunspace);
        var commandInfo = this.InvokeCommand.GetCommand("ConvertTo-Json", CommandTypes.Cmdlet);

        if (this.InputObject is null)
        {
            throw new PSArgumentNullException(nameof(this.InputObject));
        }

        if (this.Path.IsNullOrWhiteSpace())
        {
            throw new PSArgumentException(nameof(this.InputObject));
        }

        var path = System.IO.Path.GetFullPath(this.Path);
        var dir = System.IO.Path.GetDirectoryName(this.Path);

        if (!Directory.Exists(dir))
        {
            if (!this.Force.ToBool())
            {
                throw new DirectoryNotFoundException(dir);
            }

            Directory.CreateDirectory(dir!);
        }

        if (this.InputObject is string content)
        {
            File.WriteAllText(this.Path, content);
            return;
        }

        ps.AddCommand(commandInfo)
            .AddParameter("InputObject", this.InputObject)
            .AddParameter("Depth", this.Depth)
            .AddParameter("Compress", this.Compress);

        if (this.Host.Version.Major > 5)
        {
            ps.AddParameter("EnumsAsStrings", this.EnumsAsStrings);
        }

        var result = ps.Invoke<string>();

        if (result is not null && result.Count > 0)
        {
            File.WriteAllText(path, result[0] ?? string.Empty);
            this.WriteObject(new FileInfo(path));
        }
        else
        {
            throw new InvalidOperationException("Unable to convert object to JSON");
        }
    }
}