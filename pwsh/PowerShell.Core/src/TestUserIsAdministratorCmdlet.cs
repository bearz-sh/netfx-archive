using System.Management.Automation;

using Bearz.Std;

namespace Ze.PowerShell.Core;

[Alias("is-admin")]
[Cmdlet(VerbsDiagnostic.Test, "UserIsAdministrator")]
[OutputType(typeof(bool))]
public class TestUserIsAdministratorCmdlet : Cmdlet
{
    protected override void ProcessRecord()
    {
        this.WriteObject(Env.IsUserElevated);
    }
}