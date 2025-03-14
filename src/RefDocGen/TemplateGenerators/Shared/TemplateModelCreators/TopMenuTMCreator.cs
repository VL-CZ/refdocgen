using RefDocGen.TemplateGenerators.Shared.TemplateModels.Menu;
using RefDocGen.TemplateGenerators.Shared.Tools.StaticPages;
using RefDocGen.Tools;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;

internal class TopMenuTMCreator
{
    internal static TopMenuTM Default => new([new("API", "api.html")], []);

    private MenuPageLinkTM ToPage(StaticPage p)
    {
        return new MenuPageLinkTM(p.PageName.Replace("-", " ").Capitalize(), $"{Path.Combine(p.PageDirectory, p.PageName)}.html");
    }

    internal TopMenuTM CreateFrom(IEnumerable<StaticPage> pages)
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
            var dirName = dir.Key.Replace("-", " ").Capitalize();

            var dirPages = dir.Select(ToPage);

            menuFolders.Add(new(dirName, [.. dirPages]));
        }

        return new([.. menuPages], [.. menuFolders]);
    }
}
