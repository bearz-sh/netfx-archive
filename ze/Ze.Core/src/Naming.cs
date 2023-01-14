using Bearz.Text;

namespace Ze;

public static class Naming
{
    public static string ToEnvName(this string name)
        => ToEnvName(name.AsSpan());

    public static string ToEnvName(this ReadOnlySpan<char> name)
    {
        var sb = StringBuilderCache.Acquire();
        char l = char.MinValue;
        foreach (var c in name)
        {
            if (char.IsLetterOrDigit(c))
            {
                if (char.IsUpper(c) && !char.IsUpper(l))
                {
                    sb.Append('-');
                }

                l = c;
                sb.Append(char.ToLowerInvariant(c));
                continue;
            }

            if (c is '_' or '-' or '/' or ':' or '.' or ' ')
            {
                sb.Append('-');
            }

            l = c;
        }

        return StringBuilderCache.GetStringAndRelease(sb);
    }

    public static string ToSettingName(this string name)
        => ToEnvName(name.AsSpan());

    public static string ToSettingName(this ReadOnlySpan<char> name)
    {
        var sb = StringBuilderCache.Acquire();

        foreach (var c in name)
        {
            if (c is '.' or '-')
            {
                sb.Append(c);
                continue;
            }

            if (char.IsLetterOrDigit(c))
            {
                sb.Append(char.ToLowerInvariant(c));
                continue;
            }

            if (c is '_' or '/' or ':' or ' ')
            {
                sb.Append('.');
            }
        }

        return StringBuilderCache.GetStringAndRelease(sb);
    }

    public static string ToEnvVarName(this ReadOnlySpan<char> name)
    {
        var sb = StringBuilderCache.Acquire();
        foreach (var c in name)
        {
            if (char.IsLetterOrDigit(c))
            {
                sb.Append(char.ToUpperInvariant(c));
                continue;
            }

            if (c is '_' or '-' or '/' or ':' or '.' or ' ')
            {
                sb.Append('_');
            }
        }

        return StringBuilderCache.GetStringAndRelease(sb);
    }
}