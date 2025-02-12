using AngleSharp;
using AngleSharp.Dom;

/// <summary>
/// Class containing static methods related to documentation pages.
/// </summary>
internal static class DocumentationTools
{
    /// <summary>
    /// Gets a documentation page represented as <see cref="IDocument"/>.
    /// </summary>
    /// <param name="name">Name of the page to select.</param>
    /// <returns>The documentation page with the given <paramref name="name"/>, represented as <see cref="IDocument"/>.</returns>
    internal static IDocument GetPage(string name)
    {
        string file = Path.Join("output", name);
        string fileData = File.ReadAllText(file);

        // Configure and create a browsing context
        var config = Configuration.Default;
        var context = BrowsingContext.New(config);

        // Load the HTML document directly from the file
        var document = context.OpenAsync((req) => req.Content(fileData)).Result;
        return document;
    }
}
