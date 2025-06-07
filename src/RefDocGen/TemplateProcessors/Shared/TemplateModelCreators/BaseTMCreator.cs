using System.Xml.Linq;
using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Shared;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.CodeElements.Types.Abstract.Enum;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.TemplateProcessors.Shared.DocComments.Html;
using RefDocGen.TemplateProcessors.Shared.Languages;
using RefDocGen.TemplateProcessors.Shared.TemplateModels.Links;
using RefDocGen.TemplateProcessors.Shared.TemplateModels.Types;
using RefDocGen.TemplateProcessors.Shared.Tools;
using RefDocGen.Tools;

namespace RefDocGen.TemplateProcessors.Shared.TemplateModelCreators;

/// <summary>
/// Base class containing functionality shared between all TM Creators.
/// </summary>
internal abstract class BaseTMCreator
{
    /// <summary>
    /// Configuration of languages available in the documentation.
    /// </summary>
    protected IEnumerable<ILanguageConfiguration> availableLanguages;

    /// <summary>
    /// Transformer of the XML doc comments into HTML.
    /// </summary>
    protected readonly IDocCommentTransformer docCommentTransformer;

    /// <summary>
    /// Resolver of the individual type's documentation pages.
    /// </summary>
    protected readonly TypeUrlResolver typeUrlResolver;

    /// <summary>
    /// Creates a new instance of <see cref="TypeTMCreator"/> class.
    /// </summary>
    /// <param name="docCommentTransformer">
    /// <inheritdoc cref="docCommentTransformer"/>.
    /// </param>
    /// <param name="availableLanguages">
    /// <inheritdoc cref="availableLanguages"/>
    /// </param>
    protected BaseTMCreator(IDocCommentTransformer docCommentTransformer, IEnumerable<ILanguageConfiguration> availableLanguages)
    {
        this.docCommentTransformer = docCommentTransformer;
        this.availableLanguages = availableLanguages;
        typeUrlResolver = new(docCommentTransformer.TypeRegistry);
    }

    /// <inheritdoc cref="IDocCommentTransformer.ToHtmlString(XElement)"/>
    protected string? ToHtmlString(XElement docComment)
    {
        return docCommentTransformer.ToHtmlString(docComment);
    }

    /// <inheritdoc cref="IDocCommentTransformer.ToHtmlOneLineString(XElement)"/>
    protected string? ToHtmlOneLineString(XElement docComment)
    {
        return docCommentTransformer.ToHtmlOneLineString(docComment);
    }

    /// <summary>
    /// Converts each of the <see cref="XElement"/> to its HTML string representation.
    /// </summary>
    /// <param name="elements">The elements to be converted to their HTML strings.</param>
    /// <returns>Collection of raw HTML string representation of the <paramref name="elements"/>.</returns>
    protected string[] GetHtmlStrings(IEnumerable<XElement> elements)
    {
        return [.. elements.Select(ToHtmlString).WhereNotNull()];
    }

    /// <summary>
    /// Gets the <see cref="CodeLinkTM"/> representing the provided type or type member.
    /// </summary>
    /// <param name="type">The provided type containing the member.</param>
    /// <param name="member">The member for which the URL is returned. <c>null</c> if the link should point to a type.</param>
    /// <param name="includeTypeParameters">Specifies whether the type paramters should be included in the resulting <see cref="CodeLinkTM"/> instance.</param>
    /// <returns><see cref="CodeLinkTM"/> corresponding to the provided <paramref name="type"/> and <paramref name="member"/>.</returns>
    protected CodeLinkTM GetCodeLink(ITypeNameData type, IMemberData? member = null, bool includeTypeParameters = true)
    {
        string? url = typeUrlResolver.GetUrlOf(type, member?.Id);

        var name = GetLanguageSpecificData(lang => lang.GetTypeName(type, includeTypeParameters, url is null));

        return new CodeLinkTM(name, url, member?.Name);
    }

    /// <summary>
    /// Gets the <see cref="CodeLinkTM"/> representing the provided type member.
    /// </summary>
    /// <param name="type">The provided type containing the member.</param>
    /// <param name="member">The member for which the URL is returned.</param>
    /// <returns><see cref="CodeLinkTM"/> corresponding to the provided <paramref name="type"/> and <paramref name="member"/>. <see langword="null"/> if the provided <paramref name="type"/> is <see langword="null"/>.</returns>
    protected CodeLinkTM? GetCodeLinkOrNull(ITypeNameData? type, IMemberData member)
    {
        if (type is null)
        {
            return null;
        }

        return GetCodeLink(type, member);
    }

    /// <summary>
    /// Gets the <see cref="CodeLinkTM"/> representing the provided <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The provided type.</param>
    /// <returns><see cref="CodeLinkTM"/> corresponding to the provided <paramref name="type"/>. <see langword="null"/> if the provided <paramref name="type"/> is <see langword="null"/>.</returns>
    protected CodeLinkTM? GetCodeLinkOrNull(ITypeNameData? type)
    {
        if (type is null)
        {
            return null;
        }

        return GetCodeLink(type);
    }

    /// <inheritdoc cref="GetCodeLinkOrNull(ITypeNameData?)"/>
    protected CodeLinkTM? GetCodeLinkOrNull(ITypeDeclaration? type)
    {
        if (type is null)
        {
            return null;
        }

        return GetCodeLink(type.TypeObject.GetTypeNameData());
    }

    /// <summary>
    /// Gets the <see cref="GenericTypeLinkTM"/> representing the provided <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The provided type.</param>
    /// <returns><see cref="GenericTypeLinkTM"/> corresponding to the provided <paramref name="type"/>.</returns>
    protected GenericTypeLinkTM GetGenericTypeLink(ITypeNameData type)
    {
        var typeLink = GetCodeLink(type, includeTypeParameters: false);

        return new GenericTypeLinkTM(
            typeLink,
            [.. type.TypeParameters.Select(GetGenericTypeLink)]
            );
    }

    /// <summary>
    /// Creates a <see cref="TypeNameTM"/> instance based on the provided <see cref="IObjectTypeData"/> object.
    /// </summary>
    /// <param name="type">The <see cref="IObjectTypeData"/> instance representing the type.</param>
    /// <returns>A <see cref="TypeNameTM"/> instance based on the provided <paramref name="type"/>.</returns>
    protected TypeNameTM GetTypeNameFrom(IObjectTypeData type)
    {
        return GetTypeNameFrom(type, TypeKindName.Of(type));
    }

    /// <summary>
    /// Creates a <see cref="TypeNameTM"/> instance based on the provided <see cref="IEnumTypeData"/> object.
    /// </summary>
    /// <param name="enumData">The <see cref="IEnumTypeData"/> instance representing the enum.</param>
    /// <returns>A <see cref="TypeNameTM"/> instance based on the provided <paramref name="enumData"/>.</returns>
    protected TypeNameTM GetTypeNameFrom(IEnumTypeData enumData)
    {
        return GetTypeNameFrom(enumData, TypeKindName.Enum);
    }

    /// <summary>
    /// Creates a <see cref="TypeNameTM"/> instance based on the provided <see cref="IDelegateTypeData"/> object.
    /// </summary>
    /// <param name="delegateData">The <see cref="IDelegateTypeData"/> instance representing the delegate.</param>
    /// <returns>A <see cref="TypeNameTM"/> instance based on the provided <paramref name="delegateData"/>.</returns>
    protected TypeNameTM GetTypeNameFrom(IDelegateTypeData delegateData)
    {
        return GetTypeNameFrom(delegateData, TypeKindName.Delegate);
    }

    /// <summary>
    /// Gets language specific data obtained by executing the <paramref name="languageFunction"/> on each available language.
    /// <example>
    /// <code>
    /// var typeName = GetLanguageSpecificData(lang => lang.GetTypeName(type)); // gets name of the type in all available languages
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">Type of the data returned by the <paramref name="languageFunction"/>.</typeparam>
    /// <param name="languageFunction">The function that obtains the data based on the language.</param>
    /// <returns>Language specific data obtained by executing the <paramref name="languageFunction"/> on each available language.</returns>
    protected LanguageSpecificData<T> GetLanguageSpecificData<T>(Func<ILanguageConfiguration, T> languageFunction)
    {
        var languageData = availableLanguages.
            ToDictionary(lang => lang.LanguageId, lang => languageFunction(lang));

        return new LanguageSpecificData<T>(languageData);
    }

    /// <summary>
    /// Creates a <see cref="TypeNameTM"/> instance representing a nested type based on the provided <see cref="ITypeDeclaration"/> object.
    /// </summary>
    /// <param name="type">The <see cref="ITypeDeclaration"/> instance representing the nested type.</param>
    /// <param name="typeKindName">Name of the type kind.</param>
    /// <returns>A <see cref="TypeNameTM"/> instance based on the provided <paramref name="type"/>.</returns>
    private TypeNameTM GetTypeNameFrom(ITypeDeclaration type, string typeKindName)
    {
        var typeName = GetLanguageSpecificData(lang => lang.GetTypeName(type));
        string typeId = TemplateId.Of(type);

        return new TypeNameTM(typeId, typeKindName, typeName, ToHtmlOneLineString(type.SummaryDocComment));
    }
}
