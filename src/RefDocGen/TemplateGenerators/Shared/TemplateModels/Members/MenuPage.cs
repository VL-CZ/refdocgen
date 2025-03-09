namespace RefDocGen.TemplateGenerators.Shared.TemplateModels.Members;

public record MenuPage(string PageName, string Url);

public record MenuFolder(string Name, MenuPage[] Pages);

public record MenuTM(MenuPage[] Pages, MenuFolder[] Folders);
