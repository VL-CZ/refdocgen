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
    [Fact]
    public void Test1()
    {
        var userFile = Path.Join("output", "MyLibrary.User.html");
        var fileData = File.ReadAllText(userFile);

        // Configure and create a browsing context
        var config = Configuration.Default;
        var context = BrowsingContext.New(config);

        // Load the HTML document directly from the file
        var document = context.OpenAsync((req) => req.Content(fileData)).Result; // TODO

        // Access elements using query selectors
        var isAdultMethod = document.GetElementById("IsAdult").FirstChild;
        string content = Regex.Replace(isAdultMethod.TextContent, @"\s+", " ").Trim();

        content.Should().Be("public bool IsAdult() #");
    }
}
