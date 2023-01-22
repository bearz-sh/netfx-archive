using System.Management.Automation;

namespace Ze.PowerShell.DbCmd;

public class GetDbProviderFactoryCmdlet : PSCmdlet
{
    [Parameter(Position = 0)]
    public string ProviderName { get; set; } = "default";
    
    protected override void ProcessRecord()
    {
        if (DbCmdModuleState.ProviderFactories.TryGetValue(this.ProviderName, out var factory))
        {
            this.WriteObject(factory);
            return;
        }
        
        this.WriteObject(null);
    }
}