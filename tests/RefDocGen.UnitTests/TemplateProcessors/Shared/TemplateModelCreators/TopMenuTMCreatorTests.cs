using RefDocGen.TemplateProcessors.Shared.StaticPages;
using RefDocGen.TemplateProcessors.Shared.TemplateModelCreators;
using RefDocGen.TemplateProcessors.Shared.TemplateModels.Menu;
using Shouldly;

namespace RefDocGen.UnitTests.TemplateProcessors.Shared.TemplateModelCreators;

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
    public void CreateFrom_ReturnsApiHomepageAndStaticPage_WhenNonIndexPageProvided()
    {
        var page = new StaticPage(".", "custom-page", "");

        var menuItems = topMenuTMCreator.CreateFrom([page]);

        menuItems.Pages.ShouldBe([new("API", "api/index.html"), new("Custom page", "custom-page.html")]);
        menuItems.Folders.ShouldBeEmpty();
    }

    [Fact]
    public void CreateFrom_ReturnsIndexPageAndApiHomepage_WhenIndexPageProvided()
    {
        var page = new StaticPage(".", "index", "");

        var menuItems = topMenuTMCreator.CreateFrom([page]);

        menuItems.Pages.ShouldBe([new("Home", "index.html"), new("API", "api/index.html")]);
        menuItems.Folders.ShouldBeEmpty();
    }

    [Fact]
    public void CreateFrom_ReturnsExpectedData_ForComplexPageStructure()
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
