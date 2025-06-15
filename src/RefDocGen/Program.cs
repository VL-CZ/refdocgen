using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RefDocGen.AssemblyAnalysis;
using RefDocGen.CodeElements;
using RefDocGen.TemplateProcessors.Default;
using RefDocGen.TemplateProcessors.Shared.Languages;
using RefDocGen.Tools.Exceptions;
using Serilog;
using Serilog.Events;
using System.Globalization;

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
        string[] dllPaths = [Path.Join(rootPath, "demo-lib", "MyLibrary.dll"), Path.Join(rootPath, "demo-lib", "MyApp.dll")];
        string[] docPaths = [Path.Join(rootPath, "demo-lib", "MyLibrary.xml"), Path.Join(rootPath, "demo-lib", "MyApp.xml")];

        string projectPath = Path.Join(rootPath, "src", "RefDocGen");
        string outputDir = Path.Combine(projectPath, "out");
        string staticPagesDir = "C:\\Users\\vojta\\UK\\mgr-thesis\\refdocgen\\demo-lib\\pages";
        string? version = null;

        var assemblyDataConfig = new AssemblyDataConfiguration(
            MinVisibility: AccessModifier.Private,
            MemberInheritanceMode: MemberInheritanceMode.NonObject,
            AssembliesToExclude: ["MyApp"],
            NamespacesToExclude: ["MyLibrary.Exclude", "MyLibrary.Tools.Exclude"]
            );


        bool verbose = false;
        string outputTemplate = verbose
            ? "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
            : "[{Level:u}] {Message:lj}{NewLine}";

        var level = verbose ? LogEventLevel.Information : LogEventLevel.Warning;

        var serilogLogger = new LoggerConfiguration()
            .MinimumLevel.Is(level)
            .WriteTo.Console(outputTemplate: outputTemplate, formatProvider: CultureInfo.InvariantCulture)
            .CreateLogger();

        IServiceCollection services = new ServiceCollection();
        _ = services.AddLogging(builder => builder.AddSerilog(serilogLogger, dispose: true));

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

        await using var htmlRenderer = new HtmlRenderer(serviceProvider, loggerFactory);

        ILanguageConfiguration[] availableLanguages = [
            new CSharpLanguageConfiguration(),
            new OtherLanguageConfiguration()
        ];

        var logger = loggerFactory.CreateLogger("RefDocGen");

        try
        {
            var type = Type.GetType("RefDocGen.TemplateProcessors.Default.Templates.Components.LanguageSpecific.CSharp.CSharpFieldDeclaration");

            var templateProcessor = new DefaultTemplateProcessor(htmlRenderer, availableLanguages, staticPagesDir, version);

            var docGenerator = new DocGenerator(dllPaths, docPaths, templateProcessor, assemblyDataConfig, outputDir, logger);
            docGenerator.GenerateDoc();
            Console.WriteLine("Done...");
        }
        catch (Exception ex)
        {
            if (ex is RefDocGenFatalException refDocGenEx)
            {
                logger.LogError(refDocGenEx, "{ErrorMessage}", ex.Message);
            }
            else
            {
                logger.LogError(ex, "An exception occurred");
            }

        }
    }
}
