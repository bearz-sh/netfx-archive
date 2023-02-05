using Bearz.Extra.Strings;

using HandlebarsDotNet;

using Microsoft.Extensions.Configuration;

namespace Bearz.Templates.Handlebars;

public static class ConfHelpers
{
    public static void GetConfValue(IConfiguration config, EncodedTextWriter writer, Context context, Arguments arguments)
    {
        if (arguments.Length == 0) throw new InvalidOperationException("conf helper requires at least one argument");

        var key = arguments[0].ToString();
        if (key.IsNullOrWhiteSpace())
            throw new InvalidOperationException("key must not be null or whitespace");
        var defaultValue = arguments.Length > 1 ? arguments[1].ToString() : string.Empty;
        var section = config.GetSection(NormalizeConfigKey(key));
        if (section.Value == null)
        {
            writer.WriteSafeString(defaultValue);
            return;
        }

        writer.WriteSafeString(section.Value);
    }

    public static object GetConfValueAsBool(IConfiguration configuration, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "conf-bool");
        var key = arguments.GetString(0, string.Empty);
        if (key.IsNullOrWhiteSpace())
            throw new HandlebarsException("key must not be null or whitespace");
        var section = configuration.GetSection(NormalizeConfigKey(key));

        if (section.Value is null)
            return false;

        return section.Value.AsBool(false);
    }

    [CLSCompliant(false)]
    public static void RegisterConfHelpers(this IHandlebars? hb, IConfiguration configuration)
    {
        if (hb is null)
        {
            HandlebarsDotNet.Handlebars.RegisterHelper("conf-value", (w, c, a) => GetConfValue(configuration, w, c, a));
            HandlebarsDotNet.Handlebars.RegisterHelper("conf-bool", (w, c, a) => GetConfValueAsBool(configuration, c, a));
            return;
        }

        hb.RegisterHelper("conf-get", (w, c, a) => GetConfValue(configuration, w, c, a));
        hb.RegisterHelper("conf-get-bool", (w, c, a) => GetConfValueAsBool(configuration, c, a));
    }

    private static string NormalizeConfigKey(string key)
    {
        var sb = Bearz.Text.StringBuilderCache.Acquire();
        foreach (var c in key)
        {
            if (char.IsLetterOrDigit(c))
            {
                sb.Append(c);
            }
            else if (c is '(' or ')' && Std.Env.IsWindows())
            {
                sb.Append(c);
            }
            else if (c is '-' or '.' or '_' or '/' or ':')
            {
                sb.Append(':');
            }
            else
            {
                throw new InvalidOperationException($"Invalid character '{c}' in configuration key '{key}'");
            }
        }

        return Text.StringBuilderCache.GetStringAndRelease(sb);
    }
}