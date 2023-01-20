using System;
using System.IO;
using System.Management.Automation;

using Bearz.Extra.Strings;
using Bearz.Std;

namespace Ze.PowerShell.Core;

[Cmdlet(VerbsDiagnostic.Test, "Directory")]
[OutputType(typeof(bool))]
public class TestDirectoryCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = "fi")]
    public FileSystemInfo? InputObject { get; set; }

    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = "path")]
    public string? Path { get; set; }

    protected override void ProcessRecord()
    {
        if (this.InputObject is null && this.Path.IsNullOrWhiteSpace())
        {
            throw new PSArgumentNullException(nameof(this.InputObject),  "InputObject and Path cannot both be null");
        }

        try
        {

            bool isDirectory = false;
            if (this.InputObject is not null)
            {
                isDirectory = this.InputObject.Attributes.HasFlag(FileAttributes.Directory);
            }
            else if (!this.Path.IsNullOrWhiteSpace())
            {
                isDirectory = File.GetAttributes(this.Path).HasFlag(FileAttributes.Directory);
            }

            this.WriteObject(isDirectory);
        }
        catch (Exception ex)
        {
            this.WriteError(new ErrorRecord(ex, "TestDirectoryCmdlet", ErrorCategory.InvalidOperation, this.Path));
            this.WriteObject(false);
        }
    }
}