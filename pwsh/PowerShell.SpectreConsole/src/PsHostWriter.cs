using System.IO;
using System.Management.Automation.Host;
using System.Text;

namespace Ze.PowerShell.SpectreConsole;

public class PsHostWriter : TextWriter
{
    public PsHostWriter(PSHost host)
    {
        this.Host = host;
    }

    public override Encoding Encoding { get; } = Encoding.UTF8;

    public PSHost Host { get; set; }

    public override void Write(char value)
    {
        this.Host.UI.Write(value.ToString());
    }

    public override void Write(string? value)
    {
        this.Host.UI.Write(value);
    }

    public override void WriteLine(string? value)
    {
        this.Host.UI.WriteLine(value);
    }

    public override void WriteLine(object? value)
    {
        this.Host.UI.WriteLine();
    }
}