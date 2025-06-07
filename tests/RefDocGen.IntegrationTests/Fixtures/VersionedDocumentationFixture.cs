using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RefDocGen.AssemblyAnalysis;
using RefDocGen.CodeElements;
using RefDocGen.TemplateProcessors.Shared.Languages;

namespace RefDocGen.IntegrationTests.Fixtures;

/// <summary>
/// Fixture responsible for setting up and tearing down the reference documentation consisting of multiple versions.
/// </summary>
public class VersionedDocumentationFixture : IDisposable
{
    /// <summary>
    /// Directory, where the reference documentation is generated.
    /// </summary>
    internal const string outputDir = "output-versions";

    /// <summary>
    /// Path to the directory containing user-created static pages
    /// </summary>
    private readonly string staticPagesDirectory = Path.Join("data", "static-pages");

    public VersionedDocumentationFixture()
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

        var assemblyDataConfig = new AssemblyDataConfiguration(AccessModifier.Private, MemberInheritanceMode.NonObject, [], []);

        string[] versions = ["v1.0", "v1.1", "v2.0"];

        foreach (string version in versions)
        {
            var templateGenerator = new DefaultTemplateGenerator(htmlRenderer, outputDir, [new CSharpLanguageConfiguration()], staticPagesDirectory, version); // use the default template generator

            var generator = new DocGenerator(["data/MyLibrary.dll"], ["data/MyLibrary.xml"], templateGenerator, assemblyDataConfig);
            generator.GenerateDoc();
        }
    }
}
