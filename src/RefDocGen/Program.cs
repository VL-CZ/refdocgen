using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RefDocGen.AssemblyAnalysis;
using RefDocGen.CodeElements;
using RefDocGen.TemplateGenerators.Default;

namespace RefDocGen;

/// <summary>
/// Program class, containing main method
/// </summary>
public static class Program
{
    /// <summary>
    /// Main method, entry point of the RefDocGen app
    /// </summary>
    public static async Task Main()
    {
        string? rootPath = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent?.Parent?.Parent?.Parent?.Parent?.FullName;
        string dllPath = Path.Join(rootPath, "demo-lib", "MyLibrary.dll");
        string docPath = Path.Join(rootPath, "demo-lib", "MyLibrary.xml");

        string projectPath = Path.Join(rootPath, "src", "RefDocGen");
        string outputDir = Path.Combine(projectPath, "out-versions");
        string staticPagesDir = "C:\\Users\\vojta\\UK\\mgr-thesis\\refdocgen\\demo-lib\\pages";
        string? version = "v1.9";

        var minVisibility = AccessModifier.Private;
        var memberInheritanceMode = MemberInheritanceMode.NonObject;

        IServiceCollection services = new ServiceCollection();
        _ = services.AddLogging();

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

        await using var htmlRenderer = new HtmlRenderer(serviceProvider, loggerFactory);

        var templateGenerator = new DefaultTemplateGenerator(htmlRenderer, outputDir, staticPagesDir, version);

        var docGenerator = new DocGenerator(dllPath, docPath, templateGenerator, minVisibility, memberInheritanceMode);
        docGenerator.GenerateDoc();

        Console.WriteLine("Done...");
    }
}
