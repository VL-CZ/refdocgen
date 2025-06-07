using Microsoft.AspNetCore.Components.Web;
using RefDocGen.TemplateProcessors.Shared;
using RefDocGen.TemplateProcessors.Shared.DocComments.Html;
using RefDocGen.TemplateProcessors.Shared.Languages;
#pragma warning disable IDE0005 // add the namespace containing the Razor templates

#pragma warning restore IDE0005

namespace RefDocGen.TemplateProcessors.Default;

/// <summary>
/// Class used for processing the default Razor templates.
/// </summary>
internal class DefaultTemplateProcessor : RazorTemplateProcessor<
    ObjectTypeTemplate,
    DelegateTypeTemplate,
    EnumTypeTemplate,
    NamespaceTemplate,
    AssemblyTemplate,
    ApiTemplate,
    StaticPageTemplate,
    SearchTemplate>
{
    /// <summary>
    /// Initialize a new instance of <see cref="DefaultTemplateProcessor"/> class.
    /// </summary>
    /// <param name="htmlRenderer">Renderer of the Razor components.</param>
    /// <param name="staticPagesDirectory">Path to the directory containing the static pages created by user. <c>null</c> indicates that the directory is not specified.</param>
    /// <param name="docVersion">Version of the documentation (e.g. 'v1.0'). Pass <c>null</c> if no specific version should be generated.</param>
    /// <param name="availableLanguages">Configuration of languages available in the documentation.</param>
    internal DefaultTemplateProcessor(HtmlRenderer htmlRenderer, ILanguageConfiguration[] availableLanguages,
        string? staticPagesDirectory = null, string? docVersion = null)
        : base(
            htmlRenderer,
            new DocCommentTransformer(new DocCommentHtmlConfiguration()),
            availableLanguages,
            staticPagesDirectory,
            docVersion)
    {
    }
}
