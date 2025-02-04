using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RefDocGen.TemplateGenerators.Default;
using RefDocGen.CodeElements;

namespace RefDocGen.IntegrationTests;

public class DocumentationFixture : IDisposable
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

        if (Directory.Exists(outputDir))
        {
            Directory.Delete(outputDir, true);
        }

        Directory.CreateDirectory(outputDir);

        IServiceCollection services = new ServiceCollection();
        _ = services.AddLogging();

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

        using var htmlRenderer = new HtmlRenderer(serviceProvider, loggerFactory);

        var templateGenerator = new DefaultTemplateGenerator(htmlRenderer, outputDir);

        var generator = new DocGenerator("data/MyLibrary.dll", "data/MyLibrary.xml", templateGenerator, AccessModifier.Private);

        generator.GenerateDoc();
    }
}


[CollectionDefinition(Name)]
public class MyTestCollection : ICollectionFixture<DocumentationFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
    internal const string Name = "Database collection";

}

