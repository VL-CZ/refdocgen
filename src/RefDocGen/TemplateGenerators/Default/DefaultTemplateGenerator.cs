using Microsoft.AspNetCore.Components.Web;
using RefDocGen.TemplateGenerators.Default.Templates;
using RefDocGen.TemplateGenerators.Shared;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Namespaces;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Shared.Tools.DocComments.Html;

namespace RefDocGen.TemplateGenerators.Default;

/// <summary>
/// Class used for generating Razor templates using the <see cref="ObjectTypeTM"/> as a type template model and <see cref="NamespaceTM"/> as a namespace template model.
/// </summary>
internal class DefaultTemplateGenerator : RazorTemplateGenerator<
    DelegateTypeTemplate,
    EnumTypeTemplate,
    NamespaceDetailTemplate,
    NamespaceListTemplate,
    ObjectTypeTemplate>
{
    /// <summary>
    /// Initialize a new instance of <see cref="DefaultTemplateGenerator"/> class.
    /// </summary>
    /// <param name="htmlRenderer">Renderer of the Razor components.</param>
    /// <param name="docCommentTransformer">Transformer of the XML doc comments into HTML.</param>
    /// <param name="outputDir">The directory, where the generated output will be stored.</param>
    internal DefaultTemplateGenerator(
        HtmlRenderer htmlRenderer,
        IDocCommentTransformer docCommentTransformer,
        string outputDir) : base(htmlRenderer, docCommentTransformer, outputDir, "Default/Templates")
    {
    }
}
