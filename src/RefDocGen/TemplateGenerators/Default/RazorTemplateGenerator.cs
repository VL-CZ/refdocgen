using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RefDocGen.CodeElements.Abstract;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Delegate;
using RefDocGen.CodeElements.Abstract.Types.Enum;
using RefDocGen.TemplateGenerators.Default.TemplateModelCreators;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Namespaces;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Tools.DocComments.Html;

namespace RefDocGen.TemplateGenerators.Default;

/// <summary>
/// Class used for generating Razor templates using the <see cref="ObjectTypeTM"/> as a type template model and <see cref="NamespaceTM"/> as a namespace template model.
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
    /// The directory, where the generated output will be stored.
    /// </summary>
    private readonly string outputDir;

    /// <summary>
    /// Rendered of the Razor components.
    /// </summary>
    private readonly HtmlRenderer htmlRenderer;

    /// <summary>
    /// Transformer of the XML doc comments into HTML.
    /// </summary>
    private readonly IDocCommentTransformer docCommentTransformer;

    /// <summary>
    /// Initialize a new instance of <see cref="RazorTemplateGenerator{TDelegateTemplate, TEnumTemplate, TNamespaceDetailTemplate, TNamespaceListTemplate, TObjectTypeTemplate}"/> class.
    /// </summary>
    /// <param name="htmlRenderer">Renderer of the Razor components.</param>
    /// <param name="docCommentTransformer">Transformer of the XML doc comments into HTML.</param>
    /// <param name="outputDir">The directory, where the generated output will be stored.</param>
    internal RazorTemplateGenerator(HtmlRenderer htmlRenderer, IDocCommentTransformer docCommentTransformer, string outputDir)
    {
        this.outputDir = outputDir;
        this.htmlRenderer = htmlRenderer;
        this.docCommentTransformer = docCommentTransformer;
    }

    /// <inheritdoc/>
    public void GenerateTemplates(ITypeRegistry typeRegistry)
    {
        docCommentTransformer.TypeRegistry = typeRegistry;

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
        GenerateTemplate<TNamespaceListTemplate, IEnumerable<NamespaceTM>>(namespaceTMs, "index");

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
        string outputFileName = Path.Join(outputDir, $"{outputFile}.html");

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
}
