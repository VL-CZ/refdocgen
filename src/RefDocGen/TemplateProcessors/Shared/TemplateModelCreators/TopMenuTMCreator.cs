using RefDocGen.TemplateProcessors.Shared.StaticPages;
using RefDocGen.TemplateProcessors.Shared.TemplateModels.Menu;
using RefDocGen.Tools;

namespace RefDocGen.TemplateProcessors.Shared.TemplateModelCreators;

/// <summary>
/// Class responsible for creating <see cref="TopMenuDataTM"/> object, representing the data in the top menu.
/// </summary>
internal class TopMenuTMCreator
{
    /// <summary>
    /// 'API' menu page.
    /// </summary>
    private static readonly MenuPageLinkTM apiMenuPage = new("API", "api/index.html");

    /// <summary>
    /// 'Home' menu page.
    /// </summary>
    private static readonly MenuPageLinkTM homeMenuPage = new("Home", "index.html");

    /// <summary>
    /// Default top menu data - containing just the link to the 'API' page.
    /// </summary>
    internal static TopMenuDataTM DefaultMenuData => new([apiMenuPage], []);

    /// <summary>
    /// Gets the page name intended to be displayed to users.
    /// </summary>
    /// <param name="pageFileName">Name of the file containing the page, without extension.</param>
    /// <returns>The name of the page intended to be displayed to users.</returns>
    private string GetPageName(string pageFileName)
    {
        return pageFileName.Replace("-", " ").Capitalize();
    }

    /// <summary>
    /// Constructs a <see cref="MenuPageLinkTM"/> object from the provided <see cref="StaticPage"/>.
    /// </summary>
    /// <param name="page">The static page to convert.</param>
    /// <returns>A <see cref="MenuPageLinkTM"/> object representing the page.</returns>
    private MenuPageLinkTM ToMenuPage(StaticPage page)
    {
        string url = page.FolderDepth == 0
            ? page.PageName
            : Path.Combine(page.PageDirectory, page.PageName);

        return new MenuPageLinkTM(GetPageName(page.PageName), $"{url}.html");
    }

    /// <summary>
    /// Creates a <see cref="TopMenuDataTM"/> object representing the top menu items from the provided static pages.
    /// </summary>
    /// <param name="pages">A collection of user created static pages.</param>
    /// <returns>A <see cref="TopMenuDataTM"/> object representing the top menu items.</returns>
    internal TopMenuDataTM CreateFrom(IEnumerable<StaticPage> pages)
    {
        List<MenuPageLinkTM> menuPages = [apiMenuPage];

        var newPages = pages.Where(p => p.FolderDepth == 0 && !p.IsIndexPage).Select(ToMenuPage);
        menuPages.AddRange(newPages); // add menu pages (except the index page), corresponding to the files in the static pages folder

        if (pages.SingleOrDefault(p => p.IsIndexPage) is StaticPage page) // check for index page
        {
            menuPages.Insert(0, homeMenuPage);
        }

        List<MenuFolderTM> menuFolders = [];

        var directoryLookup = pages
            .Where(p => p.FolderDepth == 1) // select only the pages in the depth 1 (relative to the static pages folder)
            .ToLookup(p => p.PageDirectory);

        foreach (var dir in directoryLookup) // add menu folder, corresponding to directories
        {
            string dirName = GetPageName(dir.Key);
            var dirPages = dir.OrderBy(page => page.PageName).Select(ToMenuPage);
            menuFolders.Add(new(dirName, [.. dirPages]));
        }

        return new([.. menuPages], [.. menuFolders]);
    }
}
