using RefDocGen.AssemblyAnalysis;
using RefDocGen.DocExtraction;
using RefDocGen.TemplateGenerators;

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
    /// An instance used for generating the templates.
    /// </summary>
    private readonly ITemplateGenerator templateGenerator;

    /// <summary>
    /// Configuration describing what data should be extracted from the assemblies.
    /// </summary>
    private readonly AssemblyDataConfiguration assemblyDataConfiguration;

    /// <summary>
    /// Initialize a new instance of <see cref="DocGenerator"/> class.
    /// </summary>
    /// <param name="assemblyPaths">Paths to the DLL assemblies.</param>
    /// <param name="docXmlPaths">Path to the XML documentation files.</param>
    /// <param name="templateGenerator">An instance used for generating the templates</param>
    /// <param name="assemblyDataConfiguration">Configuration describing what data should be extracted from the assemblies.</param>
    public DocGenerator(IEnumerable<string> assemblyPaths, IEnumerable<string> docXmlPaths, ITemplateGenerator templateGenerator, AssemblyDataConfiguration assemblyDataConfiguration)
    {
        this.assemblyPaths = assemblyPaths;
        this.docXmlPaths = docXmlPaths;
        this.templateGenerator = templateGenerator;
        this.assemblyDataConfiguration = assemblyDataConfiguration;
    }

    /// <summary>
    /// Generates the reference documentation using the provided DLL and XML comments file.
    /// </summary>
    public void GenerateDoc()
    {
        var assemblyAnalyzer = new AssemblyTypeExtractor(assemblyPaths, assemblyDataConfiguration);
        var typeRegistry = assemblyAnalyzer.GetDeclaredTypes();

        var docCommentExtractor = new DocCommentExtractor(docXmlPaths, typeRegistry);
        docCommentExtractor.AddComments();

        templateGenerator.GenerateTemplates(typeRegistry);
    }
}
