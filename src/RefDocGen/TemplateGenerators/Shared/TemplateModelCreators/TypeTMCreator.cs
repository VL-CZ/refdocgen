using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Attribute;
using RefDocGen.CodeElements.Types.Abstract.Exception;
using RefDocGen.TemplateGenerators.Shared.DocComments.Html;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Members;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Shared.Tools;
using RefDocGen.TemplateGenerators.Shared.Tools.Names;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;

/// <summary>
/// Base class responsible for creating template models representing any types.
/// </summary>
internal abstract class TypeTMCreator : BaseTMCreator
{
    /// <summary>
    /// Creates a new instance of <see cref="TypeTMCreator"/> class.
    /// </summary>
    /// <param name="docCommentTransformer">
    /// <inheritdoc cref="docCommentTransformer"/>.
    /// </param>
    protected TypeTMCreator(IDocCommentTransformer docCommentTransformer, IReadOnlyDictionary<Language, ILanguageSpecificData> languageSpecificData)
        : base(docCommentTransformer, languageSpecificData)
    {
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
    /// Creates a <see cref="ParameterTM"/> instance based on the provided <see cref="IParameterData"/> object.
    /// </summary>
    /// <param name="parameter">The <see cref="IParameterData"/> instance representing the parameter.</param>
    /// <returns>A <see cref="ParameterTM"/> instance based on the provided <paramref name="parameter"/>.</returns>
    protected ParameterTM GetFrom(IParameterData parameter)
    {
        var modifiers = GetLocalizedData(lang => lang.GetModifiers(parameter));

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
        var modifiers = GetLocalizedData(lang => lang.GetModifiers(typeParameter));

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
}
