using RefDocGen.CodeElements.Abstract.Members.Enum;
using RefDocGen.CodeElements.Abstract.Types.Enum;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Members;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Tools.DocComments.Html;
using RefDocGen.TemplateGenerators.Tools.Keywords;

namespace RefDocGen.TemplateGenerators.Default.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the individual enums.
/// </summary>
internal class EnumTMCreator : BaseTMCreator
{
    public EnumTMCreator(IDocCommentTransformer docCommentTransformer) : base(docCommentTransformer)
    {
    }

    /// <summary>
    /// Creates a <see cref="EnumTypeTM"/> instance based on the provided <see cref="IEnumTypeData"/> object.
    /// </summary>
    /// <param name="enumData">The <see cref="IEnumTypeData"/> instance representing the enum.</param>
    /// <returns>A <see cref="EnumTypeTM"/> instance based on the provided <paramref name="enumData"/>.</returns>
    internal EnumTypeTM GetFrom(IEnumTypeData enumData)
    {
        var enumMemberTMs = enumData.Members.Select(GetFrom);
        List<Keyword> modifiers = [enumData.AccessModifier.ToKeyword()];

        return new EnumTypeTM(
            enumData.Id,
            enumData.ShortName,
            enumData.Namespace,
            ToHtmlString(enumData.SummaryDocComment),
            ToHtmlString(enumData.RemarksDocComment),
            modifiers.GetStrings(),
            enumMemberTMs);
    }

    /// <summary>
    /// Creates a <see cref="EnumMemberTM"/> instance based on the provided <see cref="IEnumTypeData"/> object.
    /// </summary>
    /// <param name="enumMember">The <see cref="IEnumTypeData"/> instance representing the enum member.</param>
    /// <returns>A <see cref="EnumMemberTM"/> instance based on the provided <paramref name="enumMember"/>.</returns>
    internal EnumMemberTM GetFrom(IEnumMemberData enumMember)
    {
        return new EnumMemberTM(
            enumMember.Name,
            ToHtmlString(enumMember.SummaryDocComment),
            ToHtmlString(enumMember.RemarksDocComment),
            GetHtmlStrings(enumMember.SeeAlsoDocComments));
    }
}
