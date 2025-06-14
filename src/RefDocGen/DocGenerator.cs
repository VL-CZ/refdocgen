using Microsoft.Extensions.Logging;
using RefDocGen.AssemblyAnalysis;
using RefDocGen.DocExtraction;
using RefDocGen.TemplateProcessors;

namespace RefDocGen;

/// <summary>
/// Class responsible for generating the reference documentation using the provided DLL and XML doc comments file.
/// </summary>
public class DocGenerator
{
    /// <summary>
    /// Paths to the DLL assemblies.
    /// </summary>
    private readonly IEnumerable<string> assemblyPaths;

    /// <summary>
    /// Paths to the XML documentation files.
    /// </summary>
    private readonly IEnumerable<string> docXmlPaths;

    /// <summary>
    /// An instance used for processing the templates.
    /// </summary>
    private readonly ITemplateProcessor templateProcessor;

    /// <summary>
    /// The directory, where the generated output will be stored.
    /// </summary>
    private readonly string outputDirectory;

    /// <summary>
    /// Configuration describing what data should be extracted from the assemblies.
    /// </summary>
    private readonly AssemblyDataConfiguration assemblyDataConfiguration;

    /// <summary>
    /// A logger instance.
    /// </summary>
    private readonly ILogger logger;

    /// <summary>
    /// Initialize a new instance of <see cref="DocGenerator"/> class.
    /// </summary>
    /// <param name="assemblyPaths">Paths to the DLL assemblies.</param>
    /// <param name="docXmlPaths">Path to the XML documentation files.</param>
    /// <param name="templateProcessor">An instance used for processing the templates.</param>
    /// <param name="assemblyDataConfiguration">Configuration describing what data should be extracted from the assemblies.</param>
    /// <param name="outputDirectory">The directory, where the generated output will be stored.</param>
    /// <param name="logger">A logger instance.</param>
    public DocGenerator(IEnumerable<string> assemblyPaths, IEnumerable<string> docXmlPaths, ITemplateProcessor templateProcessor,
        AssemblyDataConfiguration assemblyDataConfiguration, string outputDirectory, ILogger logger)
    {
        this.assemblyPaths = assemblyPaths;
        this.docXmlPaths = docXmlPaths;
        this.templateProcessor = templateProcessor;
        this.assemblyDataConfiguration = assemblyDataConfiguration;
        this.outputDirectory = outputDirectory;
        this.logger = logger;
    }

    /// <summary>
    /// Generates the reference documentation using the provided DLL and XML comments file.
    /// </summary>
    public void GenerateDoc()
    {
        var assemblyAnalyzer = new AssemblyTypeExtractor(assemblyPaths, assemblyDataConfiguration, logger);
        var typeRegistry = assemblyAnalyzer.GetDeclaredTypes();

        var docCommentExtractor = new DocCommentExtractor(docXmlPaths, typeRegistry, logger);
        docCommentExtractor.AddComments();

        templateProcessor.ProcessTemplates(typeRegistry, outputDirectory, logger);
    }
}
