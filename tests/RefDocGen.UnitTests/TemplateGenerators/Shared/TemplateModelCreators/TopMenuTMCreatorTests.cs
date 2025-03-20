using Shouldly;
using RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;
using RefDocGen.TemplateGenerators.Shared.Tools.StaticPages;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Menu;

namespace RefDocGen.UnitTests.TemplateGenerators.Shared.TemplateModelCreators;

/// <summary>
/// Class containing tests for <see cref="TopMenuTMCreator"/> class.
/// </summary>
public class TopMenuTMCreatorTests
{
    /// <summary>
    /// Creator of the <see cref="TopMenuDataTM"/> objects.
    /// </summary>
    private readonly TopMenuTMCreator topMenuTMCreator;

    /// <summary>
    /// Directory separator char.
    /// </summary>
    private readonly char dirSep = Path.DirectorySeparatorChar;

    public TopMenuTMCreatorTests()
    {
        topMenuTMCreator = new();
    }


    [Fact]
    public void CreateFrom_ReturnsCorrectData_ForNonIndexPage()
    {
        var page = new StaticPage(".", "custom-page", "");

        var menuItems = topMenuTMCreator.CreateFrom([page]);

        menuItems.Pages.ShouldBe([new("API", "api/index.html"), new("Custom page", "custom-page.html")]);
        menuItems.Folders.ShouldBeEmpty();
    }

    [Fact]
    public void CreateFrom_ReturnsCorrectData_ForIndexPage()
    {
        var page = new StaticPage(".", "index", "");

        var menuItems = topMenuTMCreator.CreateFrom([page]);

        menuItems.Pages.ShouldBe([new("Home", "index.html"), new("API", "api/index.html")]);
        menuItems.Folders.ShouldBeEmpty();
    }

    [Fact]
    public void CreateFrom_ReturnsCorrectData_ForComplexPageStructure()
    {
        StaticPage[] pages = [
            new(".", "contact", ""),
            new(".", "FAQ", ""),
            new("sub", "page-1", ""),
            new("sub", "page-2", ""),
            new("sub2", "page", ""),
            new($"sub{dirSep}xxx", "page", "")
            ];

        var menuItems = topMenuTMCreator.CreateFrom(pages);

        menuItems.Pages.ShouldBe([
            new("API", "api/index.html"),
            new("Contact", "contact.html"),
            new("FAQ", "FAQ.html")
            ]);

        menuItems.Folders.Length.ShouldBe(2);

        menuItems.Folders[0].Name.ShouldBe("Sub");
        menuItems.Folders[0].Pages.ShouldBe([
            new("Page 1", $"sub{dirSep}page-1.html"),
            new("Page 2", $"sub{dirSep}page-2.html")
            ]);

        menuItems.Folders[1].Name.ShouldBe("Sub2");
        menuItems.Folders[1].Pages.ShouldBe([
            new("Page", $"sub2{dirSep}page.html")
            ]);
    }

}
