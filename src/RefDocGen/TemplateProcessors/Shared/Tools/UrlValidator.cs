namespace RefDocGen.TemplateProcessors.Shared.Tools;

/// <summary>
/// Class responsible for validation of the URL paths.
/// </summary>
internal class UrlValidator
{
    /// <summary>
    /// Allowed characters on top of letters and digits.
    /// </summary>
    private static readonly char[] otherAllowedCharacters = ['-', '_', '~', '.'];

    /// <summary>
    /// Checks whether the <paramref name="pathItem"/> is a valid item in the URL path, parameters or fragment.
    /// </summary>
    /// <param name="pathItem"></param>
    /// <returns><c>true</c> if the <paramref name="pathItem"/> is a valid item in the URL path, parameters or fragment; <c>false</c> otherwise.</returns>
    internal static bool IsValidUrlItem(string pathItem)
    {
        return pathItem.All(c => char.IsAsciiLetter(c) || char.IsAsciiDigit(c) || otherAllowedCharacters.Contains(c));
    }
}
