namespace RefDocGen.Tools;

/// <summary>
/// Class containing extension methods for <see cref="IEnumerable{T}"/> interface.
/// </summary>
internal static class IEnumerableExtensions
{
    /// <summary>
    /// Filters not-null values from the enumerable.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable item. Note that <typeparamref name="T"/> must be a reference type.</typeparam>
    /// <param name="values">The provided enumerable.</param>
    /// <returns>not-null values from the enumerable.</returns>
    internal static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> values)
        where T : class
    {
        foreach (var item in values)
        {
            if (item is not null)
            {
                yield return item;
            }
        }
    }
}
