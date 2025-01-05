using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RefDocGen.CodeElements.Abstract;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Delegate;
using RefDocGen.CodeElements.Abstract.Types.Enum;
using RefDocGen.TemplateGenerators.Default;
using RefDocGen.TemplateGenerators.Default.TemplateModelCreators;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Namespaces;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;

namespace RefDocGen.TemplateGenerators.Razor;

/// <summary>
/// Class used for generating RazorLight templates using the <see cref="ObjectTypeTM"/> as a type template model and <see cref="NamespaceTM"/> as a namespace template model.
/// </summary>
internal class RazorTemplateGenerator : ITemplateGenerator
{
    /// <summary>
    /// The directory, where the generated output will be stored.
    /// </summary>
    private readonly string outputDir;

    private HtmlRenderer htmlRenderer;

    /// <summary>
    /// Initialize a new instance of <see cref="DefaultTemplateGenerator"/> class.
    /// </summary>
    /// <param name="outputDir">The directory, where the generated output will be stored.</param>
    internal RazorTemplateGenerator(HtmlRenderer htmlRenderer, string outputDir)
    {
        this.outputDir = outputDir;
        this.htmlRenderer = htmlRenderer;
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
        GenerateTemplates<Templates.ObjectType, ObjectTypeTM>(typeTemplateModels);
    }

    /// <summary>
    /// Generate the templates representing the individual enum types.
    /// </summary>
    /// <param name="enums">The enum data to be used in the templates.</param>
    private void GenerateEnumTemplates(IEnumerable<IEnumTypeData> enums)
    {
        var enumTMs = enums.Select(EnumTMCreator.GetFrom);
        GenerateTemplates<Templates.EnumType, EnumTypeTM>(enumTMs);
    }

    /// <summary>
    /// Generate the templates representing the individual delegate types.
    /// </summary>
    /// <param name="delegates">The delegate data to be used in the templates.</param>
    private void GenerateDelegateTemplates(IEnumerable<IDelegateTypeData> delegates)
    {
        var delegateTMs = delegates.Select(DelegateTMCreator.GetFrom);
        GenerateTemplates<Templates.DelegateType, DelegateTypeTM>(delegateTMs);
    }

    /// <summary>
    /// Generate the templates for the namespaces (both namespace list and individual namespace details pages).
    /// </summary>
    /// <param name="types">The type data to be used in the templates.</param>
    private void GenerateNamespaceTemplates(ITypeRegistry types)
    {
        var namespaceTMs = NamespaceListTMCreator.GetFrom(types);

        // namespace list template
        GenerateTemplate<Templates.NamespaceList, IEnumerable<NamespaceTM>>(namespaceTMs, "index");

        // namespace detail templates
        GenerateTemplates<Templates.NamespaceDetail, NamespaceTM>(namespaceTMs);
    }

    /// <summary>
    /// Generate the templates of the selected kind, using the provided template models.
    /// </summary>
    /// <param name="templateModels">Template models used for the templates generation.</param>
    /// <param name="templateKind">Kind of the templates to generate.</param>
    private void GenerateTemplates<TTemplate, T>(IEnumerable<T> templateModels)
        where T : ITemplateModelWithId
        where TTemplate : IComponent
    {
        foreach (var tm in templateModels)
        {
            GenerateTemplate<TTemplate, T>(tm, tm.Id);
        }
    }

    /// <summary>
    /// Generate the template of the selected kind, using the provided template model.
    /// </summary>
    /// <param name="templateModel">Template model used for the template generation.</param>
    /// <param name="templateKind">Kind of the template to generate.</param>
    /// <param name="templateId">Id of the template to generate</param>
    private void GenerateTemplate<TTemplate, TTemplateModel>(TTemplateModel templateModel, string templateId)
        where TTemplate : IComponent
    {
        string outputFileName = Path.Join(outputDir, $"{templateId}.html");

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
