namespace RefDocGen.TemplateProcessors.Shared.TemplateTools;

/// <summary>
/// Class with extension methods for <see cref="string"/> class.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Adds the &lt;wbr /&gt; tag after each dot in the given string.
    /// </summary>
    /// <param name="input">The provided string.</param>
    /// <returns>String with the &lt;wbr /&gt; tag after each dot.</returns>
    public static string WithWbrAfterDots(this string input)
    {
        return input.Replace(".", ".<wbr />");
    }
}
