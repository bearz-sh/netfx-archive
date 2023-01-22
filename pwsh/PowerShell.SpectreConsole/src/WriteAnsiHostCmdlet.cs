using System;
using System.Management.Automation;
using System.Management.Automation.Host;

using Spectre.Console;

using Ze.PowerShell.SpectreConsole;

namespace Ze.PowerShell.SpectreConsole;

public class WriteAnsiHostCmdlet : PSCmdlet
{
    public object? InputObject { get; set; }

    public bool NoNewLine { get; set; }

    protected override void ProcessRecord()
    {
        if (this.InputObject is null)
        {
            if (this.NoNewLine)
            {
                this.Host.UI.Write("\r");
                return;
            }

            this.Host.UI.WriteLine();
            return;
        }

        if (this.InputObject is string str)
        {
            if (str.IsNullOrWhiteSpace())
            {
                if (this.NoNewLine)
                {
                    this.Host.UI.Write("\r");
                    return;
                }

                this.Host.UI.WriteLine();
                return;
            }

            AnsiWriter.ConsoleWriter.Host = this.Host;
            var console = AnsiWriter.Console;

            if (this.NoNewLine)
            {
                console.Markup(str + "\r");
                return;
            }

            console.MarkupLine(str);
        }

        if (this.InputObject is Exception ex)
        {
            AnsiWriter.ConsoleWriter.Host = this.Host;
            var console = AnsiWriter.Console;
            console.WriteException(ex);
            return;
        }

        using var ps = System.Management.Automation.PowerShell.Create(RunspaceMode.CurrentRunspace);
        var cmd = this.InvokeCommand.GetCmdlet("Write-Host");

        ps.AddCommand(cmd)
            .AddParameter("Object", this.InputObject)
            .AddParameter("NoNewLine", this.NoNewLine);

        ps.Invoke();
    }
}