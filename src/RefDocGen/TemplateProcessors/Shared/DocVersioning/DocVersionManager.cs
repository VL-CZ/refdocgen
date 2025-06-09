using System.Text.Json;
using AngleSharp;
using RefDocGen.Tools;
using RefDocGen.Tools.Exceptions;

namespace RefDocGen.TemplateProcessors.Shared.DocVersioning;

/// <summary>
/// Class responsible for managing different versions of the documentation.
/// </summary>
internal class DocVersionManager
{
    /// <summary>
    /// Available versions of the documentation
    /// </summary>
    private readonly List<DocVersion> versions = [];

    /// <summary>
    /// The documentation version being generated.
    /// </summary>
    private readonly string currentVersion;

    /// <summary>
    /// Base output directory, containing the JSON versions file.
    /// </summary>
    private readonly string baseOutputDirectory;

    /// <summary>
    /// HTML ID of the <c>Version list</c> element.
    /// </summary>
    private const string versionListElementId = "version-list";

    /// <summary>
    /// File containing the data about doc versions.
    /// </summary>
    private readonly FileInfo versionsFile;

    /// <summary>
    /// Initializes a new instance of <see cref="DocVersionManager"/> class.
    /// </summary>
    /// <param name="baseOutputDirectory">Base output directory, containing the JSON versions file.</param>
    /// <param name="currentVersion">Current version of the documentation being generated.</param>
    public DocVersionManager(string baseOutputDirectory, string currentVersion)
    {
        this.baseOutputDirectory = baseOutputDirectory;
        this.currentVersion = currentVersion;

        if (!UrlValidator.IsValid(currentVersion))
        {
            throw new DocVersionNameException(currentVersion); // invalid version name -> throw exception
        }

        versionsFile = new FileInfo(Path.Join(baseOutputDirectory, "versions.json"));

        if (versionsFile.Exists)
        {
            string json = File.ReadAllText(versionsFile.FullName);
            versions = JsonSerializer.Deserialize<List<DocVersion>>(json) ?? [];

            if (versions.Any(v => v.Version == currentVersion))
            {
                throw new DuplicateDocVersion(currentVersion); // there's already a version with the same name -> throw exception
            }
        }
        else
        {
            File.WriteAllText(versionsFile.FullName, ""); // create the file
        }
    }

    /// <summary>
    /// Saves the current version of the documentation to the JSON configuration file.
    /// </summary>
    /// <param name="pagesGenerated">Paths of the generated documentation pages, relative to the output directory.</param>
    internal void SaveCurrentVersionData(HashSet<string> pagesGenerated)
    {
        var pageUrls = pagesGenerated.Select(p => p.ToUrlPath()).ToHashSet();

        versions.Add(new(currentVersion, pageUrls));
        string serialized = JsonSerializer.Serialize(versions);

        File.WriteAllText(versionsFile.FullName, serialized);
    }

    /// <summary>
    /// Gets available versions of the given documentation page.
    /// </summary>
    /// <param name="pagePath">Path to the page, relative to the corresponding version directory.</param>
    /// <returns>Array of available versions of the given page.</returns>
    internal string[] GetVersions(string pagePath)
    {
        string pageUrl = pagePath.ToUrlPath();

        var oldVersions = versions.Where(v => v.PageUrls.Contains(pageUrl)).Select(v => v.Version);
        return [.. oldVersions, currentVersion];
    }

    /// <summary>
    /// Updates the older versions of the documentation to contain the link to the most recent version.
    /// </summary>
    /// <param name="pagesGenerated">Paths of the generated documentation pages, relative to the output directory.</param>
    internal void UpdateOlderVersions(HashSet<string> pagesGenerated)
    {
        foreach (string pagePath in pagesGenerated)
        {
            UpdateOlderPageVersions(pagePath);
        }
    }

    /// <summary>
    /// Updates all older version of the page to contain a link to its most recent version.
    /// </summary>
    /// <param name="pagePath">Path of the documentation page, relative to the output directory.</param>
    private void UpdateOlderPageVersions(string pagePath)
    {
        string[] versions = GetVersions(pagePath);

        foreach (string oldVersion in versions)
        {
            if (oldVersion == currentVersion)
            {
                continue;
            }

            string olderVersionFile = Path.Join(baseOutputDirectory, oldVersion, pagePath);
            string olderVersionHtml = File.ReadAllText(olderVersionFile); // load the older version HTML

            // Create a new browsing context (configurations for AngleSharp)
            var config = Configuration.Default.WithDefaultLoader();

            // Parse the HTML content using AngleSharp's context
            var context = BrowsingContext.New(config);
            using var document = context.OpenAsync(req => req.Content(olderVersionHtml)).Result;

            var versionList = document.GetElementById(versionListElementId);

            if (versionList is not null)
            {
                versionList.InnerHtml = JsonSerializer.Serialize(versions); // add the current version to the 'Version list' element
            }

            File.WriteAllText(olderVersionFile, document.ToHtml()); // Write the updated HTML
        }
    }
}
