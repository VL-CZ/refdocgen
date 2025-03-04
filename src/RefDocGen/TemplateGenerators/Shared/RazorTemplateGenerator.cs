using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RefDocGen.CodeElements.Abstract;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Delegate;
using RefDocGen.CodeElements.Abstract.Types.Enum;
using RefDocGen.TemplateGenerators.Default.Templates;
using RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Namespaces;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Shared.Tools.DocComments.Html;
using RefDocGen.Tools;

namespace RefDocGen.TemplateGenerators.Shared;

/// <summary>
/// Class responsible for generating the Razor templates and populating them with the type data.
/// </summary>
/// <typeparam name="TDelegateTemplate">Type of the Razor component representing the delegate type.</typeparam>
/// <typeparam name="TEnumTemplate">Type of the Razor component representing the enum type.</typeparam>
/// <typeparam name="TNamespaceDetailTemplate">Type of the Razor component representing the namespace detail.</typeparam>
/// <typeparam name="TNamespaceListTemplate">Type of the Razor component representing the namespace list.</typeparam>
/// <typeparam name="TObjectTypeTemplate">Type of the Razor component representing the object type.</typeparam>
internal class RazorTemplateGenerator<
        TDelegateTemplate,
        TEnumTemplate,
        TNamespaceDetailTemplate,
        TNamespaceListTemplate,
        TObjectTypeTemplate
    > : ITemplateGenerator

    where TDelegateTemplate : IComponent
    where TEnumTemplate : IComponent
    where TNamespaceDetailTemplate : IComponent
    where TNamespaceListTemplate : IComponent
    where TObjectTypeTemplate : IComponent
{
    /// <summary>
    /// Namespace prefix of any template generator.
    /// </summary>
    private const string templateGeneratorsNsPrefix = "RefDocGen.TemplateGenerators.";

    /// <summary>
    /// Identifier of the <c>index</c> page.
    /// </summary>
    private const string indexPageId = "api";

    /// <summary>
    /// The directory, where the generated output will be stored.
    /// </summary>
    internal readonly string outputDirectory;

    /// <summary>
    /// Rendered of the Razor components.
    /// </summary>
    private readonly HtmlRenderer htmlRenderer;

    /// <summary>
    /// Path to the directory containing the Razor templates, relative to <c>TemplateGenerators</c> folder.
    /// </summary>
    private readonly string templatesDirectory;

    /// <summary>
    /// Path to the directory containing static files (typically css and js related to templates), relative to <see cref="templatesDirectory"/>.
    /// </summary>
    private const string staticFilesDirectory = "Static";

    /// <summary>
    /// Transformer of the XML doc comments into HTML.
    /// </summary>
    private readonly IDocCommentTransformer docCommentTransformer;

    /// <summary>
    /// Initialize a new instance of <see cref="RazorTemplateGenerator{TDelegateTemplate, TEnumTemplate, TNamespaceDetailTemplate, TNamespaceListTemplate, TObjectTypeTemplate}"/> class.
    /// </summary>
    /// <param name="htmlRenderer">Renderer of the Razor components.</param>
    /// <param name="docCommentTransformer">Transformer of the XML doc comments into HTML.</param>
    /// <param name="outputDirectory">The directory, where the generated output will be stored.</param>
    internal RazorTemplateGenerator(
        HtmlRenderer htmlRenderer,
        IDocCommentTransformer docCommentTransformer,
        string outputDirectory)
    {
        this.outputDirectory = outputDirectory;
        this.htmlRenderer = htmlRenderer;
        this.docCommentTransformer = docCommentTransformer;

        templatesDirectory = GetTemplatesDirectory();
    }

    /// <inheritdoc/>
    public void GenerateTemplates(ITypeRegistry typeRegistry)
    {
        docCommentTransformer.TypeRegistry = typeRegistry;

        GenerateObjectTypeTemplates(typeRegistry.ObjectTypes);
        GenerateEnumTemplates(typeRegistry.Enums);
        GenerateDelegateTemplates(typeRegistry.Delegates);
        GenerateNamespaceTemplates(typeRegistry);

        CopyStaticFilesDirectory();

        CopyStaticPages();
    }

    /// <summary>
    /// Generate the templates representing the individual object types.
    /// </summary>
    /// <param name="types">The type data to be used in the templates.</param>
    private void GenerateObjectTypeTemplates(IEnumerable<IObjectTypeData> types)
    {
        var creator = new ObjectTypeTMCreator(docCommentTransformer);
        var typeTemplateModels = types.Select(creator.GetFrom);
        GenerateTemplates<TObjectTypeTemplate, ObjectTypeTM>(typeTemplateModels);
    }

    /// <summary>
    /// Generate the templates representing the individual enum types.
    /// </summary>
    /// <param name="enums">The enum data to be used in the templates.</param>
    private void GenerateEnumTemplates(IEnumerable<IEnumTypeData> enums)
    {
        var creator = new EnumTMCreator(docCommentTransformer);
        var enumTMs = enums.Select(creator.GetFrom);
        GenerateTemplates<TEnumTemplate, EnumTypeTM>(enumTMs);
    }

    /// <summary>
    /// Generate the templates representing the individual delegate types.
    /// </summary>
    /// <param name="delegates">The delegate data to be used in the templates.</param>
    private void GenerateDelegateTemplates(IEnumerable<IDelegateTypeData> delegates)
    {
        var creator = new DelegateTMCreator(docCommentTransformer);
        var delegateTMs = delegates.Select(creator.GetFrom);
        GenerateTemplates<TDelegateTemplate, DelegateTypeTM>(delegateTMs);
    }

    /// <summary>
    /// Generate the templates for the namespaces (both namespace list and individual namespace details pages).
    /// </summary>
    /// <param name="types">The type data to be used in the templates.</param>
    private void GenerateNamespaceTemplates(ITypeRegistry types)
    {
        var namespaceTMs = NamespaceListTMCreator.GetFrom(types);

        // namespace list template
        GenerateTemplate<TNamespaceListTemplate, IEnumerable<NamespaceTM>>(namespaceTMs, indexPageId);

        // namespace detail templates
        GenerateTemplates<TNamespaceDetailTemplate, NamespaceTM>(namespaceTMs);
    }

    /// <summary>
    /// Generate the given templates using the provided template models.
    /// </summary>
    /// <param name="templateModels">Template models used for the templates generation.</param>
    /// <typeparam name="TTemplateModel">Type of the template model to be used in the template.</typeparam>
    /// <typeparam name="TTemplate">Type of the Razor component representing the template to generate.</typeparam>
    private void GenerateTemplates<TTemplate, TTemplateModel>(IEnumerable<TTemplateModel> templateModels)
        where TTemplate : IComponent
        where TTemplateModel : ITemplateModelWithId
    {
        foreach (var tm in templateModels)
        {
            GenerateTemplate<TTemplate, TTemplateModel>(tm, tm.Id);
        }
    }

    /// <summary>
    /// Generate the given template using the provided template model.
    /// </summary>
    /// <param name="templateModel">Template model used for the template generation.</param>
    /// <param name="outputFile">Name of the output file containing the generated template populated with the <paramref name="templateModel"/> data.</param>
    /// <typeparam name="TTemplateModel">Type of the template model to be used in the template.</typeparam>
    /// <typeparam name="TTemplate">Type of the Razor component representing the template to generate.</typeparam>
    private void GenerateTemplate<TTemplate, TTemplateModel>(TTemplateModel templateModel, string outputFile)
        where TTemplate : IComponent
    {
        string outputFileName = Path.Join(outputDirectory, $"{outputFile}.html");

        string html = htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var paramDictionary = new Dictionary<string, object?>()
            {
                ["Model"] = templateModel
            };

            var parameters = ParameterView.FromDictionary(paramDictionary);
            var output = await htmlRenderer.RenderComponentAsync<TTemplate>(parameters);

            return output.ToHtmlString();
        }).Result;


        File.WriteAllText(outputFileName, html);
    }

    /// <summary>
    /// Copies the directory containing static files (css, js, etc.) to the output directory.
    /// </summary>
    private void CopyStaticFilesDirectory()
    {
        var staticFilesDir = new DirectoryInfo(Path.Combine(templatesDirectory, staticFilesDirectory));
        string outputDirPath = Path.Combine(outputDirectory, staticFilesDirectory);

        if (staticFilesDir.Exists)
        {
            staticFilesDir.CopyTo(outputDirPath, true);
        }
        else
        {
            // TODO: log static files dir not found
        }
    }

    /// <summary>
    /// Returns base directory containing the Razor templates and static files.
    /// </summary>
    /// <returns>Path to the directory containing the Razor templates and static files.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown in 2 cases
    /// <list type="bullet">
    /// <item>The templates aren't stored under <c>RefDocGen/TemplateGenerators</c> directory.</item>
    /// <item>The templates (i.e. the generic type parameters) aren't stored in the same directory.</item>
    /// </list>
    /// </exception>
    private string GetTemplatesDirectory()
    {
        const string baseFolder = "TemplateGenerators";

        Type[] templateTypes = [
            typeof(TDelegateTemplate),
            typeof(TEnumTemplate),
            typeof(TNamespaceDetailTemplate),
            typeof(TNamespaceListTemplate),
            typeof(TObjectTypeTemplate)
        ];

        string? templatesNs = typeof(TObjectTypeTemplate).Namespace ?? "";

        if (templateTypes.Any(t => t.Namespace != templatesNs))
        {
            throw new ArgumentException("Invalid configuration, all 5 templates must be in the same directory.");
        }

        if (!templatesNs.StartsWith(templateGeneratorsNsPrefix, StringComparison.Ordinal))
        {
            throw new ArgumentException("Invalid configuration, the templates must be stored in a folder contained in 'RefDocGen/TemplateGenerators' directory.");
        }

        string relativeTemplateNs = templatesNs[templateGeneratorsNsPrefix.Length..];

        string[] templatePathFragments = [baseFolder, .. relativeTemplateNs.Split('.')];
        return Path.Combine(templatePathFragments);
    }

    private void CopyStaticPages()
    {
        var staticFilesFolder = "C:\\Users\\vojta\\UK\\mgr-thesis\\refdocgen\\demo-lib\\pages";

        foreach (var file in Directory.GetFiles(staticFilesFolder))
        {
            var fileText = File.ReadAllText(file);
            if (file.EndsWith(".md"))
            {
                var md = File.ReadAllText(file);
                fileText = Markdown.ToHtml(md);
            }

            string outputFileName = Path.Join(outputDirectory, new FileInfo(file).Name.Split('.').First() + ".html");

            string html = htmlRenderer.Dispatcher.InvokeAsync(async () =>
            {
                var paramDictionary = new Dictionary<string, object?>()
                {
                    ["Contents"] = fileText
                };

                var parameters = ParameterView.FromDictionary(paramDictionary);
                var output = await htmlRenderer.RenderComponentAsync<Template>(parameters);

                return output.ToHtmlString();
            }).Result;


            File.WriteAllText(outputFileName, html);
        }
    }
}
