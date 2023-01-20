using System;
using System.Management.Automation;

using Bearz.Extra.Strings;
using Bearz.Secrets;
using Bearz.Std;

namespace Ze.PowerShell.Core;

[Cmdlet(VerbsCommunications.Write, "Command")]
public class WriteCommandCmdlet : PSCmdlet
{
    [Parameter(Position = 0)]
    [ValidateNotNullOrEmpty]
    public string? Command { get; set; }

    [Parameter(Position = 1)]
    public CommandArgs? Arguments { get; set; }

    protected override void ProcessRecord()
    {
        var preference = this.SessionState.PSVariable.Get("CommandActionPreference");
        if (preference is null)
        {
            preference = new PSVariable("CommandActionPreference", ActionPreference.Continue, ScopedItemOptions.AllScope);
            this.SessionState.PSVariable.Set(preference);
        }
        else if (preference.Value is string value)
        {
            if (!Enum.TryParse<ActionPreference>(value, true, out var actionPreference))
            {
                preference.Value = ActionPreference.Continue;
            }
            else
            {
                preference.Value = actionPreference;
            }
        }

        if (preference.Value is ActionPreference ap &&
            (ap == ActionPreference.SilentlyContinue || ap == ActionPreference.Ignore))
        {
            return;
        }

        if (this.Command.IsNullOrWhiteSpace())
            throw new PSArgumentNullException(nameof(this.Command));

        this.Arguments ??= new CommandArgs();
        Utils.WriteCommand(this.Command, this.Arguments);
    }
}