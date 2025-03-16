namespace RefDocGen.TemplateGenerators.Shared.Tools.Keywords;

/// <summary>
/// Class containing extension methods for <see cref="List{T}"/> class.
/// </summary>
internal static class ListExtensions
{
    /// <summary>
    /// Convert each of the keywords present in the list into its string representation.
    /// </summary>
    /// <param name="keywords">The provided list of keywords.</param>
    /// <returns>An array of keywords converted into string representation.</returns>
    internal static string[] GetStrings(this List<Keyword> keywords)
    {
        return [.. keywords.Select(k => k.GetString())];
    }
}
