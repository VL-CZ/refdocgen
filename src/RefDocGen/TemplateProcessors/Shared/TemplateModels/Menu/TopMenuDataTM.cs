namespace RefDocGen.TemplateProcessors.Shared.TemplateModels.Menu;

/// <summary>
/// Represents a page link in the top menu.
/// </summary>
/// <param name="PageName">Name of the page, intended to be displayed to the user.</param>
/// <param name="Url">URL path to the page, relative to the current page.</param>
public record MenuPageLinkTM(string PageName, string Url);

/// <summary>
/// Represents a folder in the top menu.
/// </summary>
/// <param name="Name">Name of the folder, intended to be displayed to the user.</param>
/// <param name="Pages">Array of pages contained in the folder.</param>
public record MenuFolderTM(string Name, MenuPageLinkTM[] Pages);

/// <summary>
/// Template model representing the page links and folders contained in the top menu.
/// </summary>
/// <param name="Pages">Array of top menu page links.</param>
/// <param name="Folders">Array of top menu folders.</param>
public record TopMenuDataTM(MenuPageLinkTM[] Pages, MenuFolderTM[] Folders);
