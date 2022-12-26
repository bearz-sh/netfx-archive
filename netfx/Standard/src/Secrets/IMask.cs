using System;
using System.Diagnostics.CodeAnalysis;

namespace Bearz.Secrets;

public interface IMask
{
    string? Mask([NotNullIfNotNull("value")] string? value);

    ReadOnlySpan<char> Mask(ReadOnlySpan<char> value);
}