using CommandLine;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RefDocGen.AssemblyAnalysis;
using RefDocGen.CodeElements;
using RefDocGen.TemplateProcessors;
using RefDocGen.TemplateProcessors.Default;
using RefDocGen.TemplateProcessors.Shared.Languages;
using RefDocGen.Tools.Exceptions;
using Serilog;
using Serilog.Events;
using System.Globalization;

namespace RefDocGen;

internal class Options
{
    [Value(0, Required = true)]
    public string Source { get; set; }

    [Option('o', "output-dir", HelpText = "The directory where the documentation will be saved.", Default = "reference-docs")]
    public string OutputDirectory { get; set; }

    [Option('v', "verbose", HelpText = "Use verbose mode.", Default = false)]
    public bool Verbose { get; set; }

    [Option('t', "template-processor", HelpText = "The template processor to use.", Default = "default")]
    public string TemplateProcessor { get; set; }

    [Option('s', "static-pages-dir", HelpText = "Path to the directory containing user-specified static pages.", Default = null)]
    public string? StaticPagesDirectory { get; set; }

    [Option("version", HelpText = "Generate a specific version of the documentation.", Default = null)]
    public string? Version { get; set; }

    [Option("min-visibility", HelpText = "Minimum visibility of the types and members to be included in the documentation.", Default = AccessModifier.Family)]
    public AccessModifier MinVisibility { get; set; }

    [Option("inheritance-mode", HelpText = "Member inheritance mode.", Default = MemberInheritanceMode.NonObject)]
    public MemberInheritanceMode InheritanceMode { get; set; }

    [Option("assemblies-to-exclude", HelpText = "Assemblies to exclude from the documentation.")]
    public IEnumerable<string> AssembliesToExclude { get; set; }

    [Option("namespaces-to-exclude", HelpText = "Namespaces to exclude from the documentation.")]
    public IEnumerable<string> NamespacesToExclude { get; set; }
}

/// <summary>
/// Program class, containing main method
/// </summary>
public static class Program
{
    /// <summary>
    /// Main method, entry point of the RefDocGen app
    /// </summary>
    public static void Main(string[] args)
    {
        _ = Parser.Default.ParseArguments<Options>(args)
                    .MapResult(
                        Do,
                        async _ => await Task.FromResult(1)
                    );
    }

    private static async Task Do(Options o)
    {
        string[] dllPaths = [o.Source];
        string[] docPaths = dllPaths.Select(p => p.Replace(".dll", ".xml")).ToArray();

        var assemblyDataConfig = new AssemblyDataConfiguration(
            MinVisibility: o.MinVisibility,
            MemberInheritanceMode: o.InheritanceMode,
            AssembliesToExclude: o.AssembliesToExclude,
            NamespacesToExclude: o.NamespacesToExclude
            );

        var serilogLogger = GetSerilogLogger(o.Verbose);

        IServiceCollection services = new ServiceCollection();
        _ = services.AddLogging(builder => builder.AddSerilog(serilogLogger, dispose: true));

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("RefDocGen");

        await using var htmlRenderer = new HtmlRenderer(serviceProvider, loggerFactory);

        ILanguageConfiguration[] availableLanguages = [
          new CSharpLanguageConfiguration(),
                        new TodoLanguageConfiguration()
        ];

        Dictionary<string, ITemplateProcessor> templateProcessors = new()
        {
            ["default"] = new DefaultTemplateProcessor(htmlRenderer, availableLanguages, o.StaticPagesDirectory, o.Version)
        };

        try
        {
            var templateProcessor = templateProcessors[o.TemplateProcessor];

            var docGenerator = new DocGenerator(dllPaths, docPaths, templateProcessor, assemblyDataConfig, o.OutputDirectory, logger);
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
                logger.LogError(ex, "An error occurred, use the --verbose option to see detailed output");
            }

        }
    }

    private static Serilog.Core.Logger GetSerilogLogger(bool verbose)
    {
        string outputTemplate = verbose
            ? "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
            : "[{Level:u}] {Message:lj}{NewLine}";

        var level = verbose
            ? LogEventLevel.Information
            : LogEventLevel.Warning;

        return new LoggerConfiguration()
            .MinimumLevel.Is(level)
            .WriteTo.Console(outputTemplate: outputTemplate, formatProvider: CultureInfo.InvariantCulture)
            .CreateLogger();
    }
}
