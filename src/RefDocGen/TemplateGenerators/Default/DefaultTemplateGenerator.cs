using Microsoft.AspNetCore.Components.Web;
using RefDocGen.TemplateGenerators.Default.Templates;
using RefDocGen.TemplateGenerators.Shared;
using RefDocGen.TemplateGenerators.Shared.Tools.DocComments.Html;

namespace RefDocGen.TemplateGenerators.Default;

/// <summary>
/// Class used for generating default Razor templates.
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
    /// <param name="outputDir">The directory, where the generated output will be stored.</param>
    internal DefaultTemplateGenerator(HtmlRenderer htmlRenderer, string outputDir)
        : base(
            htmlRenderer,
            new DefaultDocCommentTransformer(new DocCommentHtmlConfiguration()),
            outputDir)
    {
    }
}
