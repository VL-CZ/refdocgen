using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Attribute;
using RefDocGen.CodeElements.Types.Abstract.Exception;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.TemplateProcessors.Shared.DocComments.Html;
using RefDocGen.TemplateProcessors.Shared.Languages;
using RefDocGen.TemplateProcessors.Shared.TemplateModels.Links;
using RefDocGen.TemplateProcessors.Shared.TemplateModels.Members;
using RefDocGen.TemplateProcessors.Shared.TemplateModels.Types;
using RefDocGen.TemplateProcessors.Shared.Tools;
using RefDocGen.TemplateProcessors.Shared.Tools.Names;

namespace RefDocGen.TemplateProcessors.Shared.TemplateModelCreators;

/// <summary>
/// Base class responsible for creating template models representing any types.
/// </summary>
internal abstract class TypeTMCreator : BaseTMCreator
{

    protected TypeTMCreator(IDocCommentTransformer docCommentTransformer, IEnumerable<ILanguageConfiguration> availableLanguages)
        : base(docCommentTransformer, availableLanguages)
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
        var modifiers = GetLanguageSpecificData(lang => lang.GetModifiers(parameter));

        return new ParameterTM(
            parameter.Name,
            GetGenericTypeLink(parameter.Type),
            modifiers,
            GetTemplateModels(parameter.Attributes),
            GetLanguageSpecificDefaultValue(parameter.DefaultValue),
            ToHtmlString(parameter.DocComment));
    }

    /// <summary>
    /// Creates a <see cref="TypeParameterTM"/> instance based on the provided <see cref="ITypeParameterData"/> object.
    /// </summary>
    /// <param name="typeParameter">The <see cref="ITypeParameterData"/> instance representing the type parameter.</param>
    /// <returns>A <see cref="TypeParameterTM"/> instance based on the provided <paramref name="typeParameter"/>.</returns>
    protected TypeParameterTM GetFrom(ITypeParameterData typeParameter)
    {
        var modifiers = GetLanguageSpecificData(lang => lang.GetModifiers(typeParameter));

        // get constraints
        var typeConstraints = typeParameter.TypeConstraints.Select(GetGenericTypeLink).ToArray();
        var specialConstraints = GetLanguageSpecificData(lang =>
        {
            var constraints = typeParameter.SpecialConstraints;
            return constraints.Select(c => lang.GetSpecialTypeConstraintName(c)).ToArray();
        });

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
        var name = GetLanguageSpecificData(_ => exception.Id);
        string? url = typeUrlResolver.GetUrlOf(exception.Id);

        if (TypeTools.GetType(exception.Id) is ITypeNameData type) // type found by its ID string -> get its name
        {
            name = GetLanguageSpecificData(lang => lang.GetTypeName(type, true, url is null));
        }

        return new ExceptionTM(
            new CodeLinkTM(name, url),
            ToHtmlString(exception.DocComment));
    }

    /// <summary>
    /// Creates a <see cref="AttributeTM"/> instance based on the provided <see cref="IAttributeData"/> object.
    /// </summary>
    /// <param name="attribute">The <see cref="IAttributeData"/> instance representing the attribute.</param>
    /// <returns>A <see cref="AttributeTM"/> instance based on the provided <paramref name="attribute"/>.</returns>
    protected AttributeTM GetFrom(IAttributeData attribute)
    {
        LanguageSpecificData<string>?[] constructorArgumentTMs = [.. attribute.ConstructorArgumentValues.Select(GetLanguageSpecificDefaultValue)];
        var namedArgumentTMs = attribute.NamedArguments.Select(na => GetFrom(na, attribute)).ToArray();

        var typeLink = new CodeLinkTM(
                GetLanguageSpecificData(lang => AttributeName.Of(lang, attribute)),
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
            new CodeLinkTM(
                GetLanguageSpecificData(_ => argument.Name),
                typeUrlResolver.GetUrlOf(attribute.Type, argument.Name)
            ),
            GetLanguageSpecificDefaultValue(argument.Value));
    }

    /// <summary>
    /// Gets a language specific string of the provided <paramref name="constantValue"/> in all available languages.
    /// </summary>
    /// <param name="constantValue">The constant value to be serialized.</param>
    /// <returns><see cref="LanguageSpecificData{T}"/> containing strings of the provided <paramref name="constantValue"/>.</returns>
    protected LanguageSpecificData<string>? GetLanguageSpecificDefaultValue(object? constantValue)
    {
        if (constantValue == DBNull.Value)
        {
            return null;
        }

        return GetLanguageSpecificData(lang => lang.FormatLiteralValue(constantValue));
    }
}
