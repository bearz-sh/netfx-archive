using System.Text.RegularExpressions;

using Std.Collections.Generic;
using Std.OS;

using Path = System.IO.Path;

namespace Std.Text.DotEnv;

public static partial class DotEnvFile
{
    public static void Config()
    {
        Config(new DotEnvSettings());
    }

    public static void Config(DotEnvSettings settings)
    {
        var values = Parse(settings);

        if (settings.Expand)
        {
            Expand(values, settings);
        }
        else
        {
            Load(values, settings);
        }
    }

    public static void Config(Stream stream)
    {
        Config(stream, new DotEnvSettings());
    }

    public static void Config(Stream stream, DotEnvSettings settings)
    {
        Config(Parse(stream, settings), settings);
    }

    public static void Config(string data)
    {
        var settings = new DotEnvSettings();
        Config(data, settings);
    }

    public static void Config(string data, DotEnvSettings settings)
    {
        Config(Parse(data, settings), new DotEnvSettings());
    }

    public static void Config(TextReader reader)
    {
        Config(reader, new DotEnvSettings());
    }

    public static void Config(TextReader reader, DotEnvSettings settings)
    {
        Config(Parse(reader, settings), settings);
    }

    public static void Config(IReadOnlyDictionary<string, string> values)
    {
        var settings = new DotEnvSettings();
        if (settings.Expand)
        {
            Expand(values, settings);
        }
        else
        {
            Load(values, settings);
        }
    }

    public static void Config(IReadOnlyDictionary<string, string> values, DotEnvSettings settings)
    {
        if (settings.Expand)
        {
            Expand(values, settings);
        }
        else
        {
            Load(values, settings);
        }
    }

    public static IReadOnlyDictionary<string, string> Parse(DotEnvSettings settings)
    {
        IReadOnlyDictionary<string, string> values;
        if (settings.Files.Count == 0)
        {
            var path = string.IsNullOrWhiteSpace(settings.Path)
                ? Path.Combine(Environment.CurrentDirectory, ".env")
                : settings.Path;

            path = Path.GetFullPath(path);
            using var stream = File.OpenRead(path);
            values = DotEnvSerializer.Deserialize(stream, settings);
        }
        else
        {
            values = MergeFiles(settings.Files, settings);
        }

        if (settings.Expand)
        {
            values = Expand(values, settings);
        }

        return values;
    }

    public static IReadOnlyDictionary<string, string> Parse(string data)
    {
        return DotEnvSerializer.Deserialize(data);
    }

    public static IReadOnlyDictionary<string, string> Parse(string data, DotEnvSettings settings)
    {
        return DotEnvSerializer.Deserialize(data);
    }

    public static IReadOnlyDictionary<string, string> Parse(Stream stream)
    {
        return DotEnvSerializer.Deserialize(stream, new DotEnvSettings());
    }

    public static IReadOnlyDictionary<string, string> Parse(Stream stream, DotEnvSettings settings)
    {
        var values = DotEnvSerializer.Deserialize(stream, settings);
        if (settings.Expand)
        {
            values = Expand(values, settings);
        }

        return values;
    }

    public static IReadOnlyDictionary<string, string> Parse(TextReader reader)
    {
        return DotEnvSerializer.Deserialize(reader);
    }

    public static IReadOnlyDictionary<string, string> Parse(TextReader reader, DotEnvSettings settings)
    {
        return DotEnvSerializer.Deserialize(reader);
    }

    private static IReadOnlyDictionary<string, string> MergeFiles(IEnumerable<string> files, DotEnvSettings settings)
    {
        var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var file in files)
        {
            var absFile = Path.GetFullPath(file);
            using var stream = File.OpenRead(absFile);
            var data = DotEnvSerializer.Deserialize(stream, settings);
            foreach (var kvp in data)
            {
                dictionary[kvp.Key] = kvp.Value;
            }
        }

        return dictionary;
    }

    private static void Load(IReadOnlyDictionary<string, string> values, DotEnvSettings settings)
    {
        if (settings.Override)
        {
            foreach (var kvp in values)
            {
                Environment.SetEnvironmentVariable(kvp.Key, kvp.Value);
            }

            return;
        }

        foreach (var kvp in values)
        {
            if (Environment.GetEnvironmentVariable(kvp.Key) != null)
            {
                Environment.SetEnvironmentVariable(kvp.Key, kvp.Value);
            }
        }
    }

    private static IReadOnlyDictionary<string, string> Expand(IReadOnlyDictionary<string, string> values, DotEnvSettings settings)
    {
        var result = new OrderedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        var processEnv = Env.GetAll();
        var cache = new OrderedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var kvp in processEnv)
        {
            cache.Add(kvp.Key, kvp.Value);
        }

        foreach (var kvp in values)
        {
            cache.Add(kvp.Key, kvp.Value);
        }

        string? GetValue(string key)
        {
            if (cache.TryGetValue(key, out var value)) return value;

            return null;
        }

        foreach (var kvp in values)
        {
            var value = kvp.Value;
            Env.Expand(value, false, GetValue);

            result[kvp.Key] = value;
        }

        return result;
    }
}