using System.Collections;

using Bearz.Extra.Strings;
using Bearz.Text;

using HandlebarsDotNet;

using Humanizer;

namespace Bearz.Templates.Handlebars;

public static class StringHelpers
{
    public static void EncodeUri(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "encode-uri");
        var value = arguments.GetString(1, string.Empty);
        writer.Write(System.Net.WebUtility.UrlEncode(value));
    }

    public static void DecodeUri(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "encode-uri");
        var value = arguments.GetString(1, string.Empty);
        writer.Write(System.Net.WebUtility.UrlDecode(value));
    }

    public static void Titleize(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "titleize");
        var value = arguments.GetString(0);
        writer.Write(value.Titleize());
    }

    public static void Camelize(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "camelize");
        var value = arguments.GetString(0);
        writer.Write(value.Camelize());
    }

    public static void Dasherize(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "camelize");
        var value = arguments.GetString(0);
        writer.Write(value.Dasherize());
    }

    public static void Kebaberize(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "kebaberize");
        var value = arguments.GetString(0);
        writer.Write(value.Kebaberize());
    }

    public static void Underscore(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "underscore");
        var value = arguments.GetString(0);
        writer.Write(value.Underscore());
    }

    public static void Humanize(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "humanize");
        var value = arguments.GetString(0);
        writer.Write(value.Humanize());
    }

    public static void Truncate(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "truncate");
        var value = arguments.GetString(0);
        var length = arguments.GetInt32(1);
        writer.Write(value.Truncate(length));
    }

    public static void TruncateWithEllipsis(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "truncate");
        var value = arguments.GetString(0);
        var length = arguments.GetInt32(1);
        writer.Write(value.Truncate(length, "..."));
    }

    public static void Pluralize(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "pluralize");
        var value = arguments.GetString(0);
        writer.Write(value.Pluralize());
    }

    public static void Singularize(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "singularize");
        var value = arguments.GetString(0);
        writer.Write(value.Singularize());
    }

    public static void Ordinalize(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "ordinalize");
        var value = arguments.GetString(0);
        writer.Write(value.Ordinalize());
    }

    public static void Capitalize(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "upperFirst");
        var value = arguments.GetString(0);
        writer.Write(value[0].ToString().ToUpper());
        writer.Write(value.Substring(1));
    }

    public static void ToLower(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "lower");
        var value = arguments.GetString(0);
        writer.Write(value.ToLower());
    }

    public static void ToUpper(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "upper");
        var value = arguments.GetString(0);
        writer.Write(value.ToUpper());
    }

    public static void Split(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "split");
        var value = arguments.GetString(0);
        var separator = arguments.GetString(1);
        writer.Write(value.Split(separator.ToCharArray()));
    }

    public static void Join(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "join");
        var value = arguments[0];
        if (value is not IEnumerable enumerable)
        {
            throw new HandlebarsException($"{{join}} helper must be called with an enumerable value");
        }

        var separator = arguments.GetString(1);
        writer.Write(string.Join(separator, enumerable.Cast<object>()));
    }

    public static bool IsString(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "is-string");
        var value = arguments[0];
        return value is string;
    }

    public static bool IsNullOrEmpty(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "is-empty");
        var value = arguments[0];
        if (value is null)
            return true;

        if (value is string str)
            return str.Length == 0;

        if (value is IEnumerable enumerable)
        {
            foreach (var n in enumerable)
#pragma warning disable S1751
                return false;
#pragma warning restore S1751

            return true;
        }

        return false;
    }

    public static bool IsNull(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "is-null");
        var value = arguments[0];
        return value is null;
    }

    public static bool IsNullOrWhiteSpace(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "is-whitespace");
        var value = arguments[0];
        if (value is null)
            return true;

        if (value is string str)
            return str.IsNullOrWhiteSpace();

        if (value is IEnumerable enumerable)
        {
            foreach (var n in enumerable)
#pragma warning disable S1751
                return false;
#pragma warning restore S1751

            return true;
        }

        return false;
    }

    public static void Concat(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "string-concat");
        var sb = StringBuilderCache.Acquire();
        foreach (var arg in arguments)
        {
            sb.Append(arg);
        }

        writer.Write(StringBuilderCache.GetStringAndRelease(sb));
    }

    public static void Base64Encode(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "base64-encode");
        var value = arguments.GetString(0);
        writer.Write(Convert.ToBase64String(Encodings.Utf8.GetBytes(value)));
    }

    public static void Base64Decode(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "base64-decode");
        var value = arguments.GetString(0);
        writer.Write(Encodings.Utf8.GetString(Convert.FromBase64String(value)));
    }

    public static void PadLeft(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "pad-left");
        var value = arguments.GetString(0);
        var length = arguments.GetInt32(1);
        string padding = " ";
        if (arguments.Length > 0)
            padding = arguments.GetString(0, " ");

        writer.Write(value.PadLeft(length, padding[0]));
    }

    public static void PadRight(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "pad-right");
        var value = arguments.GetString(0);
        var length = arguments.GetInt32(1);
        string padding = " ";
        if (arguments.Length > 0)
            padding = arguments.GetString(0, " ");

        writer.Write(value.PadRight(length, padding[0]));
    }

    public static void Replace(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(3, "string-replace");
        var value = arguments.GetString(0);
        var oldValue = arguments.GetString(1);
        var newValue = arguments.GetString(2);
        writer.Write(value.Replace(oldValue, newValue));
    }

    public static void Remove(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "string-remove");
        var value = arguments.GetString(0);
        var startIndex = arguments.GetInt32(1);
        var length = arguments.GetInt32(2, value.Length - startIndex);
        writer.Write(value.Remove(startIndex, length));
    }

    public static void Prepend(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "prepend");
        var value = arguments.GetString(0);
        var prefix = arguments.GetString(1);
        writer.Write(prefix + value);
    }

    public static void Append(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "append");
        var value = arguments.GetString(0);
        var suffix = arguments.GetString(1);
        writer.Write(value + suffix);
    }

    public static void Trim(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "trim");
        var value = arguments.GetString(0);
        if (arguments.Length > 1)
        {
            var chars = arguments.GetString(0, " ").ToCharArray();
            writer.Write(value.Trim(chars));
            return;
        }

        writer.Write(value.Trim());
    }

    public static void TrimStart(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "trim-start");
        var value = arguments.GetString(0);

        if (arguments.Length > 1)
        {
            var chars = arguments.GetString(0, " ").ToCharArray();
            writer.Write(value.TrimStart(chars));
            return;
        }

        writer.Write(value.TrimStart());
    }

    public static void TrimEnd(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "trim-end");
        var value = arguments.GetString(0);

        if (arguments.Length > 1)
        {
            var chars = arguments.GetString(0, " ").ToCharArray();
            writer.Write(value.TrimEnd(chars));
            return;
        }

        writer.Write(value.TrimEnd());
    }

    public static void Format(IFormatProvider? provider, EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "format");
        var value = arguments[0];
        var format = arguments.GetString(1);
        var formatProvider = provider;

        // Attempt using a custom formatter from the format provider (if any)
        var customFormatter = formatProvider?.GetFormat(typeof(ICustomFormatter)) as ICustomFormatter;
        var formattedValue = customFormatter?.Format(format, value, formatProvider);

        // Fallback to IFormattable
        formattedValue ??= (value as IFormattable)?.ToString(format, formatProvider);

        // Fallback to ToString
        formattedValue ??= value?.ToString();

        // Done
        writer.WriteSafeString(formattedValue ?? string.Empty);
    }

    public static bool StartsWith(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "starts-with");
        var value = arguments.GetString(0);
        var prefix = arguments.GetString(1);
        return value.StartsWith(prefix);
    }

    public static bool EndsWith(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "ends-with");
        var value = arguments.GetString(0);
        var suffix = arguments.GetString(1);
        return value.EndsWith(suffix);
    }

    public static void RegisterStringHelpers(this IHandlebars? hb)
    {
        if (hb is null)
        {
            HandlebarsDotNet.Handlebars.RegisterHelper("titleize", Titleize);
            HandlebarsDotNet.Handlebars.RegisterHelper("camelize", Camelize);
            HandlebarsDotNet.Handlebars.RegisterHelper("dasherize", Dasherize);
            HandlebarsDotNet.Handlebars.RegisterHelper("kebaberize", Kebaberize);
            HandlebarsDotNet.Handlebars.RegisterHelper("underscore", Underscore);
            HandlebarsDotNet.Handlebars.RegisterHelper("humanize", Humanize);
            HandlebarsDotNet.Handlebars.RegisterHelper("truncate", Truncate);
            HandlebarsDotNet.Handlebars.RegisterHelper("truncateWithEllipsis", TruncateWithEllipsis);
            HandlebarsDotNet.Handlebars.RegisterHelper("pluralize", Pluralize);
            HandlebarsDotNet.Handlebars.RegisterHelper("singularize", Singularize);
            HandlebarsDotNet.Handlebars.RegisterHelper("ordinalize", Ordinalize);
            HandlebarsDotNet.Handlebars.RegisterHelper("capitalize", Capitalize);
            HandlebarsDotNet.Handlebars.RegisterHelper("lower", ToLower);
            HandlebarsDotNet.Handlebars.RegisterHelper("upper", ToUpper);
            HandlebarsDotNet.Handlebars.RegisterHelper("join", Join);
            HandlebarsDotNet.Handlebars.RegisterHelper("is-string", (c, a) => IsString(c, a));
            HandlebarsDotNet.Handlebars.RegisterHelper("is-empty", (c, a) => IsNullOrEmpty(c, a));
            HandlebarsDotNet.Handlebars.RegisterHelper("is-null", (c, a) => IsNull(c, a));
            HandlebarsDotNet.Handlebars.RegisterHelper("is-whitespace", (c, a) => IsNullOrWhiteSpace(c, a));
            HandlebarsDotNet.Handlebars.RegisterHelper("string-concat", Concat);
            HandlebarsDotNet.Handlebars.RegisterHelper("base64-encode", Base64Encode);
            HandlebarsDotNet.Handlebars.RegisterHelper("base64-decode", Base64Decode);
            HandlebarsDotNet.Handlebars.RegisterHelper("pad-left", PadLeft);
            HandlebarsDotNet.Handlebars.RegisterHelper("pad-right", PadRight);
            HandlebarsDotNet.Handlebars.RegisterHelper("string-replace", Replace);
            HandlebarsDotNet.Handlebars.RegisterHelper("string-remove", Remove);
            HandlebarsDotNet.Handlebars.RegisterHelper("prepend", Prepend);
            HandlebarsDotNet.Handlebars.RegisterHelper("append", Append);
            HandlebarsDotNet.Handlebars.RegisterHelper("trim", Trim);
            HandlebarsDotNet.Handlebars.RegisterHelper("trim-start", TrimStart);
            HandlebarsDotNet.Handlebars.RegisterHelper("trim-end", TrimEnd);
            HandlebarsDotNet.Handlebars.RegisterHelper("format", (w, c, a) => Format(HandlebarsDotNet.Handlebars.Configuration.FormatProvider, w, c, a));
            HandlebarsDotNet.Handlebars.RegisterHelper("starts-with", (c, a) => StartsWith(c, a));
            HandlebarsDotNet.Handlebars.RegisterHelper("ends-with", (c, a) => EndsWith(c, a));

            return;
        }

        hb.RegisterHelper("titleize", Titleize);
        hb.RegisterHelper("camelize", Camelize);
        hb.RegisterHelper("dasherize", Dasherize);
        hb.RegisterHelper("kebaberize", Kebaberize);
        hb.RegisterHelper("underscore", Underscore);
        hb.RegisterHelper("humanize", Humanize);
        hb.RegisterHelper("truncate", Truncate);
        hb.RegisterHelper("truncateWithEllipsis", TruncateWithEllipsis);
        hb.RegisterHelper("pluralize", Pluralize);
        hb.RegisterHelper("singularize", Singularize);
        hb.RegisterHelper("ordinalize", Ordinalize);
        hb.RegisterHelper("capitalize", Capitalize);
        hb.RegisterHelper("lower", ToLower);
        hb.RegisterHelper("upper", ToUpper);
        hb.RegisterHelper("join", Join);
        hb.RegisterHelper("is-string", (c, a) => IsString(c, a));
        hb.RegisterHelper("is-empty", (c, a) => IsNullOrEmpty(c, a));
        hb.RegisterHelper("is-null", (c, a) => IsNull(c, a));
        hb.RegisterHelper("is-whitespace", (c, a) => IsNullOrWhiteSpace(c, a));
        hb.RegisterHelper("string-concat", Concat);
        hb.RegisterHelper("base64-encode", Base64Encode);
        hb.RegisterHelper("base64-decode", Base64Decode);
        hb.RegisterHelper("pad-left", PadLeft);
        hb.RegisterHelper("pad-right", PadRight);
        hb.RegisterHelper("string-replace", Replace);
        hb.RegisterHelper("string-remove", Remove);
        hb.RegisterHelper("prepend", Prepend);
        hb.RegisterHelper("append", Append);
        hb.RegisterHelper("trim", Trim);
        hb.RegisterHelper("trim-start", TrimStart);
        hb.RegisterHelper("trim-end", TrimEnd);
        hb.RegisterHelper("format", (w, c, a) => Format(hb.Configuration.FormatProvider, w, c, a));
        hb.RegisterHelper("starts-with", (c, a) => StartsWith(c, a));
        hb.RegisterHelper("ends-with", (c, a) => EndsWith(c, a));
    }
}