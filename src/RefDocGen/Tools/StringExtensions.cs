namespace RefDocGen.Tools;

/// <summary>
/// Class containing extension methods for <see cref="string"/> class.
/// </summary>
internal static class StringExtensions
{
    /// <summary>
    /// Tries to find the first occurence of a given character in the given string.
    /// </summary>
    /// <param name="s">The string to search.</param>
    /// <param name="value">The character to seek.</param>
    /// <param name="index">Zero-based index of the first occurence of <paramref name="value"/>; -1 if not found.</param>
    /// <returns>True, if the character is found, false otherwise.</returns>
    internal static bool TryGetIndex(this string s, char value, out int index)
    {
        if (s.Contains(value))
        {
            index = s.IndexOf(value);
            return true;
        }
        else
        {
            index = -1;
            return false;
        }
    }
}
