using System.Runtime.CompilerServices;

namespace Bearz.Extra.Arrays;

public static class ArrayExtensions
{
    /// <summary>
    /// Clears the values of the array.
    /// </summary>
    /// <param name="array">The array to perform the clear operation against.</param>
    /// <param name="index">The start index. Defaults to 0.</param>
    /// <param name="length">The number of items to clear.</param>
    /// <typeparam name="T">The item type in the array.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear<T>(this T[] array, int index = 0, int? length = null)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));

        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index));

        length ??= array.Length;

        System.Array.Clear(array, index, length.Value);
    }
}