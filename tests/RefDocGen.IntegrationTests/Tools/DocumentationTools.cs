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
    /// <param name="pageVersion">Version of the page to retrieve. <c>null</c> if the version is not specified.</param>
    /// <param name="outputDirectory">Output directory containing the pages.</param>
    /// <returns>The documentation page with the given <paramref name="pageName"/>, represented as <see cref="IDocument"/>.</returns>
    internal static IDocument GetApiPage(string pageName, string outputDirectory = "output", string? pageVersion = null)
    {
        string fileUrl = Path.Join("api", pageName);
        return GetPage(fileUrl, outputDirectory, pageVersion);
    }

    /// <summary>
    /// Gets a documentation page represented as <see cref="IDocument"/>.
    /// </summary>
    /// <param name="pageUrl">URL of the page to select, relative to the output directory.</param>
    /// <param name="outputDirectory">Output directory containing the pages.</param>
    /// <param name="pageVersion">Version of the page to retrieve. <c>null</c> if the version is not specified.</param>
    /// <returns>The documentation page with the given <paramref name="pageUrl"/>, represented as <see cref="IDocument"/>.</returns>
    internal static IDocument GetPage(string pageUrl, string outputDirectory = "output", string? pageVersion = null)
    {
        string file = Path.Join(outputDirectory, pageVersion, pageUrl);
        string fileData = File.ReadAllText(file);

        // Configure and create a browsing context
        var config = Configuration.Default;
        var context = BrowsingContext.New(config);

        // Load the HTML document directly from the file
        var document = context.OpenAsync((req) => req.Content(fileData)).Result;
        return document;
    }
}
