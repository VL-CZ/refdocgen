using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RefDocGen.AssemblyAnalysis;
using RefDocGen.CodeElements;
using RefDocGen.TemplateGenerators.Default;

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

    /// <summary>
    /// Path to the directory containing user-created static pages
    /// </summary>
    private readonly string staticPagesDirectory = Path.Join("data", "static-pages");

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

        var templateGenerator = new DefaultTemplateGenerator(htmlRenderer, outputDir, staticPagesDirectory); // use the default template generator

        var assemblyDataConfig = new AssemblyDataConfiguration(
            AccessModifier.Private,
            MemberInheritanceMode.NonObject,
            NamespacesToExclude: ["MyLibrary.Exclude", "MyLibrary.Tools.Exclude"],
            AssembliesToExclude: []);

        var generator = new DocGenerator(["data/MyLibrary.dll"], ["data/MyLibrary.xml"], templateGenerator, assemblyDataConfig);

        generator.GenerateDoc();
    }
}
