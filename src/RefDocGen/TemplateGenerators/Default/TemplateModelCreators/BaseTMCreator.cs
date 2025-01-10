using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Exception;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Members;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Tools;
using RefDocGen.TemplateGenerators.Tools.DocComments.Html;
using System.Xml.Linq;

namespace RefDocGen.TemplateGenerators.Default.TemplateModelCreators;

internal class BaseTMCreator
{
    protected readonly IDocCommentTransformer docCommentTransformer;
    protected readonly TypeUrlResolver typeUrlResolver;

    protected readonly ParameterTMCreator parameterTMCreator;
    protected readonly TypeParameterTMCreator typeParameterTMCreator;
    protected readonly ExceptionTMCreator exceptionTMCreator;

    public BaseTMCreator(IDocCommentTransformer docCommentTransformer)
    {
        this.docCommentTransformer = docCommentTransformer;
        typeUrlResolver = new(docCommentTransformer.TypeRegistry);
        parameterTMCreator = new(docCommentTransformer);
        typeParameterTMCreator = new(docCommentTransformer);
        exceptionTMCreator = new(docCommentTransformer);
    }

    protected string ToHtmlString(XElement docComment)
    {
        return docCommentTransformer.ToHtmlString(docComment);
    }

    protected ParameterTM[] GetTemplateModels(IEnumerable<IParameterData> parameters)
    {
        return parameters
                .Select(parameterTMCreator.GetFrom)
                .ToArray();
    }

    protected TypeParameterTM[] GetTemplateModels(IEnumerable<ITypeParameterData> typeParameters)
    {
        return typeParameters
                .Select(typeParameterTMCreator.GetFrom)
                .ToArray();
    }

    protected ExceptionTM[] GetTemplateModels(IEnumerable<IExceptionDocumentation> exceptions)
    {
        return exceptions
                .Select(exceptionTMCreator.GetFrom)
                .ToArray();
    }
}
