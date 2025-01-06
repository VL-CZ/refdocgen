using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RefDocGen.TemplateGenerators.Default;

#pragma warning disable IDE0005
using RefDocGen.TemplateGenerators.Default.Templates;
#pragma warning restore IDE0005

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
        //string templatePath = "TemplateGenerators/Default/Templates/Default";
        //string outputDir = Path.Combine(projectPath, "out");
        string outputDir = Path.Combine(projectPath, "out-razor");

        IServiceCollection services = new ServiceCollection();
        _ = services.AddLogging();

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

        await using var htmlRenderer = new HtmlRenderer(serviceProvider, loggerFactory);

        //var templateGenerator = new DefaultTemplateGenerator(projectPath, templatePath, outputDir);
        var templateGenerator = new RazorTemplateGenerator<
                DelegateTypeTemplate,
                EnumTypeTemplate,
                NamespaceDetailTemplate,
                NamespaceListTemplate,
                ObjectTypeTemplate
            >(htmlRenderer, outputDir);

        var docGenerator = new DocGenerator(dllPath, docPath, templateGenerator);
        docGenerator.GenerateDoc();

        Console.WriteLine("Done...");
    }
}
