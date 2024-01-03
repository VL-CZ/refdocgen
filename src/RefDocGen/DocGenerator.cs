namespace RefDocGen;

public class DocGenerator
{
    private readonly string assemblyPath;
    private readonly string docXmlPath;

    public DocGenerator(string assemblyPath, string docXmlPath)
    {
        this.assemblyPath = assemblyPath;
        this.docXmlPath = docXmlPath;
    }

    public void GenerateDoc()
    {
        var assemblyAnalyzer = new AssemblyAnalyzer(assemblyPath);
        var types = assemblyAnalyzer.GetTemplateModels();

        var docCommentExtractor = new DocCommentExtractor(docXmlPath, types);
        var templateModels = docCommentExtractor.GetTemplateModels();

        var templateGenerator = new TemplateGenerator();
        templateGenerator.GenerateTemplates(templateModels);
    }
}
