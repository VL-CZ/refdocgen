using RazorLight;
using RefDocGen.CodeElements.Abstract;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Delegate;
using RefDocGen.CodeElements.Abstract.Types.Enum;
using RefDocGen.TemplateGenerators.Default.TemplateModelCreators;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Namespaces;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Default.Templates;

namespace RefDocGen.TemplateGenerators.Default;

/// <summary>
/// Class used for generating RazorLight templates using the <see cref="ObjectTypeTM"/> as a type template model and <see cref="NamespaceTM"/> as a namespace template model.
/// </summary>
internal class DefaultTemplateGenerator : ITemplateGenerator
{
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
    public void GenerateTemplates(ITypeRegistry typeRegistry)
    {
        GenerateObjectTypeTemplates(typeRegistry.ObjectTypes);
        GenerateEnumTemplates(typeRegistry.Enums);
        GenerateDelegateTemplates(typeRegistry.Delegates);
        GenerateNamespaceTemplates(typeRegistry);
    }

    /// <summary>
    /// Generate the templates representing the individual object types.
    /// </summary>
    /// <param name="types">The type data to be used in the templates.</param>
    private void GenerateObjectTypeTemplates(IEnumerable<IObjectTypeData> types)
    {
        var typeTemplateModels = types.Select(ObjectTypeTMCreator.GetFrom);
        GenerateTemplates(typeTemplateModels, TemplateKind.ObjectType);
    }

    /// <summary>
    /// Generate the templates representing the individual enum types.
    /// </summary>
    /// <param name="enums">The enum data to be used in the templates.</param>
    private void GenerateEnumTemplates(IEnumerable<IEnumTypeData> enums)
    {
        var enumTMs = enums.Select(EnumTMCreator.GetFrom);
        GenerateTemplates(enumTMs, TemplateKind.EnumType);
    }

    /// <summary>
    /// Generate the templates representing the individual delegate types.
    /// </summary>
    /// <param name="delegates">The delegate data to be used in the templates.</param>
    private void GenerateDelegateTemplates(IEnumerable<IDelegateTypeData> delegates)
    {
        var delegateTMs = delegates.Select(DelegateTMCreator.GetFrom);
        GenerateTemplates(delegateTMs, TemplateKind.DelegateType);
    }

    /// <summary>
    /// Generate the templates for the namespaces (both namespace list and individual namespace details pages).
    /// </summary>
    /// <param name="types">The type data to be used in the templates.</param>
    private void GenerateNamespaceTemplates(ITypeRegistry types)
    {
        var namespaceTMs = NamespaceListTMCreator.GetFrom(types);

        // namespace list template
        GenerateTemplate(namespaceTMs, TemplateKind.NamespaceList, "index");

        // namespace detail templates
        GenerateTemplates(namespaceTMs, TemplateKind.NamespaceDetail);
    }

    /// <summary>
    /// Generate the templates of the selected kind, using the provided template models.
    /// </summary>
    /// <param name="templateModels">Template models used for the templates generation.</param>
    /// <param name="templateKind">Kind of the templates to generate.</param>
    private void GenerateTemplates<T>(IEnumerable<T> templateModels, TemplateKind templateKind) where T : ITemplateModelWithId
    {
        foreach (var tm in templateModels)
        {
            GenerateTemplate(tm, templateKind, tm.Id);
        }
    }

    /// <summary>
    /// Generate the template of the selected kind, using the provided template model.
    /// </summary>
    /// <param name="templateModel">Template model used for the template generation.</param>
    /// <param name="templateKind">Kind of the template to generate.</param>
    /// <param name="templateId">Id of the template to generate</param>
    private void GenerateTemplate<T>(T templateModel, TemplateKind templateKind, string templateId)
    {
        string outputFileName = Path.Join(outputDir, $"{templateId}.html");
        string templatePath = Path.Join(templatesFolderPath, templateKind.GetFileName());

        var task = razorLightEngine.CompileRenderAsync(templatePath, templateModel);
        //task.Wait(); // TODO: consider using Async

        string result = task.Result;

        File.WriteAllText(outputFileName, result);

    }
}
