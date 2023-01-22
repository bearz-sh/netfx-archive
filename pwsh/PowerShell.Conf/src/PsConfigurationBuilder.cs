using System;
using System.Collections.Generic;
using System.Linq;

using Bearz.Extra.Strings;
using Bearz.Std;
using Bearz.Text.DotEnv;

using Microsoft.Extensions.Configuration;

namespace Ze.PowerShell.Conf;

public static class PsConfigurationBuilder
{
    public static ConfigurationBuilder Configure(
        ConfigurationBuilder builder,
        string? basePath = null,
        string[]? files = null,
        bool includeEnvironmentVariables = true,
        bool reloadOnChange = false,
        string? environmentPrefix = null)
    {
        basePath ??= System.Environment.CurrentDirectory;
        builder.SetBasePath(basePath);

        if (includeEnvironmentVariables)
        {
            var hasPrefix = !string.IsNullOrWhiteSpace(environmentPrefix);
            var env = Environment.GetEnvironmentVariables();
            var data = new Dictionary<string, string?>();
            foreach (var key in env.Keys)
            {
                if (key is string name)
                {
                    if (name.Contains("("))
                        continue;

                    if (hasPrefix && !name.StartsWithIgnoreCase(environmentPrefix!))
                        continue;

                    data[name.Replace("_", ":")] = env[key]?.ToString();
                }
            }

            builder.AddInMemoryCollection(data);
        }

        if (files != null)
        {
            foreach (var config in files)
            {
                var ext = System.IO.Path.GetExtension(config);
                switch (ext)
                {
                    case ".json":
                    case ".jsonc":
                        builder.AddJsonFile(config, false, reloadOnChange);
                        break;
                    case ".yml":
                    case ".yaml":
                        builder.AddYamlFile(config, false, reloadOnChange);
                        break;
                    case ".ini":
                        builder.AddIniFile(config, false, reloadOnChange);
                        break;
                    case ".env":
                        {
                            using var fs = System.IO.File.OpenRead(config);
                            var values =
                                Bearz.Text.DotEnv.DotEnvFile.Parse(fs, new DotEnvSettings() { Expand = true, });

                            builder.AddInMemoryCollection(values);
                        }

                        break;

                    default:
                        throw new NotSupportedException($"Unsupported file extension '{ext}'");
                }
            }
        }

        return builder;
    }

    public static ConfigurationBuilder Create(
        string? basePath = null,
        string[]? files = null,
        bool includeEnvironmentVariables = true,
        bool reloadOnChange = false,
        string? environmentPrefix = null)
    {
        return Configure(
            new ConfigurationBuilder(),
            basePath,
            files,
            includeEnvironmentVariables,
            reloadOnChange,
            environmentPrefix);
    }
}