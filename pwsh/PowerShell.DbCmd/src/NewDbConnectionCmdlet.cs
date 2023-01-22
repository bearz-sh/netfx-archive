using System;
using System.Collections.Generic;
using System.Management.Automation;

using Bearz.Extra.Strings;

namespace Ze.PowerShell.DbCmd;

public class NewDbConnectionCmdlet : PSCmdlet
{
    public string? ConnectionString { get; set; }

    public string? ProviderName { get; set; }

    public ScriptBlock? Do { get; set; }

    protected override void ProcessRecord()
    {
        var providerName = this.ProviderName ?? "default";
        var connectionString = this.ConnectionString;
        if (connectionString.IsNullOrWhiteSpace())
        {
            if (!DbCmdModuleState.ConnectionStrings.TryGetValue("default", out var cs))
            {
                throw new PSArgumentNullException(
                    nameof(this.ConnectionString),
                    "ConnectionString is null or empty and no default connection string is set.");
            }

            connectionString = cs;
        }

        if (!DbCmdModuleState.ProviderFactories.TryGetValue(providerName, out var factory))
        {
            throw new PSArgumentException($"Unknown provider name '{providerName}'.", nameof(this.ProviderName));
        }

        var connection = factory.CreateConnection();
        if (connection is null)
#pragma warning disable S112
            throw new NullReferenceException($"Factory {factory.GetType().FullName} returned null connection");
#pragma warning restore S112
        connection.ConnectionString = connectionString;

        if (this.Do is null)
        {
            this.WriteObject(connection);
            return;
        }

        try
        {
            connection.Open();
            var variables = new List<PSVariable>()
            {
                new PSVariable("_", connection),
                new PSVariable("Connection", connection),
            };
            this.Do.InvokeWithContext(new Dictionary<string, ScriptBlock>(), variables);
        }
        finally
        {
            connection.Dispose();
        }
    }
}