using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Bearz.Extra.Strings;
using Bearz.Secrets;
using Bearz.Std;
using Bearz.Text;

namespace Ze.PowerShell.Core;

public static class Utils
{
    private static Dictionary<string, Hashtable> moduleConfigCache = new(StringComparer.OrdinalIgnoreCase);

    public static Hashtable? GetModuleConfig(string moduleName)
    {
        if (moduleConfigCache.TryGetValue(moduleName, out var config))
        {
            return config;
        }

#pragma warning disable S1168
        return null;
#pragma warning restore S1168
    }

    public static Hashtable SetModuleConfig(string moduleName, Hashtable config)
    {
        moduleConfigCache[moduleName] = config;
        return config;
    }

    public static void WriteCommand(string command, CommandArgs args)
    {
        var message = $"{command} {args}";
        message = SecretMasker.Default.Mask(message);

        if (Env.Get("TF_BUILD")?.EqualsIgnoreCase("true") == true)
        {
            Console.WriteLine($"##[command]{message}");
        }
        else if (Env.Get("GITHUB_ACTIONS")?.EqualsIgnoreCase("true") == true)
        {
            Console.WriteLine($"::notice::{message}");
        }
        else
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }
    }

    public static Encoding GetEncoding(string? encodingName)
    {
        if (string.IsNullOrEmpty(encodingName))
        {
            return Encoding.Default;
        }

        if (encodingName.EqualsIgnoreCase("utf8nobom"))
        {
            return Encodings.Utf8NoBom;
        }

        if (encodingName.EqualsIgnoreCase("utf8bom"))
        {
            return Encodings.Utf8;
        }

        if (encodingName.EqualsIgnoreCase("bigendianutf32"))
        {
            return Encodings.BigEndianUnicode;
        }

        try
        {
            return Encoding.GetEncoding(encodingName);
        }
        catch (ArgumentException)
        {
            return Encoding.Default;
        }
    }
}