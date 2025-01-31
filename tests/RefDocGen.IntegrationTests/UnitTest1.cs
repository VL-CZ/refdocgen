using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RefDocGen.TemplateGenerators.Default.Templates;
using RefDocGen.TemplateGenerators.Default;
using RefDocGen.TemplateGenerators.Tools.DocComments.Html;
using System.Xml.Linq;
using FluentAssertions;
using AngleSharp;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System.Reflection.Metadata;

namespace RefDocGen.IntegrationTests;

public sealed class DocumentationFixture : IDisposable
{
    private const string outputDir = "output";

    public DocumentationFixture()
    {
        Initialize();
    }

    public void Dispose()
    {
        Directory.Delete(outputDir, true);
    }

    public void Initialize()
    {
        string outputDir = "output";

        Directory.CreateDirectory(outputDir);

        IServiceCollection services = new ServiceCollection();
        _ = services.AddLogging();

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

        using var htmlRenderer = new HtmlRenderer(serviceProvider, loggerFactory);

        IDocCommentTransformer docCommentParser = new DefaultDocCommentTransformer(
                new DocCommentHtmlConfiguration()
            );

        var templateGenerator = new RazorTemplateGenerator<
                DelegateTypeTemplate,
                EnumTypeTemplate,
                NamespaceDetailTemplate,
                NamespaceListTemplate,
                ObjectTypeTemplate
            >(htmlRenderer, docCommentParser, outputDir);

        var generator = new DocGenerator("data/MyLibrary.dll", "data/MyLibrary.xml", templateGenerator);

        generator.GenerateDoc();
    }


}

public class UnitTest1 : IClassFixture<DocumentationFixture>
{
    private IDocument GetDocument(string name)
    {
        var userFile = Path.Join("output", name);
        var fileData = File.ReadAllText(userFile);

        // Configure and create a browsing context
        var config = Configuration.Default;
        var context = BrowsingContext.New(config);

        // Load the HTML document directly from the file
        var document = context.OpenAsync((req) => req.Content(fileData)).Result; // TODO
        return document;
    }

    private INode GetSignature(IElement memberElement)
    {
        return memberElement.FirstChild ?? throw new Exception();
    }

    private IHtmlDivElement GetDocComment(IElement memberElement)
    {
        return memberElement.FindChild<IHtmlDivElement>() ?? throw new Exception();
    }

    [Fact]
    public void Test1()
    {
        var document = GetDocument("MyLibrary.User.html");

        // Access elements using query selectors
        var isAdultMethod = document.GetElementById("IsAdult").FirstChild;
        string content = Regex.Replace(isAdultMethod.TextContent, @"\s+", " ").Trim();

        content.Should().Be("public bool IsAdult() #");
    }

    [Fact]
    public void Test2()
    {
        var document = GetDocument("MyLibrary.User.html");

        // Access elements using query selectors
        var docComment = document.GetElementById("IsAdult").FindChild<IHtmlDivElement>();
        string content = Regex.Replace(docComment.TextContent, @"\s+", " ").Trim();

        content.Should().Be("Checks if the user is adult.");
    }
}
