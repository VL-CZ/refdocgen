using RefDocGen.AssemblyAnalysis;
using RefDocGen.DocExtraction;
using RefDocGen.TemplateGenerators;

namespace RefDocGen;

public class DocGenerator
{
    private readonly string assemblyPath;
    private readonly string docXmlPath;

    private readonly ITemplateGenerator templateGenerator;

    public DocGenerator(string assemblyPath, string docXmlPath, ITemplateGenerator templateGenerator)
    {
        this.assemblyPath = assemblyPath;
        this.docXmlPath = docXmlPath;
        this.templateGenerator = templateGenerator;
    }

    public void GenerateDoc()
    {
        var assemblyAnalyzer = new AssemblyTypeExtractor(assemblyPath);
        var types = assemblyAnalyzer.GetDeclaredClasses();

        var docCommentExtractor = new DocCommentExtractor(docXmlPath, types);
        types = docCommentExtractor.ExtractComments();

        templateGenerator.GenerateTemplates(types);
    }
}
