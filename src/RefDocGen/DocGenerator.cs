using RefDocGen.AssemblyAnalysis;
using RefDocGen.CodeElements;
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
    private readonly string[] assemblyPaths;

    /// <summary>
    /// Paths to the XML documentation files.
    /// </summary>
    private readonly string[] docXmlPaths;

    /// <summary>
    /// An instance used for generating the templates.
    /// </summary>
    private readonly ITemplateGenerator templateGenerator;

    /// <summary>
    /// Minimal visibility of the types and members to include.
    /// </summary>
    private readonly AccessModifier minVisibility;

    /// <summary>
    /// Specifies which inherited members should be included in types.
    /// </summary>
    private readonly MemberInheritanceMode memberInheritanceMode;

    /// <summary>
    /// Initialize a new instance of <see cref="DocGenerator"/> class.
    /// </summary>
    /// <param name="assemblyPaths">Paths to the DLL assemblies.</param>
    /// <param name="docXmlPaths">Path to the XML documentation files.</param>
    /// <param name="templateGenerator">An instance used for generating the templates</param>
    /// <param name="minVisibility">Minimal visibility of the types and members to include.</param>
    /// <param name="memberInheritanceMode">Specifies which inherited members should be included in types.</param>
    public DocGenerator(string[] assemblyPaths, string[] docXmlPaths, ITemplateGenerator templateGenerator, AccessModifier minVisibility, MemberInheritanceMode memberInheritanceMode)
    {
        this.assemblyPaths = assemblyPaths;
        this.docXmlPaths = docXmlPaths;
        this.templateGenerator = templateGenerator;
        this.minVisibility = minVisibility;
        this.memberInheritanceMode = memberInheritanceMode;
    }

    /// <summary>
    /// Generates the reference documentation using the provided DLL and XML comments file.
    /// </summary>
    public void GenerateDoc()
    {
        var assemblyAnalyzer = new AssemblyTypeExtractor(assemblyPaths, minVisibility, memberInheritanceMode);
        var typeRegistry = assemblyAnalyzer.GetDeclaredTypes();

        var docCommentExtractor = new DocCommentExtractor(docXmlPaths, typeRegistry);
        docCommentExtractor.AddComments();

        templateGenerator.GenerateTemplates(typeRegistry);
    }
}
