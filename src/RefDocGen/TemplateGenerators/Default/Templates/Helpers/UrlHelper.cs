namespace RefDocGen.TemplateGenerators.Default.Templates.Helpers;

/// <summary>
/// Helper class for URL manipulation, specifically for generating relative URIs.
/// </summary>
public static class UrlHelper
{
    /// <summary>
    /// Gets a relative URI based on the provided file name and the nesting level of the current page.
    /// The nesting level determines how many directory levels to go up in the URL before accessing the file.
    /// </summary>
    /// <param name="fileName">The name of the file to access, which will be included at the end of the URL.</param>
    /// <param name="nestingLevel">The number of directory levels to go up from the current page.</param>
    /// <returns>A relative URL (as a string) pointing to the specified file.</returns>
    public static string GetRelativeUrl(string fileName, int nestingLevel)
    {
        string[] fragments = [.. Enumerable.Repeat("..", nestingLevel), fileName];
        return string.Join('/', fragments);
    }
}
