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
using System.Xml.Linq;

namespace RefDocGen.TemplateGenerators.Default.TemplateModelCreators;

internal class BaseTMCreator
{
    protected readonly IDocCommentTransformer docCommentTransformer;
    protected readonly TypeUrlResolver typeUrlResolver;

    public BaseTMCreator(IDocCommentTransformer docCommentTransformer)
    {
        this.docCommentTransformer = docCommentTransformer;
        typeUrlResolver = new(docCommentTransformer.TypeRegistry);
    }

    protected string ToHtmlString(XElement docComment)
    {
        return docCommentTransformer.ToHtmlString(docComment);
    }

    protected string[] GetHtmlStrings(IEnumerable<XElement> elements)
    {
        return elements.Select(ToHtmlString).ToArray();
    }

    protected ParameterTM[] GetTemplateModels(IEnumerable<IParameterData> parameters)
    {
        return parameters
                .Select(GetFrom)
                .ToArray();
    }

    protected TypeParameterTM[] GetTemplateModels(IEnumerable<ITypeParameterData> typeParameters)
    {
        return typeParameters
                .Select(GetFrom)
                .ToArray();
    }

    protected ExceptionTM[] GetTemplateModels(IEnumerable<IExceptionDocumentation> exceptions)
    {
        return exceptions
                .Select(GetFrom)
                .ToArray();
    }

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
    /// <param name="parameterData">The <see cref="IParameterData"/> instance representing the parameter.</param>
    /// <returns>A <see cref="ParameterTM"/> instance based on the provided <paramref name="parameterData"/>.</returns>
    protected ParameterTM GetFrom(IParameterData parameterData)
    {
        List<Keyword> modifiers = [];

        if (parameterData.IsInput)
        {
            modifiers.Add(Keyword.In);
        }

        if (parameterData.IsOutput)
        {
            modifiers.Add(Keyword.Out);
        }

        if (RefKeyword.IsPresentIn(parameterData))
        {
            modifiers.Add(Keyword.Ref);
        }

        if (parameterData.IsParamsCollection)
        {
            modifiers.Add(Keyword.Params);
        }

        string? defaultValue = parameterData.DefaultValue == DBNull.Value
            ? null
            : LiteralValueFormatter.Format(parameterData.DefaultValue);

        return new ParameterTM(
            parameterData.Name,
            CSharpTypeName.Of(parameterData.Type),
            ToHtmlString(parameterData.DocComment),
            modifiers.GetStrings(),
            defaultValue);
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
        var typeConstraints = typeParameter.TypeConstraints.Select(CSharpTypeName.Of);
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
            exception.TypeName,
            ToHtmlString(exception.DocComment));
    }
}
