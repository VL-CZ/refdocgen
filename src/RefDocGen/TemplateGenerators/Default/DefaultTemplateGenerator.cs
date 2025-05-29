using Microsoft.AspNetCore.Components.Web;
using RefDocGen.TemplateGenerators.Shared;
using RefDocGen.TemplateGenerators.Shared.DocComments.Html;

#pragma warning disable IDE0005 // add the namespace containing the Razor templates
using RefDocGen.TemplateGenerators.Default.Templates;
using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.CodeElements.Types.Abstract.Enum;
using RefDocGen.CodeElements;
#pragma warning restore IDE0005

namespace RefDocGen.TemplateGenerators.Default;

internal class OtherLanguageData : ILanguageSpecificData
{
    public string LanguageName => "Other";

    public string LanguageId => "other-lang";

    public string FormatLiteralValue(object? literalValue)
    {
        return "";
    }

    public string[] GetModifiers(IFieldData field)
    {
        return [];
    }

    public PropertyModifiers GetModifiers(IPropertyData property)
    {
        return new([], [], []);
    }

    public string[] GetModifiers(IMethodData method)
    {
        return [];
    }

    public string[] GetModifiers(IConstructorData constructor)
    {
        return [];
    }

    public string[] GetModifiers(IEventData eventData)
    {
        return [];
    }

    public string[] GetModifiers(IParameterData parameter)
    {
        return [];
    }

    public PropertyModifiers GetModifiers(IIndexerData indexer)
    {
        return new([], [], []);
    }

    public string[] GetModifiers(IOperatorData operatorData)
    {
        return [];
    }

    public string[] GetModifiers(IObjectTypeData type)
    {
        return [];
    }

    public string[] GetModifiers(IDelegateTypeData delegateType)
    {
        return [];
    }

    public string[] GetModifiers(IEnumTypeData enumType)
    {
        return [];
    }

    public string[] GetModifiers(ITypeParameterData typeParameter)
    {
        return [];
    }

    public string GetSpecialTypeConstraintName(SpecialTypeConstraint constraint)
    {
        return "";
    }

    public string GetTypeName(ITypeDeclaration type)
    {
        return "";
    }
}

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
    private static readonly ILanguageSpecificData[] languages = [
        new CSharpLanguageData(),
        new OtherLanguageData()
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
            languages,
            staticPagesDirectory,
            docVersion)
    {
    }
}
