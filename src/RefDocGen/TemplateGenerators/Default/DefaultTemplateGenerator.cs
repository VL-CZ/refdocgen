using Microsoft.AspNetCore.Components.Web;
using RefDocGen.TemplateGenerators.Shared;
using RefDocGen.TemplateGenerators.Shared.Tools.DocComments.Html;

#pragma warning disable IDE0005 // add the namespace containing the Razor templates
using RefDocGen.TemplateGenerators.Default.Templates;
#pragma warning restore IDE0005

namespace RefDocGen.TemplateGenerators.Default;

/// <summary>
/// Class used for generating default Razor templates.
/// </summary>
internal class DefaultTemplateGenerator : RazorTemplateGenerator<
    DelegateTypeTemplate,
    EnumTypeTemplate,
    NamespaceDetailTemplate,
    NamespaceListTemplate,
    ObjectTypeTemplate,
    Template>
{
    /// <summary>
    /// Initialize a new instance of <see cref="DefaultTemplateGenerator"/> class.
    /// </summary>
    /// <param name="htmlRenderer">Renderer of the Razor components.</param>
    /// <param name="outputDir">The directory, where the generated output will be stored.</param>
    /// <param name="staticPagesDirectory">Path to the directory containing the static pages created by user. <c>null</c> indicates that the directory is not specified.</param>
    internal DefaultTemplateGenerator(HtmlRenderer htmlRenderer, string outputDir, string? staticPagesDirectory = null)
        : base(
            htmlRenderer,
            new DefaultDocCommentTransformer(new DocCommentHtmlConfiguration()),
            outputDir,
            staticPagesDirectory)
    {
    }
}
