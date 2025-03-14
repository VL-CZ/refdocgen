using RefDocGen.TemplateGenerators.Shared.TemplateModels.Menu;
using RefDocGen.TemplateGenerators.Shared.Tools.StaticPages;
using RefDocGen.Tools;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;

internal class TopMenuTMCreator
{
    internal static TopMenuDataTM Default => new([new("API", "api.html")], []);

    private string GetPageName(string pageName)
    {
        return pageName.Replace("-", " ").Capitalize();
    }

    private MenuPageLinkTM ToPage(StaticPage p)
    {
        return new MenuPageLinkTM(GetPageName(p.PageName), $"{Path.Combine(p.PageDirectory, p.PageName)}.html");
    }

    internal TopMenuDataTM CreateFrom(IEnumerable<StaticPage> pages)
    {
        List<MenuPageLinkTM> menuPages = [new("API", "api.html")];

        var newPages = pages.Where(p => p.FolderDepth == 0).Select(ToPage);

        menuPages.AddRange(newPages);

        if (menuPages.SingleOrDefault(p => p.PageName == "Index") is MenuPageLinkTM indexPage)
        {
            _ = menuPages.Remove(indexPage);
            indexPage = indexPage with { PageName = "Home" };
            menuPages.Insert(0, indexPage);
        }

        List<MenuFolderTM> menuFolders = [];

        var lookup = pages.Where(p => p.FolderDepth == 1)
            .ToLookup(p => p.PageDirectory);

        foreach (var dir in lookup)
        {
            string dirName = GetPageName(dir.Key);

            var dirPages = dir.Select(ToPage);

            menuFolders.Add(new(dirName, [.. dirPages]));
        }

        return new([.. menuPages], [.. menuFolders]);
    }
}
