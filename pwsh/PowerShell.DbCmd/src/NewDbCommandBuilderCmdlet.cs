using System;
using System.Management.Automation;

namespace Ze.PowerShell.DbCmd;

public class NewDbCommandBuilderCmdlet : PSCmdlet
{
    public string? ProviderName { get; set; }

    protected override void ProcessRecord()
    {
        var providerName = this.ProviderName ?? "default";
        if (!DbCmdModuleState.ProviderFactories.TryGetValue(providerName, out var factory))
        {
            throw new PSArgumentException($"Unknown provider name '{providerName}'.", nameof(this.ProviderName));
        }

        var builder = factory.CreateCommandBuilder();
        if (builder is null)
#pragma warning disable S112
            throw new NullReferenceException($"DbProviderFactory {factory.GetType().FullName} returned null from CreateCommandBuilder.");
#pragma warning restore S112

        this.WriteObject(builder);
    }
}