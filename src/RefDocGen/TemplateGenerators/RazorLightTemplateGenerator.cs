using RazorLight;
using RefDocGen.CodeElements.Abstract.Types;

namespace RefDocGen.TemplateGenerators;

/// <summary>
/// An abstract class used for generating any templates using the RazorLight library (for further info, see <see href="https://github.com/toddams/RazorLight"/>).
/// <para>
///     Template models are of a user-defined type, and constructed from <see cref="ITypeData"/> objects
/// </para>
/// </summary>
/// <typeparam name="TTypeTM">Type of the template model representing a type.</typeparam>
/// <typeparam name="TNamespaceTM">Type of the template model representing a namespace.</typeparam>
internal abstract class RazorLightTemplateGenerator<TTypeTM, TNamespaceTM> : ITemplateGenerator where TTypeTM : ITemplateModelWithId
{
    /// <summary>
    /// Default path to the Razor template representing a type, relative to <see cref="templatesFolderPath"/>.
    /// </summary>
    internal const string typeTemplateDefaultPath = "Template.cshtml";

    /// <summary>
    /// Default path to the Razor template representing a namespace list, relative to <see cref="templatesFolderPath"/>.
    /// </summary>
    internal const string namespaceListTemplateDefaultPath = "TemplateNamespace.cshtml";

    /// <summary>
    /// Path to the folder containing Razor templates.
    /// </summary>
    private readonly string templatesFolderPath;

    /// <summary>
    /// Path to the Razor template representing a type, relative to <see cref="templatesFolderPath"/>.
    /// </summary>
    private readonly string typeTemplatePath;

    /// <summary>
    /// Path to the Razor template representing a namespace list, relative to <see cref="templatesFolderPath"/>.
    /// </summary>
    private readonly string namespaceListTemplatePath;

    /// <summary>
    /// The directory, where the generated output will be stored.
    /// </summary>
    private readonly string outputDir;

    /// <summary>
    /// RazorLight engine used for generating the templates.
    /// </summary>
    private readonly RazorLightEngine razorLightEngine;

    /// <summary>
    /// Initialize a new instance of <see cref="RazorLightTemplateGenerator{T, T2}"/> class.
    /// </summary>
    /// <param name="projectPath">Path to the project root directory.</param>
    /// <param name="templatesFolderPath">Path to the folder containing Razor templates.</param>
    /// <param name="outputDir">RazorLight engine used for generating the templates.</param>
    /// <param name="typeTemplatePath">Path to the Razor template representing a type, relative to <paramref name="templatesFolderPath"/>.</param>
    /// <param name="namespaceListTemplatePath">Path to the Razor template representing a namespace list, relative to <paramref name="templatesFolderPath"/></param>
    protected RazorLightTemplateGenerator(string projectPath, string templatesFolderPath, string outputDir,
        string typeTemplatePath = typeTemplateDefaultPath, string namespaceListTemplatePath = namespaceListTemplateDefaultPath)
    {
        this.templatesFolderPath = templatesFolderPath;
        this.outputDir = outputDir;

        this.typeTemplatePath = typeTemplatePath;
        this.namespaceListTemplatePath = namespaceListTemplatePath;

        razorLightEngine = new RazorLightEngineBuilder()
            .UseFileSystemProject(projectPath)
            .UseMemoryCachingProvider()
            .Build();
    }

    /// <inheritdoc/>
    public void GenerateTemplates(IReadOnlyList<ITypeData> types)
    {
        GenerateTypeTemplates(types);
        GenerateNamespaceListTemplate(types);
    }

    /// <summary>
    /// Generate the templates representing the individual types.
    /// </summary>
    /// <param name="types">The type data to be used in the templates.</param>
    protected void GenerateTypeTemplates(IReadOnlyList<ITypeData> types)
    {
        var typeTemplateModels = GetTypeTemplateModels(types);

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

    /// <summary>
    /// Generate the template containing the list of namespaces of a program.
    /// </summary>
    /// <param name="types">The type data to be used in the templates.</param>
    protected void GenerateNamespaceListTemplate(IReadOnlyList<ITypeData> types)
    {
        var namespaceModels = GetNamespaceTemplateModels(types);

        string outputFileName = Path.Join(outputDir, $"index.html");
        string templatePath = Path.Join(templatesFolderPath, namespaceListTemplatePath);

        var task = razorLightEngine.CompileRenderAsync(templatePath, namespaceModels);
        string result = task.Result;

        File.WriteAllText(outputFileName, result);
    }

    /// <summary>
    /// Get template models representing the types based on the provided provided <paramref name="types"/>.
    /// <para>
    /// These template models are intended to be passed to the Razor templatess.
    /// </para>
    /// </summary>
    /// <param name="types"><see cref="ITypeData"/> objects representing the types.</param>
    /// <returns>A collection of template models, representing the types, that will be passed to the Razor templates.</returns>
    protected abstract IEnumerable<TTypeTM> GetTypeTemplateModels(IReadOnlyList<ITypeData> types);

    /// <summary>
    /// Get template models representing the namespaces based on the provided provided <paramref name="types"/>.
    /// <para>
    /// These template models are intended to be passed to the Razor templatess.
    /// </para>
    /// </summary>
    /// <param name="types"><see cref="ITypeData"/> objects representing the types.</param>
    /// <returns>A collection of template models, representing the namespaces, that will be passed to the Razor templates.</returns>
    protected abstract IEnumerable<TNamespaceTM> GetNamespaceTemplateModels(IReadOnlyList<ITypeData> types);
}
