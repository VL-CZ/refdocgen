using RefDocGen.TemplateGenerators.Shared.TemplateModels.Menu;
using RefDocGen.TemplateGenerators.Shared.Tools.StaticPages;
using RefDocGen.Tools;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;

/// <summary>
/// Class responsible for creating <see cref="TopMenuDataTM"/> object, representing the data in the top menu.
/// </summary>
internal class TopMenuTMCreator
{
    /// <summary>
    /// 'API' menu page.
    /// </summary>
    private static readonly MenuPageLinkTM apiMenuPage = new("API", "api.html");

    /// <summary>
    /// 'Home' menu page.
    /// </summary>
    private static readonly MenuPageLinkTM homeMenuPage = new("Home", "index.html");

    /// <summary>
    /// Default menu data.
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
        return new MenuPageLinkTM(GetPageName(page.PageName), $"{Path.Combine(page.PageDirectory, page.PageName)}.html");
    }

    /// <summary>
    /// Creates a <see cref="TopMenuDataTM"/> object representing the top menu items from the provided static pages.
    /// </summary>
    /// <param name="pages">A collection of user created static pages.</param>
    /// <returns>A <see cref="TopMenuDataTM"/> object representing the top menu items.</returns>
    internal TopMenuDataTM CreateFrom(IEnumerable<StaticPage> pages)
    {
        List<MenuPageLinkTM> menuPages = [apiMenuPage];

        var newPages = pages.Where(p => p.FolderDepth == 0).Select(ToMenuPage);
        menuPages.AddRange(newPages); // add menu pages, corresponding to the files in the static pages folder

        if (menuPages.SingleOrDefault(p => p.Url == "index") is MenuPageLinkTM indexPage) // check for index page
        {
            _ = menuPages.Remove(indexPage);
            menuPages.Insert(0, homeMenuPage);
        }

        List<MenuFolderTM> menuFolders = [];

        var directoryLookup = pages
            .Where(p => p.FolderDepth == 1) // select only the pages in the depth 1 (relative to the static pages folder)
            .ToLookup(p => p.PageDirectory);

        foreach (var dir in directoryLookup) // add menu folder, corresponding to directories
        {
            string dirName = GetPageName(dir.Key);
            var dirPages = dir.Select(ToMenuPage);
            menuFolders.Add(new(dirName, [.. dirPages]));
        }

        return new([.. menuPages], [.. menuFolders]);
    }
}
