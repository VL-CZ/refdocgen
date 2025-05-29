using Microsoft.AspNetCore.Components.Web;
using RefDocGen.TemplateGenerators.Shared;
using RefDocGen.TemplateGenerators.Shared.DocComments.Html;

#pragma warning disable IDE0005 // add the namespace containing the Razor templates
using RefDocGen.TemplateGenerators.Default.Templates;
using RefDocGen.TemplateGenerators.Shared.Languages;
#pragma warning restore IDE0005

namespace RefDocGen.TemplateGenerators.Default;

/// <summary>
/// Class used for generating default Razor templates.
/// </summary>
internal class DefaultTemplateGenerator : RazorTemplateGenerator<
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
    /// Languages available in the documentation.
    /// </summary>
    private static readonly ILanguageConfiguration[] availableLanguages = [
        new CSharpLanguageConfiguration(),
        new OtherLanguageConfiguration()
    ];

    /// <summary>
    /// Initialize a new instance of <see cref="DefaultTemplateGenerator"/> class.
    /// </summary>
    /// <param name="htmlRenderer">Renderer of the Razor components.</param>
    /// <param name="outputDir">The directory, where the generated output will be stored.</param>
    /// <param name="staticPagesDirectory">Path to the directory containing the static pages created by user. <c>null</c> indicates that the directory is not specified.</param>
    /// <param name="docVersion">Version of the documentation (e.g. 'v1.0'). Pass <c>null</c> if no specific version should be generated.</param>
    internal DefaultTemplateGenerator(HtmlRenderer htmlRenderer, string outputDir, string? staticPagesDirectory = null, string? docVersion = null)
        : base(
            htmlRenderer,
            new DocCommentTransformer(new DocCommentHtmlConfiguration()),
            outputDir,
            availableLanguages,
            staticPagesDirectory,
            docVersion)
    {
    }
}
