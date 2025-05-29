using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.TemplateGenerators.Shared.DocComments.Html;
using RefDocGen.TemplateGenerators.Shared.Languages;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the individual delegates.
/// </summary>
internal class DelegateTMCreator : TypeTMCreator
{
    public DelegateTMCreator(IDocCommentTransformer docCommentTransformer, IEnumerable<ILanguageConfiguration> availableLanguages)
        : base(docCommentTransformer, availableLanguages)
    {
    }

    /// <summary>
    /// Creates a <see cref="DelegateTypeTM"/> instance based on the provided <see cref="IDelegateTypeData"/> object.
    /// </summary>
    /// <param name="delegateType">The <see cref="IDelegateTypeData"/> instance representing the delegate.</param>
    /// <returns>A <see cref="DelegateTypeTM"/> instance based on the provided <paramref name="delegateType"/>.</returns>
    internal DelegateTypeTM GetFrom(IDelegateTypeData delegateType)
    {
        var modifiers = GetLanguageSpecificData(langData => langData.GetModifiers(delegateType));

        return new DelegateTypeTM(
            Id: delegateType.Id,
            Name: GetTypeName(delegateType),
            Namespace: delegateType.Namespace,
            Assembly: delegateType.Assembly,
            Modifiers: modifiers,
            ReturnType: GetGenericTypeLink(delegateType.ReturnType),
            ReturnsVoid: delegateType.ReturnType.IsVoid,
            Parameters: GetTemplateModels(delegateType.Parameters),
            TypeParameters: GetTemplateModels(delegateType.TypeParameters),
            Attributes: GetTemplateModels(delegateType.Attributes),
            DeclaringType: GetTypeLinkOrNull(delegateType.DeclaringType),
            SummaryDocComment: ToHtmlString(delegateType.SummaryDocComment),
            RemarksDocComment: ToHtmlString(delegateType.RemarksDocComment),
            ReturnsDocComment: ToHtmlString(delegateType.ReturnValueDocComment),
            SeeAlsoDocComments: GetHtmlStrings(delegateType.SeeAlsoDocComments),
            Exceptions: GetTemplateModels(delegateType.Exceptions));
    }
}
