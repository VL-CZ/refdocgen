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
    /// Path to the DLL assembly.
    /// </summary>
    private readonly string assemblyPath;

    /// <summary>
    /// Path to the XML documentation file.
    /// </summary>
    private readonly string docXmlPath;

    /// <summary>
    /// An instance used for generating the templates.
    /// </summary>
    private readonly ITemplateGenerator templateGenerator;

    /// <summary>
    /// Initialize a new instance of <see cref="DocGenerator"/> class.
    /// </summary>
    /// <param name="assemblyPath">Path to the DLL assembly.</param>
    /// <param name="docXmlPath">Path to the XML documentation file.</param>
    /// <param name="templateGenerator">An instance used for generating the templates</param>
    public DocGenerator(string assemblyPath, string docXmlPath, ITemplateGenerator templateGenerator)
    {
        this.assemblyPath = assemblyPath;
        this.docXmlPath = docXmlPath;
        this.templateGenerator = templateGenerator;
    }

    /// <summary>
    /// Generates the reference documentation using the provided DLL and XML comments file.
    /// </summary>
    public void GenerateDoc()
    {
        var assemblyAnalyzer = new AssemblyTypeExtractor(assemblyPath);
        var types = assemblyAnalyzer.GetDeclaredTypes();

        var docCommentExtractor = new DocCommentExtractor(docXmlPath, types);
        docCommentExtractor.AddComments();

        templateGenerator.GenerateTemplates([.. types.Values]);
    }
}
