using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Attribute;
using RefDocGen.CodeElements.Types.Abstract.Exception;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.TemplateGenerators.Shared.DocComments.Html;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Members;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Shared.Tools;
using RefDocGen.TemplateGenerators.Shared.Tools.Keywords;
using RefDocGen.TemplateGenerators.Shared.Tools.Names;
using RefDocGen.Tools;
using System.Xml.Linq;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;

/// <summary>
/// Base class responsible for creating template models representing any types.
/// </summary>
internal abstract class TypeTMCreator
{
    protected ILanguageSpecificData languageSpecificData = new CSharpLanguageData();

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
    protected TypeTMCreator(IDocCommentTransformer docCommentTransformer)
    {
        this.docCommentTransformer = docCommentTransformer;
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
    /// Converts each item of the enumerable into <see cref="ParameterTM"/> instance.
    /// </summary>
    /// <param name="parameters">An enumerable of the provided <see cref="IParameterData" /> to convert</param>
    /// <returns>Enumerable of <see cref="ParameterTM"/> instances, corresponding to the provided parameters.</returns>
    protected ParameterTM[] GetTemplateModels(IEnumerable<IParameterData> parameters)
    {
        return [.. parameters.Select(GetFrom)];
    }

    /// <summary>
    /// Converts each item of the enumerable into <see cref="TypeParameterTM"/> instance.
    /// </summary>
    /// <param name="typeParameters">An enumerable of the provided <see cref="ITypeParameterData" /> to convert</param>
    /// <returns>Enumerable of <see cref="TypeParameterTM"/> instances, corresponding to the provided type parameters.</returns>
    protected TypeParameterTM[] GetTemplateModels(IEnumerable<ITypeParameterData> typeParameters)
    {
        return [.. typeParameters.Select(GetFrom)];
    }

    /// <summary>
    /// Converts each item of the enumerable into <see cref="ExceptionTM"/> instance.
    /// </summary>
    /// <param name="exceptions">An enumerable of the provided <see cref="IExceptionDocumentation" /> to convert</param>
    /// <returns>Enumerable of <see cref="ExceptionTM"/> instances, corresponding to the provided exceptions.</returns>
    protected ExceptionTM[] GetTemplateModels(IEnumerable<IExceptionDocumentation> exceptions)
    {
        return [.. exceptions.Select(GetFrom)];
    }

    /// <summary>
    /// Converts each item of the enumerable into <see cref="AttributeTM"/> instance.
    /// </summary>
    /// <param name="attributes">An enumerable of the provided <see cref="IAttributeData" /> to convert.</param>
    /// <returns>Enumerable of <see cref="AttributeTM"/> instances, corresponding to the provided attributes.</returns>
    protected AttributeTM[] GetTemplateModels(IEnumerable<IAttributeData> attributes)
    {
        return [.. attributes.Select(GetFrom)];
    }

    /// <summary>
    /// Gets the <see cref="TypeLinkTM"/> from the provided <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The provided type.</param>
    /// <returns><see cref="TypeLinkTM"/> corresponding to the provided <paramref name="type"/>.</returns>
    protected TypeLinkTM GetTypeLink(ITypeNameData type)
    {
        string? url = typeUrlResolver.GetUrlOf(type);

        return new TypeLinkTM(
            CSharpTypeName.Of(type, url is null),
            url
            );
    }

    /// <inheritdoc cref="GetTypeLink(ITypeNameData)"/>
    protected TypeLinkTM GetTypeLink(ITypeDeclaration type)
    {
        string? url = typeUrlResolver.GetUrlOf(type.Id);

        return new TypeLinkTM(
            CSharpTypeName.Of(type),
            url);
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
    /// Creates a <see cref="ParameterTM"/> instance based on the provided <see cref="IParameterData"/> object.
    /// </summary>
    /// <param name="parameter">The <see cref="IParameterData"/> instance representing the parameter.</param>
    /// <returns>A <see cref="ParameterTM"/> instance based on the provided <paramref name="parameter"/>.</returns>
    protected ParameterTM GetFrom(IParameterData parameter)
    {
        var modifiers = languageSpecificData.GetModifiers(parameter);

        string? defaultValue = parameter.DefaultValue == DBNull.Value
            ? null
            : LiteralValueFormatter.Format(parameter.DefaultValue);

        return new ParameterTM(
            parameter.Name,
            GetTypeLink(parameter.Type),
            modifiers,
            GetTemplateModels(parameter.Attributes),
            defaultValue,
            ToHtmlString(parameter.DocComment));
    }

    /// <summary>
    /// Creates a <see cref="TypeParameterTM"/> instance based on the provided <see cref="ITypeParameterData"/> object.
    /// </summary>
    /// <param name="typeParameter">The <see cref="ITypeParameterData"/> instance representing the type parameter.</param>
    /// <returns>A <see cref="TypeParameterTM"/> instance based on the provided <paramref name="typeParameter"/>.</returns>
    protected TypeParameterTM GetFrom(ITypeParameterData typeParameter)
    {
        var modifiers = languageSpecificData.GetModifiers(typeParameter);

        // get constraints
        var typeConstraints = typeParameter.TypeConstraints.Select(GetTypeLink);
        var specialConstraints = typeParameter.SpecialConstraints.Select(c => c.GetName());

        return new TypeParameterTM(
            typeParameter.Name,
            ToHtmlString(typeParameter.DocComment),
            modifiers,
            typeConstraints,
            specialConstraints);
    }

    /// <summary>
    /// Creates a <see cref="ExceptionTM"/> instance based on the provided <see cref="IExceptionDocumentation"/> object.
    /// </summary>
    /// <param name="exception">The <see cref="IExceptionDocumentation"/> instance representing the exception.</param>
    /// <returns>A <see cref="ExceptionTM"/> instance based on the provided <paramref name="exception"/>.</returns>
    protected ExceptionTM GetFrom(IExceptionDocumentation exception)
    {
        return new ExceptionTM(
            new TypeLinkTM(
                exception.Id,
                typeUrlResolver.GetUrlOf(exception.Id)
                ),
            ToHtmlString(exception.DocComment));
    }

    /// <summary>
    /// Creates a <see cref="AttributeTM"/> instance based on the provided <see cref="IAttributeData"/> object.
    /// </summary>
    /// <param name="attribute">The <see cref="IAttributeData"/> instance representing the attribute.</param>
    /// <returns>A <see cref="AttributeTM"/> instance based on the provided <paramref name="attribute"/>.</returns>
    protected AttributeTM GetFrom(IAttributeData attribute)
    {
        string?[] constructorArgumentTMs = [.. attribute.ConstructorArgumentValues.Select(LiteralValueFormatter.Format)];
        var namedArgumentTMs = attribute.NamedArguments.Select(na => GetFrom(na, attribute)).ToArray();

        var typeLink = new TypeLinkTM(
                CSharpAttributeName.Of(attribute),
                typeUrlResolver.GetUrlOf(attribute.Type));

        return new AttributeTM(
               typeLink,
               constructorArgumentTMs,
               namedArgumentTMs);
    }

    /// <summary>
    /// Creates a <see cref="NamedAttributeArgumentTM"/> instance based on the provided <see cref="NamedAttributeArgument"/> and <see cref="IAttributeData"/> objects.
    /// </summary>
    /// <param name="argument">The <see cref="NamedAttributeArgument"/> instance representing the attribute argument.</param>
    /// <param name="attribute">The attribute containing the <paramref name="argument"/>.</param>
    /// <returns>A <see cref="NamedAttributeArgumentTM"/> instance based on the provided <paramref name="argument"/> and <paramref name="attribute"/>.</returns>
    protected NamedAttributeArgumentTM GetFrom(NamedAttributeArgument argument, IAttributeData attribute)
    {
        return new NamedAttributeArgumentTM(
            new TypeLinkTM(
                argument.Name,
                typeUrlResolver.GetUrlOf(attribute.Type.Id, argument.Name)
            ),
            LiteralValueFormatter.Format(argument.Value));
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
}
