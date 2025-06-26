using CommandLine;
using CommandLine.Text;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Build.Locator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RefDocGen.AssemblyAnalysis;
using RefDocGen.TemplateProcessors;
using RefDocGen.TemplateProcessors.Default;
using RefDocGen.TemplateProcessors.Shared.Languages;
using RefDocGen.Tools.Config;
using RefDocGen.Tools.Exceptions;
using Serilog;
using Serilog.Events;
using System.Globalization;
using System.Reflection;

namespace RefDocGen;

/// <summary>
/// Enum specifying the available documentation templates.
/// </summary>
internal enum DocumentationTemplate
{
    Default
    // #ADD_TEMPLATE: add an enum value representing the template here (e.g., 'Custom')
    // #ADD_TEMPLATE_PROCESSOR: add an enum value representing the template processor here (e.g., 'Liquid')
}

/// <summary>
/// Program class, containing main method.
/// </summary>
public static class Program
{
    /// <summary>
    /// Main method, the entry point of the <c>RefDocGen</c> app.
    /// </summary>
    public static async Task Main(string[] args)
    {
        _ = MSBuildLocator.RegisterDefaults(); // register the MSBuild instance, see https://learn.microsoft.com/en-us/visualstudio/msbuild/find-and-use-msbuild-versions?view=vs-2022#register-instance-before-calling-msbuild

        var parser = new Parser(with => with.HelpWriter = null);
        var parserResult = parser.ParseArguments<CommandLineConfiguration>(args);

        await parserResult.MapResult(
            Run,
            _ => DisplayHelp(parserResult)
        );
    }

    /// <summary>
    /// Runs the app using the provided configuration.
    /// </summary>
    /// <param name="config">The command-line configuration.</param>
    /// <returns>A completed task.</returns>
    private static async Task Run(CommandLineConfiguration config)
    {
        var serilogLogger = GetSerilogLogger(config.Verbose);

        IServiceCollection services = new ServiceCollection();
        _ = services.AddLogging(builder => builder.AddSerilog(serilogLogger, dispose: true));

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger(nameof(RefDocGen));

        await using var htmlRenderer = new HtmlRenderer(serviceProvider, loggerFactory);

        ILanguageConfiguration[] availableLanguages = [
            new CSharpLanguageConfiguration(),
            new TodoLanguageConfiguration()
            // #ADD_LANGUAGE: instantiate the custom language configuration here
        ];

        Dictionary<DocumentationTemplate, ITemplateProcessor> templateProcessors = new()
        {
            [DocumentationTemplate.Default] = new DefaultTemplateProcessor(htmlRenderer, availableLanguages, config.StaticPagesDirectory, config.Version)
            // #ADD_TEMPLATE: use the enum value together with the RazorTemplateProcessor with 8 type parameters, representing the templates
            //                additionally, pass the 'DocCommentHtmlConfiguration' or a custom configuration (if provided)
            //
            // Example:
            // [DocumentationTemplate.Custom] = RazorTemplateProcessor<
            //                                        CustomObjectTypePage,
            //                                        CustomDelegateTypePage,
            //                                        CustomEnumTypePage,
            //                                        CustomNamespacePage,
            //                                        CustomAssemblyPage,
            //                                        CustomApiHomePage,
            //                                        CustomStaticPage,
            //                                        CustomSearchPage
            //                                    >.With(
            //                                        new DocCommentHtmlConfiguration(), // if a custom configuration is provided, use CustomHtmlConfiguration.
            //                                        htmlRenderer,
            //                                        availableLanguages,
            //                                        config.StaticPagesDirectory,
            //                                        config.Version)
            //
            // #ADD_TEMPLATE_PROCESSOR: use the enum value together with the custom template processor
            //
            // Example:
            // [DocumentationTemplate.Liquid] = CustomLiquidTemplateProcessor(...args...)
            //
        };

        try
        {
            string[] assemblyPaths = AssemblyLocator.GetAssemblies(config.Input);
            string[] docPaths = [.. assemblyPaths.Select(p => Path.ChangeExtension(p, ".xml"))];

            var assemblyDataConfig = new AssemblyDataConfiguration(
                MinVisibility: config.MinVisibility,
                MemberInheritanceMode: config.MemberInheritance,
                AssembliesToExclude: config.ProjectsToExclude,
                NamespacesToExclude: config.NamespacesToExclude
                );

            if (config.Version is null && Directory.Exists(config.OutputDirectory)
                && Directory.EnumerateFileSystemEntries(config.OutputDirectory).Any()) // the output directory exists and it its not empty and the documentation is not versioned
            {
                if (config.ForceCreate)
                {
                    Directory.Delete(config.OutputDirectory, true); // delete the output directory
                }
                else
                {
                    throw new OutputDirectoryNotEmptyException(config.OutputDirectory); // throw an exception
                }
            }

            _ = Directory.CreateDirectory(config.OutputDirectory); // create the output directory

            var templateProcessor = templateProcessors[config.Template];

            var docGenerator = new DocGenerator(assemblyPaths, docPaths, templateProcessor, assemblyDataConfig, config.OutputDirectory, logger);
            docGenerator.GenerateDoc();

            Console.WriteLine($"Documentation generated in the '{config.OutputDirectory}' folder");
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

    /// <summary>
    /// Displays the help text on the command line.
    /// </summary>
    /// <param name="result">Command line parser result.</param>
    /// <returns>A completed task.</returns>
    private static Task DisplayHelp(ParserResult<CommandLineConfiguration> result)
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

        Console.WriteLine(helpText); // print the help

        return Task.CompletedTask;
    }
}
