using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RefDocGen.AssemblyAnalysis;
using RefDocGen.CodeElements;
using RefDocGen.TemplateProcessors.Default;
using RefDocGen.TemplateProcessors.Shared.Languages;

namespace RefDocGen.IntegrationTests.Fixtures;

/// <summary>
/// Fixture responsible for setting up and tearing down the reference documentation consisting of multiple versions.
/// </summary>
/// <remarks>
/// This fixture uses the DLL and XML documentation of the 'RefDocGen.ExampleLibrary'
/// </remarks>
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
            var templateProcessor = new DefaultTemplateProcessor(htmlRenderer, [new CSharpLanguageConfiguration()], staticPagesDirectory, version); // use the default template generator

            var logger = Substitute.For<ILogger>();

            var generator = new DocGenerator(
                ["../../../../RefDocGen.ExampleLibrary/bin/Debug/net8.0/RefDocGen.ExampleLibrary.dll"], // use only the ExampleLibrary
                templateProcessor,
                assemblyDataConfig,
                outputDir,
                "RefDocGen.ExampleLibrary",
                logger);

            generator.GenerateDoc();
        }
    }
}
