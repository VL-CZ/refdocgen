namespace RefDocGen.Tools;

/// <summary>
/// Class containing extension methods for <see cref="string"/>[] class.
/// </summary>
internal static class StringArrayExtensions
{
    /// <summary>
    /// Removes common leading whitespace from all non-empty lines in the given array of lines.
    /// </summary>
    /// <param name="lines">An array of strings representing lines of text.</param>
    /// <returns>A new array of strings with common leading whitespace removed from each line.</returns>
    internal static string[] Dedent(this string[] lines)
    {
        // Find minimum indentation (ignore empty lines)
        int minIndent = lines
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => l.TakeWhile(char.IsWhiteSpace).Count()) // Get number of leading whitespace chars
            .DefaultIfEmpty(0)
            .Min();

        // Remove that many spaces from the start of each line
        // Empty lines are kept as they are
        return [.. lines.Select(l => string.IsNullOrWhiteSpace(l) ? l : l[minIndent..])];
    }
}
