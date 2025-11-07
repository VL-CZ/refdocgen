using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Markdig;
using Microsoft.Extensions.Logging;
using RefDocGen.Tools.Exceptions;

namespace RefDocGen.TemplateProcessors.Shared.StaticPages;

/// <summary>
/// Class responsible for handling all user-provided static pages.
/// </summary>
internal class StaticPageProcessor
{
    /// <summary>
    /// Path to the folder containing the static pages.
    /// </summary>
    private readonly string staticPagesDirectory;

    /// <summary>
    /// HTML file extension.
    /// </summary>
    private const string htmlExt = ".html";

    /// <summary>
    /// Markdown file extension.
    /// </summary>
    private const string markdownExt = ".md";

    /// <summary>
    /// File extensions of the pages.
    /// </summary>
    private static readonly string[] pageFileExtensions = [htmlExt, markdownExt];

    /// <summary>
    /// Path to the CSS file, relative to <see cref="staticPagesDirectory"/>.
    /// </summary>
    internal static readonly string cssFilePath = Path.Join("css", "styles.css");

    /// <summary>
    /// A logger instance.
    /// </summary>
    private readonly ILogger? logger;

    /// <summary>
    /// Initializes a new instance of <see cref="StaticPageProcessor"/> class.
    /// </summary>
    /// <param name="staticPagesDirectory">Path to the folder containing the static pages.</param>
    /// <param name="logger">A logger instance.</param>
    public StaticPageProcessor(string staticPagesDirectory, ILogger? logger)
    {
        this.staticPagesDirectory = staticPagesDirectory;
        this.logger = logger;

        if (!Directory.Exists(staticPagesDirectory))
        {
            throw new StaticPagesDirectoryNotFoundException(staticPagesDirectory); // Static pages directory not found
        }
    }

    /// <summary>
    /// Gets all static pages stored in the static pages folder or its subfolders.
    /// </summary>
    /// <remarks>
    /// Markdown pages are internally converted into HTML.
    /// </remarks>
    /// <returns>All static pages stored in the static pages folder or its subfolders.</returns>
    internal IEnumerable<StaticPage> GetStaticPages()
    {
        foreach (string filePath in Directory.GetFiles(staticPagesDirectory, "*", SearchOption.AllDirectories).OrderBy(path => path))
        {
            var file = new FileInfo(filePath);
            if (pageFileExtensions.Contains(file.Extension))
            {
                string fileText = File.ReadAllText(filePath);

                if (file.Extension == markdownExt) // convert Markdown to HTML
                {
                    string md = File.ReadAllText(filePath);
                    fileText = Markdown.ToHtml(md);
                }

                fileText = ProcessPage(fileText);
                string pageDir = Path.GetRelativePath(staticPagesDirectory, file.DirectoryName ?? staticPagesDirectory);

                yield return new(pageDir, Path.GetFileNameWithoutExtension(filePath), fileText);
            }
        }
    }

    /// <summary>
    /// Returns a <see cref="FileInfo"/> object representing the user provided CSS file.
    /// </summary>
    /// <returns>A <see cref="FileInfo"/> object representing the user provided CSS file.</returns>
    internal FileInfo GetCssFile()
    {
        string cssPath = Path.Join(staticPagesDirectory, cssFilePath);
        return new FileInfo(cssPath);
    }

    /// <summary>
    /// Copies all files stored in the static pages folder or its subfolders that are not pages to the <paramref name="outputDirectory"/>.
    /// </summary>
    /// <param name="outputDirectory">Directory, where the files will be copied.</param>
    internal void CopyNonPageFiles(string outputDirectory)
    {
        foreach (string filePath in Directory.GetFiles(staticPagesDirectory, "*", SearchOption.AllDirectories))
        {
            var file = new FileInfo(filePath);
            if (!pageFileExtensions.Contains(file.Extension))
            {
                string relativePath = Path.GetRelativePath(staticPagesDirectory, file.FullName);
                string outputPath = Path.Join(outputDirectory, relativePath);

                string? dir = Path.GetDirectoryName(outputPath);

                if (dir is not null)
                {
                    _ = Directory.CreateDirectory(dir);
                }

                File.Copy(file.FullName, outputPath, true);
                logger?.LogInformation("Static file {FilePath} copied to {OutputPath}", filePath, outputPath);
            }
        }
    }

    /// <summary>
    /// Processes the static page, the following operations are performed:
    /// <list type="number">
    /// <item>Seaching for all links leading to Markdown files and updating them to point to the corresponding HTML pages.</item>
    /// <item>Wrapping images inside &lt;a&gt; elements.</item>
    /// </list>
    /// </summary>
    /// <param name="html">The provided HTML content.</param>
    /// <returns>The HTML content with all Markdown links resolved.</returns>
    private string ProcessPage(string html)
    {
        var config = Configuration.Default;
        var context = BrowsingContext.New(config);

        // Load the HTML document directly from the file
        var document = context.OpenAsync((req) => req.Content(html)).Result;

        document = ResolveLinksToMarkdownFiles(document);
        document = AddImageLinks(document);

        return document.Body?.InnerHtml ?? "";
    }

    /// <summary>
    /// Seaches for all links leading to Markdown files and updates them to point to the corresponding HTML pages.
    /// </summary>
    /// <param name="document">The provided HTML document.</param>
    /// <returns>The HTML document with all Markdown links updated to point to the corresponding HTML pages.</returns>
    private IDocument ResolveLinksToMarkdownFiles(IDocument document)
    {
        var links = document.QuerySelectorAll("a[href]");
        const string href = "href";

        foreach (var link in links) // browse all links and change them from ".md" to ".html"
        {
            if (link.GetAttribute(href) is string hrefValue && hrefValue.EndsWith(markdownExt, StringComparison.InvariantCulture))
            {
                string htmlPage = hrefValue[..^3] + htmlExt;
                link.SetAttribute(href, htmlPage);
            }
        }

        return document;
    }

    /// <summary>
    /// Wraps all <c>&lt;img&gt;</c> elements in the document with <c>&lt;a&gt;</c> elements,
    /// pointing to the image source.
    /// </summary>
    /// <param name="document">The provided HTML document.</param>
    /// <returns>The modified <see cref="IDocument"/> with images wrapped in <c>&lt;a&gt;</c> elements.</returns>
    private IDocument AddImageLinks(IDocument document)
    {
        var images = document.QuerySelectorAll("img");

        foreach (var image in images) // browse all images and wrap them inside <a> elements
        {
            if (image.GetAttribute("src") is string imagePath)
            {
                // Create <a> element
                var link = document.CreateElement<IHtmlAnchorElement>();
                link.Href = imagePath;

                // Wrap the image with <a>
                image.Replace(link);
                _ = link.AppendChild(image);
            }
        }

        return document;
    }
}
