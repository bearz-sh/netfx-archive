using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

using P = System.IO.Path;

namespace Std.OS;

public static partial class Path
{
    public static char PathSeparator => P.PathSeparator;

    public static char AltDirectorySeparator => P.AltDirectorySeparatorChar;

    public static char DirectorySeparator => P.DirectorySeparatorChar;

    public static char VolumeSeparator => P.VolumeSeparatorChar;

#if NET7_0_OR_GREATER
    public static bool Exists([NotNullWhen(true)] string? path)
    {
        return P.Exists(path);
    }
#endif

#if  !NETLEGACY

    public static ReadOnlySpan<char> Basename(ReadOnlySpan<char> path)
    {
        return P.GetFileName(path);
    }

    public static ReadOnlySpan<char> Dirname(ReadOnlySpan<char> path)
    {
        return P.GetDirectoryName(path);
    }

    public static ReadOnlySpan<char> Extension(ReadOnlySpan<char> path)
    {
        return P.GetExtension(path);
    }

    public static string Join(string path1, string path2)
    {
        return P.Join(path1, path2);
    }

    public static string Join(string path1, string path2, string path3)
    {
        return P.Join(path1, path2, path3);
    }

    public static string Join(string path1, string path2, string path3, string path4)
    {
        return P.Join(path1, path2, path3, path4);
    }

    public static string Join(params string[] paths)
    {
        return P.Join(paths);
    }

    public static bool IsPathFullyQualified(string path)
    {
        return P.IsPathFullyQualified(path);
    }

#endif
}