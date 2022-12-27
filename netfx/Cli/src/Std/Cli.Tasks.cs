using Bearz.Text;

namespace Bearz.Std;

public static partial class Cli
{
    public static string Cat(params string[] paths)
    {
        return Cat(false, paths);
    }

    public static string Cat(bool throwIfNotFound, params string[] paths)
    {
        var sb = StringBuilderCache.Acquire();
        foreach (var path in paths)
        {
            if (!Fs.FileExists(path))
            {
                if (throwIfNotFound)
                    throw new FileNotFoundException($"File not found: {path}");
                continue;
            }

            if (sb.Length > 0)
                sb.Append('\n');

            sb.Append(Fs.ReadTextFile(path));
        }

        return StringBuilderCache.GetStringAndRelease(sb);
    }

    public static void Tee(string path, string content, bool append)
    {
        Fs.WriteTextFile(path, content, null, append);
        Echo(content);
    }
}