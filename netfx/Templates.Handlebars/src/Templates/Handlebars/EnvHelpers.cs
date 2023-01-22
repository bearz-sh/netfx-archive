using Bearz.Extra.Strings;
using Bearz.Std;

using HandlebarsDotNet;

namespace Bearz.Templates.Handlebars;

public static class EnvHelpers
{
    public static void GetEnvVariable(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "ev-get");

        var key = arguments[0].ToString();
        if (key.IsNullOrWhiteSpace())
            throw new HandlebarsException("ev-get name must not be null or whitespace");

        var defaultValue = arguments.Length > 1 ? arguments[1].ToString() : string.Empty;
        if (Env.TryGet(NormalizeEnvKey(key), out var value))
        {
            writer.WriteSafeString(value);
            return;
        }

        writer.WriteSafeString(defaultValue);
    }

    public static object GetEnvVariableAsBool(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "ev-get-bool");

        var key = arguments[0].ToString();
        if (key.IsNullOrWhiteSpace())
            throw new HandlebarsException("ev-get-bool name must not be null or whitespace");

        object? defaultValue = false;
        if (arguments.Length > 0)
            defaultValue = arguments[0];

        if (Env.TryGet(NormalizeEnvKey(key), out var value))
        {
            return value.AsBool();
        }

        return defaultValue.AsBool();
    }

    public static void ExpandEnvVar(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "ev-expand");

        var template = arguments[0].ToString();
        if (template.IsNullOrWhiteSpace())
            throw new HandlebarsException("ev-expand template must not be null or whitespace");

        writer.WriteSafeString(Env.Expand(template));
    }

    public static void RegisterEnvHelpers(this IHandlebars? hb)
    {
        if (hb is null)
        {
            HandlebarsDotNet.Handlebars.RegisterHelper("ev-get", GetEnvVariable);
            HandlebarsDotNet.Handlebars.RegisterHelper("ev-get-bool", GetEnvVariableAsBool);
            HandlebarsDotNet.Handlebars.RegisterHelper("ev-expand", ExpandEnvVar);
            return;
        }

        hb.RegisterHelper("ev-get", GetEnvVariable);
        hb.RegisterHelper("ev-get-bool", GetEnvVariableAsBool);
        hb.RegisterHelper("ev-expand", ExpandEnvVar);
    }

    private static string NormalizeEnvKey(string key)
    {
        var sb = Bearz.Text.StringBuilderCache.Acquire();
        foreach (var c in key)
        {
            if (char.IsLetterOrDigit(c))
            {
                sb.Append(char.ToUpperInvariant(c));
            }
            else if (c is '(' or ')' && Std.Env.IsWindows())
            {
                sb.Append(c);
            }
            else if (c is '-' or '.' or '_' or '/' or ':')
            {
                sb.Append('_');
            }
            else
            {
                throw new InvalidOperationException($"Invalid character '{c}' in configuration key '{key}'");
            }
        }

        return Text.StringBuilderCache.GetStringAndRelease(sb);
    }
}