namespace RefDocGen.TemplateProcessors.Shared.TemplateTools;

/// <summary>
/// Class with extension methods for <see cref="IEnumerable{T}"/> class.
/// </summary>
public static class IEnumerableExtensions
{
    /// <summary>
    /// Enumerates the sequence and returns tuples of (index, item).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The provided sequence.</param>
    /// <returns>Tuples containing (index, item).</returns>
    public static IEnumerable<(int index, T item)> Enumerate<T>(this IEnumerable<T> source)
    {
        int i = 0;
        foreach (var item in source)
        {
            yield return (i, item);
            i++;
        }
    }
}
