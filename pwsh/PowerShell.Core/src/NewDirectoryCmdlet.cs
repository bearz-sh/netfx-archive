using System;
using System.IO;
using System.Management.Automation;

using Bearz.Std;

namespace Ze.PowerShell.Core;

[Cmdlet(VerbsCommon.New, "Directory")]
[Alias("Make-Directory")]
[OutputType(typeof(DirectoryInfo), typeof(DirectoryInfo[]))]
public class NewDirectoryCmdlet : PSCmdlet
{
    [Parameter(Position = 0, ValueFromPipeline = true)]
    public string[] Path { get; set; } = Array.Empty<string>();

    protected override void ProcessRecord()
    {
        foreach (var path in this.Path)
        {
            var next = Env.Expand(path);

            if (Directory.Exists(next) || File.Exists(next))
                continue;

            this.WriteVerbose($"Creating directory '{path}'");
            System.IO.Directory.CreateDirectory(path);
            this.WriteObject(new DirectoryInfo(path));
        }

        base.ProcessRecord();
    }
}