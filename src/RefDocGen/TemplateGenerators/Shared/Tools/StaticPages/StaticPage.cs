namespace RefDocGen.TemplateGenerators.Shared.Tools.StaticPages;

internal record StaticPage(string PageDirectory, string PageName, string HtmlBody)
{
    /// <summary>
    /// Gets the depth of the file relative to the root folder.
    /// A value of 0 indicates that the file is in the root folder,
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
}
