using Microsoft.Win32.SafeHandles;

#if NET7_0_OR_GREATER

namespace Ze.Virtual.FileSystem;

public partial interface IFileSystem
{
    UnixFileMode GetUnixFileMode(Microsoft.Win32.SafeHandles.SafeFileHandle fileHandle);

    void MakeDirectory(string path, UnixFileMode mode);

    void SetUnixFileMode(string path, UnixFileMode mode);

    void SetUnixFileMode(SafeFileHandle fileHandle, UnixFileMode mode);
}

#endif