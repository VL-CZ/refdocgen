namespace RefDocGen.TemplateGenerators.Shared.TemplateModels.Menu;

public record MenuPageLinkTM(string PageName, string Url);

public record MenuFolderTM(string Name, MenuPageLinkTM[] Pages);

public record TopMenuTM(MenuPageLinkTM[] Pages, MenuFolderTM[] Folders);
