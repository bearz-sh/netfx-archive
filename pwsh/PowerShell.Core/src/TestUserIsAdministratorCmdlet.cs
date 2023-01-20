using System.Management.Automation;

using Bearz.Std;

namespace Ze.PowerShell.Core;

[Alias("isadmin")]
[Cmdlet(VerbsDiagnostic.Test, "UserIsAdministrator")]
[OutputType(typeof(bool))]
public class TestUserIsAdministratorCmdlet : Cmdlet
{
    protected override void ProcessRecord()
    {
        this.WriteObject(Env.IsUserElevated);
    }
}