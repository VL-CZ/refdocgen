using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Exception;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Members;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Tools;
using RefDocGen.TemplateGenerators.Tools.DocComments.Html;
using RefDocGen.TemplateGenerators.Tools.Keywords;
using RefDocGen.TemplateGenerators.Tools.TypeName;
using RefDocGen.Tools;
using System.Xml.Linq;

namespace RefDocGen.TemplateGenerators.Default.TemplateModelCreators;

/// <summary>
/// Base class responsible for creating template models representing any types.
/// </summary>
internal abstract class TypeTMCreator
{
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
        return elements.Select(ToHtmlString).WhereNotNull().ToArray();
    }

    /// <summary>
    /// Converts each item of the enumerable into <see cref="ParameterTM"/> instance.
    /// </summary>
    /// <param name="parameters">An enumerable of the provided <see cref="IParameterData" /> to convert</param>
    /// <returns>Enumerable of <see cref="ParameterTM"/> instances, corresponding to the provided parameters.</returns>
    protected ParameterTM[] GetTemplateModels(IEnumerable<IParameterData> parameters)
    {
        return parameters
                .Select(GetFrom)
                .ToArray();
    }

    /// <summary>
    /// Converts each item of the enumerable into <see cref="TypeParameterTM"/> instance.
    /// </summary>
    /// <param name="typeParameters">An enumerable of the provided <see cref="ITypeParameterData" /> to convert</param>
    /// <returns>Enumerable of <see cref="TypeParameterTM"/> instances, corresponding to the provided type parameters.</returns>
    protected TypeParameterTM[] GetTemplateModels(IEnumerable<ITypeParameterData> typeParameters)
    {
        return typeParameters
                .Select(GetFrom)
                .ToArray();
    }

    /// <summary>
    /// Converts each item of the enumerable into <see cref="ExceptionTM"/> instance.
    /// </summary>
    /// <param name="exceptions">An enumerable of the provided <see cref="IExceptionDocumentation" /> to convert</param>
    /// <returns>Enumerable of <see cref="ExceptionTM"/> instances, corresponding to the provided exceptions.</returns>
    protected ExceptionTM[] GetTemplateModels(IEnumerable<IExceptionDocumentation> exceptions)
    {
        return exceptions
                .Select(GetFrom)
                .ToArray();
    }

    /// <summary>
    /// Gets the <see cref="TypeLinkTM"/> from the provided <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The provided type.</param>
    /// <returns><see cref="TypeLinkTM"/> corresponding to the provided <paramref name="type"/>.</returns>
    protected TypeLinkTM GetTypeLink(ITypeNameData type)
    {
        return new TypeLinkTM(
            CSharpTypeName.Of(type),
            typeUrlResolver.GetUrlOf(type)
            );
    }

    /// <summary>
    /// Creates a <see cref="ParameterTM"/> instance based on the provided <see cref="IParameterData"/> object.
    /// </summary>
    /// <param name="parameter">The <see cref="IParameterData"/> instance representing the parameter.</param>
    /// <returns>A <see cref="ParameterTM"/> instance based on the provided <paramref name="parameter"/>.</returns>
    protected ParameterTM GetFrom(IParameterData parameter)
    {
        List<Keyword> modifiers = [];

        if (parameter.IsExtensionParameter)
        {
            modifiers.Add(Keyword.This);
        }

        if (parameter.IsInput)
        {
            modifiers.Add(Keyword.In);
        }

        if (parameter.IsOutput)
        {
            modifiers.Add(Keyword.Out);
        }

        if (RefKeyword.IsPresentIn(parameter))
        {
            modifiers.Add(Keyword.Ref);
        }

        if (parameter.IsParamsCollection)
        {
            modifiers.Add(Keyword.Params);
        }

        string? defaultValue = parameter.DefaultValue == DBNull.Value
            ? null
            : LiteralValueFormatter.Format(parameter.DefaultValue);

        return new ParameterTM(
            parameter.Name,
            GetTypeLink(parameter.Type),
            modifiers.GetStrings(),
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
        List<Keyword> modifiers = [];

        if (typeParameter.IsCovariant)
        {
            modifiers.Add(Keyword.Out);
        }
        if (typeParameter.IsContravariant)
        {
            modifiers.Add(Keyword.In);
        }

        // get constraints
        var typeConstraints = typeParameter.TypeConstraints.Select(GetTypeLink);
        var specialConstraints = typeParameter.SpecialConstraints.Select(c => c.GetName());

        return new TypeParameterTM(
            typeParameter.Name,
            ToHtmlString(typeParameter.DocComment),
            modifiers.GetStrings(),
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
}
