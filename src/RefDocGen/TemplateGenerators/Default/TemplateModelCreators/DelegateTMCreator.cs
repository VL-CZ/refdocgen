using RefDocGen.CodeElements.Abstract.Types.Delegate;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Tools.DocComments.Html;
using RefDocGen.TemplateGenerators.Tools.Keywords;
using RefDocGen.TemplateGenerators.Tools.TypeName;

namespace RefDocGen.TemplateGenerators.Default.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the individual delegates.
/// </summary>
internal class DelegateTMCreator : BaseTMCreator
{
    public DelegateTMCreator(IDocCommentTransformer docCommentTransformer) : base(docCommentTransformer)
    {
    }

    /// <summary>
    /// Creates a <see cref="DelegateTypeTM"/> instance based on the provided <see cref="IDelegateTypeData"/> object.
    /// </summary>
    /// <param name="delegateTypeData">The <see cref="IDelegateTypeData"/> instance representing the delegate.</param>
    /// <returns>A <see cref="DelegateTypeTM"/> instance based on the provided <paramref name="delegateTypeData"/>.</returns>
    internal DelegateTypeTM GetFrom(IDelegateTypeData delegateTypeData)
    {
        List<Keyword> modifiers = [delegateTypeData.AccessModifier.ToKeyword()];

        return new DelegateTypeTM(
            delegateTypeData.Id,
            CSharpTypeName.Of(delegateTypeData),
            delegateTypeData.Namespace,
            ToHtmlString(delegateTypeData.SummaryDocComment),
            ToHtmlString(delegateTypeData.RemarksDocComment),
            modifiers.GetStrings(),
            ToHtmlString(delegateTypeData.ReturnValueDocComment),
            CSharpTypeName.Of(delegateTypeData.ReturnType),
            delegateTypeData.ReturnType.IsVoid,
            GetTemplateModels(delegateTypeData.Parameters),
            GetTemplateModels(delegateTypeData.TypeParameterDeclarations),
            GetTemplateModels(delegateTypeData.Exceptions));
    }
}
