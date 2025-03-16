using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

/// <summary>
/// This class tests that user-specified static pages are correctly included in the documentation.
/// </summary>
[Collection(DocumentationTestCollection.Name)]
public class StaticPagesTests
{
    [Theory]
    [InlineData("helloworld.js")]
    [InlineData("css/styles.css")]
    public void NonPage_File_Is_Copied(string filePath)
    {
        string outputfilePath = Path.Join("output", filePath);
        var file = new FileInfo(outputfilePath);

        file.Exists.ShouldBeTrue();

        string fileText = File.ReadAllText(outputfilePath);

        string originalFile = Path.Join("data", "static-pages", filePath);
        string expectedFileText = File.ReadAllText(originalFile);

        fileText.ShouldBe(expectedFileText);
    }

    [Theory]
    [InlineData("htmlPage.html", "<h1>Heading 1</h1> <h2>Heading 2</h2>")]
    [InlineData("folder/anotherPage.html", "<div>Text</div>")]
    [InlineData("markdownPage.html", "<h1>Markdown page</h1> <p>Text</p>")]
    public void StaticPage_Is_Created(string filePath, string expectedHtml)
    {
        var page = DocumentationTools.GetPage(filePath);

        string normalizedBodyHtml = page.GetPageBody().GetNormalizedInnerHtml();

        normalizedBodyHtml.ShouldBe(expectedHtml);
    }
}
