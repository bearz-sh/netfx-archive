using System.Diagnostics.CodeAnalysis;
using System.Text;

using Bearz.Text;

using Directory = System.IO.Directory;
using File = System.IO.File;

namespace Bearz.Std;

public static class Fs
{
    public static byte[] ReadFile(string path)
        => File.ReadAllBytes(path);

#if NET6_0_OR_GREATER
    public static Task<byte[]> ReadFileAsync(string path, CancellationToken cancellationToken = default)
        => File.ReadAllBytesAsync(path, cancellationToken);
#endif

    public static string ReadTextFile(string path, Encoding? encoding = null)
        => File.ReadAllText(path, encoding ?? Encodings.Utf8NoBom);

#if NET6_0_OR_GREATER
    public static Task<string> ReadTextFileAsync(string path, Encoding? encoding = null, CancellationToken cancellationToken = default)
        => File.ReadAllTextAsync(path, encoding ?? Encodings.Utf8NoBom, cancellationToken);
#endif

    public static string CatFiles(bool throwIfNotFound, params string[] files)
    {
        var sb = StringBuilderCache.Acquire();
        foreach (var file in files)
        {
            if (throwIfNotFound && !File.Exists(file))
                throw new FileNotFoundException($"File not found: {file}");

            if (sb.Length > 0)
                sb.Append('\n');

            sb.Append(ReadTextFile(file));
        }

        return StringBuilderCache.GetStringAndRelease(sb);
    }

    public static bool FileExists([NotNullWhen(true)] string? path)
        => File.Exists(path);

    public static bool DirectoryExits([NotNullWhen(true)] string? path)
        => Directory.Exists(path);

    public static void WriteFile(string path, byte[] data)
        => File.WriteAllBytes(path, data);

    public static FileStream Open(string path)
        => File.Open(path, FileMode.OpenOrCreate);

    public static FileStream Open(string path, FileMode mode)
        => File.Open(path, mode);

    public static FileStream Open(string path, FileMode mode, FileAccess access)
        => File.Open(path, mode, access);

    public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
        => File.Open(path, mode, access, share);

#if NET6_0_OR_GREATER
    public static FileStream Open(string path, FileStreamOptions options)
        => File.Open(path, options);
#endif

    public static void CopyFile(string source, string destination, bool overwrite = false)
        => File.Copy(source, destination, overwrite);

    public static void MoveFile(string source, string destination)
        => File.Move(source, destination);

    public static IEnumerable<string> ReadDirectory(string path)
        => Directory.EnumerateFileSystemEntries(path);

    public static IEnumerable<string> ReadDirectory(string path, string searchPattern)
        => Directory.EnumerateFileSystemEntries(path, searchPattern);

    public static IEnumerable<string> ReadDirectory(string path, string searchPattern, SearchOption searchOption)
        => Directory.EnumerateFileSystemEntries(path, searchPattern, searchOption);

#if NET6_0_OR_GREATER
    public static IEnumerable<string> ReadDirectory(string path, string searchPattern, EnumerationOptions enumerationOptions)
        => Directory.EnumerateFileSystemEntries(path, searchPattern, enumerationOptions);
#endif

#if NET5_0_OR_GREATER
    public static FileSystemInfo Symlink(string path, string target)
        => File.CreateSymbolicLink(path, target);
#endif

    public static void WriteTextFile(string path, string? contents, Encoding? encoding = null, bool append = false)
    {
        if (append)
            File.AppendAllText(path, contents, encoding ?? Encodings.Utf8NoBom);
        else
            File.WriteAllText(path, contents, encoding ?? Encodings.Utf8NoBom);
    }

    public static void MakeDirectory(string path)
        => Directory.CreateDirectory(path);

#if NET7_0_OR_GREATER
    public static void MakeDirectory(string path, UnixFileMode mode)
        => Directory.CreateDirectory(path, mode);
#endif

    public static void RemoveFile(string path)
        => File.Delete(path);

    public static void RemoveDirectory(string path, bool recursive = false)
        => Directory.Delete(path, recursive);
}