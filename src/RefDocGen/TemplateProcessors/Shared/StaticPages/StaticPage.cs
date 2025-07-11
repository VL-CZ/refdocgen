namespace RefDocGen.TemplateProcessors.Shared.StaticPages;

/// <summary>
/// Represents a static page created by user.
/// </summary>
/// <param name="PageDirectory">Path to the page directory, relative to the static files directory.</param>
/// <param name="PageName">Name of the page file, without extension (e.g. "index").</param>
/// <param name="HtmlBody">HTML content of the page body.</param>
internal record StaticPage(string PageDirectory, string PageName, string HtmlBody)
{
    /// <summary>
    /// Gets the depth of the page relative to the static files folder.
    /// 0 is returned if the file is stored directly in the static files folder,
    /// while higher values indicate deeper subfolder levels.
    /// </summary>
    internal int FolderDepth
    {
        get
        {
            if (PageDirectory == ".")
            {
                return 0;
            }

            return PageDirectory.Count(c => c == Path.DirectorySeparatorChar) + 1;
        }
    }

    /// <summary>
    /// Indicates whether the page is an 'index' page.
    /// </summary>
    internal bool IsIndexPage => FolderDepth == 0 && PageName.Equals("index", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Full name of the page file, relative to the static files directory (without extension).
    /// </summary>
    internal string FullName => Path.Join(PageDirectory, PageName);
}
