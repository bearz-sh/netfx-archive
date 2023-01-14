using Bearz.Text;

namespace Ze.Cli;

public static class CliFormatter
{
    public static string FormatPosixOptionName(string name)
    {
        var sb = StringBuilderCache.Acquire();
        sb.Append("--");
        for (var i = 0; i < name.Length; i++)
        {
            var c = name[i];
            if (char.IsUpper(c))
            {
                if (i > 0)
                {
                    sb.Append('-');
                }

                sb.Append(char.ToLower(c));
            }
            else
            {
                sb.Append(c);
            }
        }

        return StringBuilderCache.GetStringAndRelease(sb);
    }

    public static string FormatPowerShellOptionName(string name)
    {
        return $"-{name}";
    }
}