using RefDocGen.AssemblyAnalysis;
using RefDocGen.TemplateModels.Builders;

namespace RefDocGen;

public class DocGenerator
{
    private readonly string assemblyPath;
    private readonly string docXmlPath;

    private readonly ITemplateModelBuilder templateModelBuilder;

    public DocGenerator(string assemblyPath, string docXmlPath)
    {
        this.assemblyPath = assemblyPath;
        this.docXmlPath = docXmlPath;
        templateModelBuilder = new CSharpTemplateModelBuilder();
    }

    public void GenerateDoc()
    {
        var assemblyAnalyzer = new AssemblyTypeExtractor(assemblyPath);
        var types = assemblyAnalyzer.GetDeclaredClasses();

        var templateModels = types.Select(templateModelBuilder.CreateClassTemplateModel).ToArray();

        var docCommentExtractor = new DocCommentExtractor(docXmlPath, templateModels);
        templateModels = docCommentExtractor.GetTemplateModels();

        var templateGenerator = new RazorTemplateGenerator();
        templateGenerator.GenerateTemplates(templateModels);
    }
}
