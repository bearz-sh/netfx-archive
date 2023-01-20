using System;
using System.Collections;
using System.IO;
using System.Management.Automation;

namespace Ze.PowerShell.Core;

[Cmdlet(VerbsCommunications.Read, "Json")]
[OutputType(typeof(PSObject), typeof(Hashtable), typeof(Array))]
public class ReadJsonCmdlet : PSCmdlet
{
    [Parameter(ValueFromPipeline = true, Position = 0)]
    [ValidateNotNullOrEmpty]
    public string Path { get; set; } = string.Empty;

    [Parameter]
    public int? Depth { get; set; }

    [Parameter]
    public SwitchParameter AsHashtable { get; set; }

    [Parameter]
    public SwitchParameter NoEnumerate { get; set; }

    protected override void ProcessRecord()
    {
        var path = System.IO.Path.GetFullPath(this.Path);
        if (!File.Exists(path))
            throw new FileNotFoundException(path);

        var json = File.ReadAllText(path);
        using var ps = System.Management.Automation.PowerShell.Create(RunspaceMode.CurrentRunspace);
        var commandInfo = this.InvokeCommand.GetCommand("ConvertFrom-Json", CommandTypes.Cmdlet);

        ps.AddCommand(commandInfo)
            .AddParameter("InputObject", json);

        if (this.Host.Version.Major > 5)
        {
            if (this.Depth.HasValue)
                ps.AddParameter("Depth", this.Depth.Value);

            ps.AddParameter("AsHashtable", this.AsHashtable);
            ps.AddParameter("NoEnumerate", this.NoEnumerate);

            var result = ps.Invoke();
            this.WriteObject(result, false);
            return;
        }

        if (this.AsHashtable.ToBool())
        {
            var ht = ConvertToHashtableCmdlet.Convert(ps.Invoke());
            this.WriteObject(ht, false);
            return;
        }

        this.WriteObject(ps.Invoke(), false);
    }
}