using RazorLight;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.TemplateGenerators.Default.TemplateModelCreators;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Namespaces;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;

namespace RefDocGen.TemplateGenerators.Default;

/// <summary>
/// Class used for generating RazorLight templates using the <see cref="TypeTM"/> as a type template model and <see cref="NamespaceTM"/> as a namespace template model.
/// </summary>
internal class DefaultTemplateGenerator : ITemplateGenerator
{
    /// <summary>
    /// Path to the Razor template representing a type, relative to <see cref="templatesFolderPath"/>.
    /// </summary>
    private const string typeTemplatePath = "TypeTemplate.cshtml";

    /// <summary>
    /// Path to the Razor template representing a namespace list, relative to <see cref="templatesFolderPath"/>.
    /// </summary>
    private const string namespaceListTemplatePath = "NamespaceListTemplate.cshtml";

    /// <summary>
    /// Path to the Razor template representing a namespace detail, relative to <see cref="templatesFolderPath"/>.
    /// </summary>
    private const string namespaceDetailTemplatePath = "NamespaceDetailTemplate.cshtml";

    /// <summary>
    /// Path to the folder containing Razor templates.
    /// </summary>
    private readonly string templatesFolderPath;

    /// <summary>
    /// The directory, where the generated output will be stored.
    /// </summary>
    private readonly string outputDir;

    /// <summary>
    /// RazorLight engine used for generating the templates.
    /// </summary>
    private readonly RazorLightEngine razorLightEngine;

    /// <summary>
    /// Initialize a new instance of <see cref="DefaultTemplateGenerator"/> class.
    /// </summary>
    /// <param name="projectPath">Path to the project root directory.</param>
    /// <param name="templatesFolderPath">Path to the folder containing Razor templates.</param>
    /// <param name="outputDir">RazorLight engine used for generating the templates.</param>
    internal DefaultTemplateGenerator(string projectPath, string templatesFolderPath, string outputDir)
    {
        this.templatesFolderPath = templatesFolderPath;
        this.outputDir = outputDir;

        razorLightEngine = new RazorLightEngineBuilder()
            .UseFileSystemProject(projectPath)
            .UseMemoryCachingProvider()
            .Build();
    }

    /// <inheritdoc/>
    public void GenerateTemplates(ITypeDeclarations typeDeclarations)
    {
        GenerateTypeTemplates(typeDeclarations.Types);
        GenerateEnumTemplates(typeDeclarations.Enums);
        GenerateNamespaceTemplates(typeDeclarations);
    }

    /// <summary>
    /// Generate the templates representing the individual types.
    /// </summary>
    /// <param name="types">The type data to be used in the templates.</param>
    private void GenerateTypeTemplates(IEnumerable<ITypeData> types)
    {
        var typeTemplateModels = types.Select(TypeTMCreator.GetFrom);

        foreach (var model in typeTemplateModels)
        {
            string outputFileName = Path.Join(outputDir, $"{model.Id}.html");
            string templatePath = Path.Join(templatesFolderPath, typeTemplatePath);

            var task = razorLightEngine.CompileRenderAsync(templatePath, model);
            //task.Wait(); // TODO: consider using Async
            string result = task.Result;

            File.WriteAllText(outputFileName, result);
        }
    }

    private void GenerateEnumTemplates(IEnumerable<IEnumData> enums)
    {
        var typeTemplateModels = enums.Select(EnumTMCreator.GetFrom);

        foreach (var model in typeTemplateModels)
        {
            string outputFileName = Path.Join(outputDir, $"{model.Id}.html");
            string templatePath = Path.Join(templatesFolderPath, typeTemplatePath).Replace("Type", "Enum");

            var task = razorLightEngine.CompileRenderAsync(templatePath, model);
            //task.Wait(); // TODO: consider using Async
            string result = task.Result;

            File.WriteAllText(outputFileName, result);
        }
    }

    /// <summary>
    /// Generate the templates for the namespaces (both namespace list and individual namespace details pages).
    /// </summary>
    /// <param name="types">The type data to be used in the templates.</param>
    private void GenerateNamespaceTemplates(ITypeDeclarations types)
    {
        var namespaceTMs = NamespaceListTMCreator.GetFrom(types);

        GenerateNamespaceListTemplate(namespaceTMs);

        foreach (var namespaceTM in namespaceTMs)
        {
            GenerateNamespaceDetailTemplate(namespaceTM);
        }
    }

    /// <summary>
    /// Generate the template containing the list of namespaces of a program.
    /// </summary>
    /// <param name="namespaceTMs">The namespace template models to be used in the template.</param>
    private void GenerateNamespaceListTemplate(IEnumerable<NamespaceTM> namespaceTMs)
    {
        string outputFileName = Path.Join(outputDir, "index.html");
        string templatePath = Path.Join(templatesFolderPath, namespaceListTemplatePath);

        var task = razorLightEngine.CompileRenderAsync(templatePath, namespaceTMs);
        string result = task.Result;

        File.WriteAllText(outputFileName, result);
    }

    /// <summary>
    /// Generate the template containing the given namespace detail.
    /// </summary>
    /// <param name="namespaceTM">Template model of the given namespace.</param>
    private void GenerateNamespaceDetailTemplate(NamespaceTM namespaceTM)
    {
        string outputFileName = Path.Join(outputDir, $"{namespaceTM.Id}.html");
        string templatePath = Path.Join(templatesFolderPath, namespaceDetailTemplatePath);

        var task = razorLightEngine.CompileRenderAsync(templatePath, namespaceTM);
        string result = task.Result;

        File.WriteAllText(outputFileName, result);
    }
}
