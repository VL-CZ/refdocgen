using RefDocGen.CodeElements.Abstract.Types.Delegate;
using RefDocGen.TemplateGenerators.Shared.DocComments.Html;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Shared.Tools.Keywords;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the individual delegates.
/// </summary>
internal class DelegateTMCreator : TypeTMCreator
{
    public DelegateTMCreator(IDocCommentTransformer docCommentTransformer) : base(docCommentTransformer)
    {
    }

    /// <summary>
    /// Creates a <see cref="DelegateTypeTM"/> instance based on the provided <see cref="IDelegateTypeData"/> object.
    /// </summary>
    /// <param name="delegateType">The <see cref="IDelegateTypeData"/> instance representing the delegate.</param>
    /// <returns>A <see cref="DelegateTypeTM"/> instance based on the provided <paramref name="delegateType"/>.</returns>
    internal DelegateTypeTM GetFrom(IDelegateTypeData delegateType)
    {
        List<Keyword> modifiers = [delegateType.AccessModifier.ToKeyword()];

        return new DelegateTypeTM(
            delegateType.Id,
            GetTypeName(delegateType),
            delegateType.Namespace,
            delegateType.Assembly,
            modifiers.GetStrings(),
            GetTypeLink(delegateType.ReturnType),
            delegateType.ReturnType.IsVoid,
            GetTemplateModels(delegateType.Parameters),
            GetTemplateModels(delegateType.TypeParameters),
            GetTemplateModels(delegateType.Attributes),
            GetTypeLinkOrNull(delegateType.DeclaringType),
            ToHtmlString(delegateType.SummaryDocComment),
            ToHtmlString(delegateType.RemarksDocComment),
            ToHtmlString(delegateType.ReturnValueDocComment),
            GetHtmlStrings(delegateType.SeeAlsoDocComments),
            GetTemplateModels(delegateType.Exceptions));
    }
}
