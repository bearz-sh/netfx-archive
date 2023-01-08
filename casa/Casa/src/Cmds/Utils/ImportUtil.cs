using System.Text.Json;

using Bearz.Text.DotEnv;

using Newtonsoft.Json.Linq;

namespace Casa.Cmds.Utils;

public static class ImportUtil
{
    public static IReadOnlyDictionary<string, string> Import(string filePath)
    {
        var ext = Path.GetExtension(filePath);
        switch (ext)
        {
            case ".json":
            case ".jsonc":
                return ImportJson(filePath);
            case ".env":
                return ImportEnv(filePath);
            default:
                throw new NotSupportedException($"File extension {ext} is not supported for importing secrets or environment variables.");
        }
    }

    private static IReadOnlyDictionary<string, string> ImportEnv(string filePath)
    {
        using var reader = new StreamReader(filePath);
        return Bearz.Text.DotEnv.DotEnvFile.Parse(reader, new DotEnvSettings()
        {
            Expand = true,
        });
    }

    private static Dictionary<string, string> ImportJson(string filePath)
    {
        var result = new Dictionary<string, string>();
        var json = File.ReadAllText(filePath);
        var root = JsonSerializer.Deserialize<Dictionary<string, object?>>(json, new JsonSerializerOptions()
        {
            MaxDepth = 20,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            PropertyNameCaseInsensitive = true,
        });
        if (root is null)
            return result;

        ParseChild(root, result);

        return result;
    }

    private static void ParseChild(Dictionary<string, object?> source, Dictionary<string, string> dest, string? baseKey = null)
    {
        foreach (var item in source)
        {
            var value = item.Value;
            if (value is null)
                continue;

            var key = (baseKey is null ? item.Key : $"{baseKey}_{item.Key}").ToUpper();

            if (item.Value is Dictionary<string, object?> child)
            {
                ParseChild(child, dest, key);
            }
            else if (item.Value is Array _)
            {
                throw new NotImplementedException("Array not supported");
            }
            else
            {
                var strValue = value.ToString();
                if (strValue is null)
                    continue;

                dest[key] = strValue;
            }
        }
    }
}