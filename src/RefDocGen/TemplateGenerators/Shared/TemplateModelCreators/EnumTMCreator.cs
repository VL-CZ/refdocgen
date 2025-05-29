using RefDocGen.CodeElements.Members.Abstract.Enum;
using RefDocGen.CodeElements.Types.Abstract.Enum;
using RefDocGen.TemplateGenerators.Shared.DocComments.Html;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Members;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the individual enums.
/// </summary>
internal class EnumTMCreator : TypeTMCreator
{
    public EnumTMCreator(IDocCommentTransformer docCommentTransformer, IEnumerable<ILanguageConfiguration> languages)
        : base(docCommentTransformer, languages)
    {
    }

    /// <summary>
    /// Creates a <see cref="EnumTypeTM"/> instance based on the provided <see cref="IEnumTypeData"/> object.
    /// </summary>
    /// <param name="enumType">The <see cref="IEnumTypeData"/> instance representing the enum.</param>
    /// <returns>A <see cref="EnumTypeTM"/> instance based on the provided <paramref name="enumType"/>.</returns>
    internal EnumTypeTM GetFrom(IEnumTypeData enumType)
    {
        var enumMemberTMs = enumType.Members.OrderBy(m => m.Value).Select(GetFrom).ToArray();
        var modifiers = GetLanguageSpecificData(langData => langData.GetModifiers(enumType));

        return new EnumTypeTM(
            Id: enumType.Id,
            Name: GetTypeName(enumType),
            Namespace: enumType.Namespace,
            Assembly: enumType.Assembly,
            Modifiers: modifiers,
            Members: enumMemberTMs,
            Attributes: GetTemplateModels(enumType.Attributes),
            DeclaringType: GetTypeLinkOrNull(enumType.DeclaringType),
            SummaryDocComment: ToHtmlString(enumType.SummaryDocComment),
            RemarksDocComment: ToHtmlString(enumType.RemarksDocComment),
            SeeAlsoDocComments: GetHtmlStrings(enumType.SeeAlsoDocComments));
    }

    /// <summary>
    /// Creates a <see cref="EnumMemberTM"/> instance based on the provided <see cref="IEnumTypeData"/> object.
    /// </summary>
    /// <param name="enumMember">The <see cref="IEnumTypeData"/> instance representing the enum member.</param>
    /// <returns>A <see cref="EnumMemberTM"/> instance based on the provided <paramref name="enumMember"/>.</returns>
    internal EnumMemberTM GetFrom(IEnumMemberData enumMember)
    {

        return new EnumMemberTM(
            Id: enumMember.Id,
            Name: enumMember.Name,
            Value: GetLanguageSpecificDefaultValue(enumMember.Value),
            Attributes: GetTemplateModels(enumMember.Attributes),
            SummaryDocComment: ToHtmlString(enumMember.SummaryDocComment),
            RemarksDocComment: ToHtmlString(enumMember.RemarksDocComment),
            SeeAlsoDocComments: GetHtmlStrings(enumMember.SeeAlsoDocComments));
    }
}
