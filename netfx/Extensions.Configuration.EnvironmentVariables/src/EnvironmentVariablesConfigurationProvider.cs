// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;

using Microsoft.Extensions.Configuration;

namespace Bearz.Extensions.Configuration.EnvironmentVariables;

/// <summary>
/// An environment variable based <see cref="ConfigurationProvider"/>.
/// </summary>
public class EnvironmentVariablesConfigurationProvider : ConfigurationProvider
{
    private const string MySqlServerPrefix = "MYSQLCONNSTR_";
    private const string SqlAzureServerPrefix = "SQLAZURECONNSTR_";
    private const string SqlServerPrefix = "SQLCONNSTR_";
    private const string CustomConnectionStringPrefix = "CUSTOMCONNSTR_";

    private readonly string prefix;
    private readonly string normalizedPrefix;
    private readonly string delimiter;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvironmentVariablesConfigurationProvider"/> class.
    /// </summary>
    /// <param name="delimiter">The delimiter to use to create namespaced configuration entries.</param>
    public EnvironmentVariablesConfigurationProvider(string delimiter = "__")
    {
        this.prefix = string.Empty;
        this.normalizedPrefix = string.Empty;
        this.delimiter = delimiter;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvironmentVariablesConfigurationProvider"/> class.
    /// The overload allows the caller to specify a prefix to filter the environment variables.
    /// </summary>
    /// <param name="prefix">A prefix used to filter the environment variables.</param>
    /// <param name="delimiter">The delimiter to use to create namespaced configuration entries.</param>
    public EnvironmentVariablesConfigurationProvider(string? prefix, string delimiter = "__")
    {
        this.delimiter = delimiter;
        this.prefix = prefix ?? string.Empty;
        this.normalizedPrefix = this.Normalize(this.prefix);
    }

    /// <summary>
    /// Loads the environment variables.
    /// </summary>
    public override void Load() =>
        this.Load(Environment.GetEnvironmentVariables());

    /// <summary>
    /// Generates a string representing this provider name and relevant details.
    /// </summary>
    /// <returns> The configuration name. </returns>
    public override string ToString()
        => $"{this.GetType().Name} Prefix: '{this.prefix}'";

    internal void Load(IDictionary envVariables)
    {
        var data = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        IDictionaryEnumerator e = envVariables.GetEnumerator();
        try
        {
            while (e.MoveNext())
            {
                string key = (string)e.Entry.Key;
                string? value = (string?)e.Entry.Value;

                if (key.StartsWith(MySqlServerPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    this.HandleMatchedConnectionStringPrefix(data, MySqlServerPrefix, "MySql.Data.MySqlClient", key, value);
                }
                else if (key.StartsWith(SqlAzureServerPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    this.HandleMatchedConnectionStringPrefix(data, SqlAzureServerPrefix, "System.Data.SqlClient", key, value);
                }
                else if (key.StartsWith(SqlServerPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    this.HandleMatchedConnectionStringPrefix(data, SqlServerPrefix, "System.Data.SqlClient", key, value);
                }
                else if (key.StartsWith(CustomConnectionStringPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    this.HandleMatchedConnectionStringPrefix(data, CustomConnectionStringPrefix, null, key, value);
                }
                else
                {
                    this.AddIfNormalizedKeyMatchesPrefix(data, this.Normalize(key), value);
                }
            }
        }
        finally
        {
            (e as IDisposable)?.Dispose();
        }

        this.Data = data;
    }

    private string Normalize(string key) => key.Replace(this.delimiter, ConfigurationPath.KeyDelimiter);

    private void HandleMatchedConnectionStringPrefix(Dictionary<string, string?> data, string connectionStringPrefix, string? provider, string fullKey, string? value)
    {
        string normalizedKeyWithoutConnectionStringPrefix = this.Normalize(fullKey.Substring(connectionStringPrefix.Length));

        // Add the key-value pair for connection string, and optionally provider name
        this.AddIfNormalizedKeyMatchesPrefix(data, $"ConnectionStrings:{normalizedKeyWithoutConnectionStringPrefix}", value);
        if (provider != null)
        {
            this.AddIfNormalizedKeyMatchesPrefix(data, $"ConnectionStrings:{normalizedKeyWithoutConnectionStringPrefix}_ProviderName", provider);
        }
    }

    private void AddIfNormalizedKeyMatchesPrefix(Dictionary<string, string?> data, string normalizedKey, string? value)
    {
        if (normalizedKey.StartsWith(this.normalizedPrefix, StringComparison.OrdinalIgnoreCase))
        {
            data[normalizedKey.Substring(this.normalizedPrefix.Length)] = value;
        }
    }
}