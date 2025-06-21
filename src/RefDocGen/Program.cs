using CommandLine;
using CommandLine.Text;
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
using System.Reflection;

namespace RefDocGen;

internal class Configuration
{
    [Usage(ApplicationAlias = "refdocgen INPUT [OPTIONS]")]
    public static IEnumerable<Example> Examples => [
        new("Generate reference documentation", new object())
    ];

    [Value(0, Required = true, MetaName = "INPUT", HelpText = "The assembly, project, or solution to document.")]
    public required string Input { get; set; }

    [Option('o', "output-dir", HelpText = "The output directory for the generated documentation.", Default = "reference-docs", MetaValue = "DIR")]
    public required string OutputDirectory { get; set; }

    [Option('t', "template", HelpText = "The template to use for the documentation.", Default = DocumentationTemplate.Default, MetaValue = "TEMPLATE")]
    public DocumentationTemplate Template { get; set; }

    [Option('s', "static-pages-dir", HelpText = "Directory containing additional static pages to include in the documentation.", Default = null, MetaValue = "DIR")]
    public string? StaticPagesDirectory { get; set; }

    [Option('v', "verbose", HelpText = "Enable verbose output.", Default = false)]
    public bool Verbose { get; set; }

    [Option("doc-version", HelpText = "Generate a specific version of the documentation.", Default = null, MetaValue = "VERSION")]
    public string? Version { get; set; }

    [Option("min-visibility", HelpText = "Minimum visibility level of types and members to include in the documentation.",
        Default = AccessModifier.Family, MetaValue = "VISIBILITY")]
    public AccessModifier MinVisibility { get; set; }

    [Option("inherit-members", Default = MemberInheritanceMode.NonObject, MetaValue = "MODE",
        HelpText = "Specify which inherited members to include in the documentation.")]
    public MemberInheritanceMode MemberInheritance { get; set; }

    [Option("exclude-projects", HelpText = "Projects to exclude from the documentation.", MetaValue = "PROJECT [PROJECT...]")]
    public required IEnumerable<string> ProjectsToExclude { get; set; }

    [Option("exclude-namespaces", HelpText = "Namespaces to exclude from the documentation.", MetaValue = "NAMESPACE [NAMESPACE...]")]
    public required IEnumerable<string> NamespacesToExclude { get; set; }
}

enum DocumentationTemplate { Default }

/// <summary>
/// Program class, containing main method
/// </summary>
public static class Program
{
    /// <summary>
    /// Main method, entry point of the RefDocGen app
    /// </summary>
    public static async Task Main(string[] args)
    {
        var parser = new Parser(with => with.HelpWriter = null);
        var parserResult = parser.ParseArguments<Configuration>(args);

        await parserResult.MapResult(
            Run,
            _ => DisplayHelp(parserResult)
        );
    }

    private static async Task Run(Configuration config)
    {
        string[] dllPaths = [config.Input];
        string[] docPaths = dllPaths.Select(p => p.Replace(".dll", ".xml")).ToArray();

        var assemblyDataConfig = new AssemblyDataConfiguration(
            MinVisibility: config.MinVisibility,
            MemberInheritanceMode: config.MemberInheritance,
            AssembliesToExclude: config.ProjectsToExclude,
            NamespacesToExclude: config.NamespacesToExclude
            );

        var serilogLogger = GetSerilogLogger(config.Verbose);

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

        Dictionary<DocumentationTemplate, ITemplateProcessor> templateProcessors = new()
        {
            [DocumentationTemplate.Default] = new DefaultTemplateProcessor(htmlRenderer, availableLanguages, config.StaticPagesDirectory, config.Version)
        };

        try
        {
            var templateProcessor = templateProcessors[config.Template];

            var docGenerator = new DocGenerator(dllPaths, docPaths, templateProcessor, assemblyDataConfig, config.OutputDirectory, logger);
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

    /// <summary>
    /// Gets a Serilog logger instance.
    /// </summary>
    /// <param name="verbose">Indicates whether the verbose mode is used.</param>
    /// <returns>An instance of Serilog logger.</returns>
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

    static async Task DisplayHelp(ParserResult<Configuration> result)
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version;

        var helpText = HelpText.AutoBuild(result, h =>
        {
            string? versionSuffix = version is not null
                ? $", version {version?.Major}.{version?.Minor}.{version?.Build}"
                : null;

            h.AddEnumValuesToHelpText = true;
            h.MaximumDisplayWidth = 100;
            h.AddNewLineBetweenHelpSections = true;
            h.Heading = $"{nameof(RefDocGen)} - reference documentation generator for .NET{versionSuffix}";
            h.OptionComparison = (o1, o2) =>
            {
                if (o1.IsValue && !o2.IsValue)
                {
                    return -1; // value before option
                }
                if (!o1.IsValue && o2.IsValue)
                {
                    return 1; // option after value
                }

                // else keep the original order
                if (o1.Index < o2.Index)
                {
                    return -1;
                }
                else if (o1.Index > o2.Index)
                {
                    return 1;
                }
                return 0;
            };

            return HelpText.DefaultParsingErrorsHandler(result, h);
        }, e => e);

        Console.WriteLine(helpText);
    }
}
