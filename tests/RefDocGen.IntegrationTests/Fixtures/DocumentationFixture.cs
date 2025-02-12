using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RefDocGen.TemplateGenerators.Default;
using RefDocGen.CodeElements;

namespace RefDocGen.IntegrationTests.Fixtures;

/// <summary>
/// Fixture responsible for setting up and tearing down the reference documentation for testing purposes.
/// </summary>
public class DocumentationFixture : IDisposable
{
    /// <summary>
    /// Directory, where the reference documentation is generated.
    /// </summary>
    private const string outputDir = "output";

    public DocumentationFixture()
    {
        GenerateDoc();
    }

    public void Dispose()
    {
        Directory.Delete(outputDir, true);
    }

    /// <summary>
    /// Generates the reference documentation.
    /// </summary>
    public void GenerateDoc()
    {
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

        var templateGenerator = new DefaultTemplateGenerator(htmlRenderer, outputDir); // use default template generator

        var generator = new DocGenerator("data/MyLibrary.dll", "data/MyLibrary.xml", templateGenerator, AccessModifier.Private);
        generator.GenerateDoc();
    }
}


/// <summary>
/// Marks the collection of tests that use the <see cref="DocumentationFixture"/>.
/// </summary>
[CollectionDefinition(Name)]
#pragma warning disable CA1711
public class DocumentationTestCollection : ICollectionFixture<DocumentationFixture>
{
    /// <summary>
    /// Name of this test collection.
    /// </summary>
    internal const string Name = "documentation-test-collection";
}

#pragma warning restore CA1711
