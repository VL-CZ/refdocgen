using RefDocGen.AssemblyAnalysis;
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

        // var templateModels = types.Select(templateModelBuilder.CreateClassTemplateModel).ToArray();

        // var docCommentExtractor = new DocCommentExtractor(docXmlPath, templateModels);
        // templateModels = docCommentExtractor.GetTemplateModels();

        templateGenerator.GenerateTemplates(types);
    }
}
