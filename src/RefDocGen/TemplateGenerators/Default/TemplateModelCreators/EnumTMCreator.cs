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
internal class EnumTMCreator : TypeTMCreator
{
    public EnumTMCreator(IDocCommentTransformer docCommentTransformer) : base(docCommentTransformer)
    {
    }

    /// <summary>
    /// Creates a <see cref="EnumTypeTM"/> instance based on the provided <see cref="IEnumTypeData"/> object.
    /// </summary>
    /// <param name="enumType">The <see cref="IEnumTypeData"/> instance representing the enum.</param>
    /// <returns>A <see cref="EnumTypeTM"/> instance based on the provided <paramref name="enumType"/>.</returns>
    internal EnumTypeTM GetFrom(IEnumTypeData enumType)
    {
        var enumMemberTMs = enumType.Members.Select(GetFrom).ToArray();
        List<Keyword> modifiers = [enumType.AccessModifier.ToKeyword()];

        return new EnumTypeTM(
            enumType.Id,
            enumType.ShortName,
            enumType.Namespace,
            modifiers.GetStrings(),
            enumMemberTMs,
            ToHtmlString(enumType.SummaryDocComment),
            ToHtmlString(enumType.RemarksDocComment),
            GetHtmlStrings(enumType.SeeAlsoDocComments));
    }

    /// <summary>
    /// Creates a <see cref="EnumMemberTM"/> instance based on the provided <see cref="IEnumTypeData"/> object.
    /// </summary>
    /// <param name="enumMember">The <see cref="IEnumTypeData"/> instance representing the enum member.</param>
    /// <returns>A <see cref="EnumMemberTM"/> instance based on the provided <paramref name="enumMember"/>.</returns>
    internal EnumMemberTM GetFrom(IEnumMemberData enumMember)
    {
        return new EnumMemberTM(
            enumMember.Id,
            enumMember.Name,
            ToHtmlString(enumMember.SummaryDocComment),
            ToHtmlString(enumMember.RemarksDocComment),
            GetHtmlStrings(enumMember.SeeAlsoDocComments));
    }
}
