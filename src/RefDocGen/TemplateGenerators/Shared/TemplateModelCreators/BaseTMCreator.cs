using RefDocGen.CodeElements.Shared;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.CodeElements.Types.Abstract.Enum;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.TemplateGenerators.Shared.DocComments.Html;
using RefDocGen.TemplateGenerators.Shared.Languages;
using RefDocGen.TemplateGenerators.Shared.TemplateModelCreators.Tools;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Shared.Tools;
using RefDocGen.TemplateGenerators.Shared.Tools.Names;
using RefDocGen.Tools;
using System.Xml.Linq;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;

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
    /// Gets the <see cref="TypeLinkTM"/> from the provided <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The provided type.</param>
    /// <returns><see cref="TypeLinkTM"/> corresponding to the provided <paramref name="type"/>.</returns>
    protected TypeLinkTM GetTypeLink(ITypeNameData type)
    {
        string? url = typeUrlResolver.GetUrlOf(type);
        var name = GetLanguageSpecificData(lang => lang.GetTypeName(type));

        return new TypeLinkTM(
            //CSharpTypeName.Of(type, useFullName: url is null),
            name,
            url
            );
    }

    protected GenericTypeLinkTM GetGenericTypeLink(ITypeNameData type)
    {
        string? url = typeUrlResolver.GetUrlOf(type);

        return new GenericTypeLinkTM(
            CSharpTypeName.Of(type, false, url is null), // TODO
            url,
            type.TypeParameters.Select(GetGenericTypeLink).ToArray()
            );
    }

    /// <inheritdoc cref="GetTypeLink(ITypeNameData)"/>
    protected TypeLinkTM GetTypeLink(ITypeDeclaration type)
    {
        return GetTypeLink(type.TypeObject.GetTypeNameData());
    }

    /// <summary>
    /// Gets the <see cref="TypeLinkTM"/> from the provided <paramref name="type"/> or <see langword="null"/> if the type is <see langword="null"/>.
    /// </summary>
    /// <param name="type">The provided type.</param>
    /// <returns><see cref="TypeLinkTM"/> corresponding to the provided <paramref name="type"/>. <see langword="null"/> if the provided <paramref name="type"/> is <see langword="null"/>.</returns>
    protected TypeLinkTM? GetTypeLinkOrNull(ITypeNameData? type)
    {
        if (type is null)
        {
            return null;
        }

        return GetTypeLink(type);
    }

    /// <inheritdoc cref="GetTypeLinkOrNull(ITypeNameData?)"/>
    protected TypeLinkTM? GetTypeLinkOrNull(ITypeDeclaration? type)
    {
        if (type is null)
        {
            return null;
        }

        return GetTypeLink(type);
    }

    /// <summary>
    /// Gets the C# name of the type, excluding its generic parameters.
    /// </summary>
    /// <param name="type">The provided type.</param>
    /// <returns>C# name of the provided type.</returns>
    protected string GetTypeName(ITypeDeclaration type)
    {
        return CSharpTypeName.Of(type, false);
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
    /// </summary>
    /// <example>
    /// <code>
    /// var typeName = GetLanguageSpecificData(lang => lang.GetTypeName(type)); // gets name of the type in all available languages
    /// </code>
    /// </example>
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
        return new TypeNameTM(type.Id, typeKindName, typeName, ToHtmlString(type.SummaryDocComment));
    }
}
