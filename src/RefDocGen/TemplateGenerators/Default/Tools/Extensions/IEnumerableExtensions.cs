namespace RefDocGen.TemplateGenerators.Default.Tools.Extensions;

/// <summary>
/// Class containing extension methods for <see cref="IEnumerable{T}"/> interface.
/// </summary>
internal static class IEnumerableExtensions
{
    /// <summary>
    /// Convert each of the keywords present in the enumerable into its string representation.
    /// </summary>
    /// <param name="keywords">The provided enumerable of keywords.</param>
    /// <returns>Enumerable of keywords converted into string representation.</returns>
    internal static IEnumerable<string> GetStrings(this IEnumerable<Keyword> keywords)
    {
        return keywords.Select(k => k.GetString());
    }
}
