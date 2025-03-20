using AngleSharp;
using AngleSharp.Dom;

/// <summary>
/// Class containing static methods related to documentation pages.
/// </summary>
internal static class DocumentationTools
{
    /// <summary>
    /// Gets a documentation API page represented as <see cref="IDocument"/>.
    /// </summary>
    /// <param name="pageName">Name of the page to select.</param>
    /// <returns>The documentation page with the given <paramref name="pageName"/>, represented as <see cref="IDocument"/>.</returns>
    internal static IDocument GetApiPage(string pageName)
    {
        string fileUrl = Path.Join("api", pageName);
        return GetPage(fileUrl);
    }

    /// <summary>
    /// Gets a documentation page represented as <see cref="IDocument"/>.
    /// </summary>
    /// <param name="pageUrl">URL of the page to select, relative to the output directory.</param>
    /// <returns>The documentation page with the given <paramref name="pageUrl"/>, represented as <see cref="IDocument"/>.</returns>
    internal static IDocument GetPage(string pageUrl)
    {
        string file = Path.Join("output", pageUrl);
        string fileData = File.ReadAllText(file);

        // Configure and create a browsing context
        var config = Configuration.Default;
        var context = BrowsingContext.New(config);

        // Load the HTML document directly from the file
        var document = context.OpenAsync((req) => req.Content(fileData)).Result;
        return document;
    }
}
