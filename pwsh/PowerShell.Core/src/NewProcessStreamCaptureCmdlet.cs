using System.IO;
using System.Management.Automation;

using Bearz.Diagnostics;
using Bearz.Extra.Strings;

namespace Ze.PowerShell.Core;

[Cmdlet(VerbsCommon.New, "ProcessStreamCapture")]
[OutputType(typeof(StreamCapture))]
public class NewProcessStreamCaptureCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = "FileName")]
    public string? FileName { get; set; }

    [Parameter(Mandatory = true, Position = 0, ParameterSetName = "FileInfo")]
    public FileInfo? FileInfo { get; set; }

    [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Stream")]
    public Stream? Stream { get; set; }

    [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Writer")]
    public TextWriter? Writer { get; set; }

    [Parameter(ParameterSetName = "FileName")]
    [Parameter(ParameterSetName = "FileInfo")]
    [Parameter(ParameterSetName = "Stream")]
    public string Encoding { get; set; } = "utf8";

    [Parameter(ParameterSetName = "FileName")]
    [Parameter(ParameterSetName = "FileInfo")]
    [Parameter(ParameterSetName = "Stream")]
    public int? BufferSize { get; set; }

    [Parameter(ParameterSetName = "FileName")]
    [Parameter(ParameterSetName = "FileInfo")]
    [Parameter(ParameterSetName = "Stream")]
    public SwitchParameter KeepOpen { get; set; }

    protected override void ProcessRecord()
    {
        if (this.FileName.IsNullOrWhiteSpace() && this.FileInfo is null &&
            this.Stream is null && this.Writer is null)
        {
            throw new PSArgumentNullException(nameof(this.FileName), "FileName, FileInfo, Stream, or Writer must be specified.");
        }

        if (this.Stream is not null)
        {
            var encoding = Utils.GetEncoding(this.Encoding);
            var keepOpen = this.KeepOpen.IsPresent;
            this.BufferSize ??= 4096;
            this.WriteObject(new StreamCapture(this.Stream, encoding, this.BufferSize.Value, keepOpen));
            return;
        }

        if (!this.FileName.IsNullOrWhiteSpace())
        {
            this.FileName = Path.GetFullPath(this.FileName);
            var dir = Path.GetDirectoryName(this.FileName);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!);

            this.FileInfo = new FileInfo(this.FileName);
        }

        if (this.FileInfo is not null)
        {
            var encoding = Utils.GetEncoding(this.Encoding);
            var keepOpen = this.KeepOpen.IsPresent;
            if (this.KeepOpen)
            {
                this.WriteObject(new StreamCapture(this.FileInfo, keepOpen));
            }

            this.WriteObject(new StreamCapture(this.FileInfo, encoding));
            return;
        }

        this.WriteObject(new StreamCapture(this.Writer!));
    }
}