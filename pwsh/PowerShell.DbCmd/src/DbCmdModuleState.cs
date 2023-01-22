using System;
using System.Collections.Generic;
using System.Data.Common;

using Bearz.Extra.Strings;

using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace Ze.PowerShell.DbCmd;

public class DbCmdModuleState
{
    static DbCmdModuleState()
    {
        ProviderFactories["sqlite"] = SqliteFactory.Instance;
        ProviderFactories["default"] = SqliteFactory.Instance;
        ProviderFactories["microsoft.data.sqlite"] = SqliteFactory.Instance;
        ProviderFactories["sqlserver"] = SqlClientFactory.Instance;
        ProviderFactories["sqlclient"] = SqlClientFactory.Instance;
        ProviderFactories["microsoft.data.sqlclient"] = SqlClientFactory.Instance;
    }

    public static Dictionary<string, DbProviderFactory> ProviderFactories { get; } = new (StringComparer.OrdinalIgnoreCase);

    public static Dictionary<string, string> ConnectionStrings { get; } = new(StringComparer.OrdinalIgnoreCase);

    public static string GetConnectionString(string? name)
    {
        if (name.IsNullOrWhiteSpace())
            name = "default";

        if (ConnectionStrings.TryGetValue(name, out var connectionString))
        {
            return connectionString;
        }

        throw new ArgumentException($"Connection string '{name}' not found.");
    }

    public static DbProviderFactory GetFactory(string? providerName)
    {
        if (providerName.IsNullOrWhiteSpace())
            providerName = "default";

        if (!ProviderFactories.TryGetValue(providerName, out var factory))
            throw new ArgumentException($"Unknown provider name '{providerName}'");

        return factory;
    }
}