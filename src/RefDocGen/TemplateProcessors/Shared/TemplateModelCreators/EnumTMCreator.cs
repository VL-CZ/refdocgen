using RefDocGen.CodeElements.Members.Abstract.Enum;
using RefDocGen.CodeElements.Types.Abstract.Enum;
using RefDocGen.TemplateProcessors.Shared.DocComments.Html;
using RefDocGen.TemplateProcessors.Shared.Languages;
using RefDocGen.TemplateProcessors.Shared.TemplateModels.Members;
using RefDocGen.TemplateProcessors.Shared.TemplateModels.Types;
using RefDocGen.TemplateProcessors.Shared.Tools;

namespace RefDocGen.TemplateProcessors.Shared.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the individual enums.
/// </summary>
internal class EnumTMCreator : TypeTMCreator
{
    public EnumTMCreator(IDocCommentTransformer docCommentTransformer, IEnumerable<ILanguageConfiguration> availableLanguages)
        : base(docCommentTransformer, availableLanguages)
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
            Id: TemplateId.Of(enumType),
            Name: enumType.ShortName,
            Namespace: enumType.Namespace,
            Assembly: enumType.Assembly,
            Modifiers: modifiers,
            Members: enumMemberTMs,
            Attributes: GetTemplateModels(enumType.Attributes),
            DeclaringType: GetCodeLinkOrNull(enumType.DeclaringType),
            SummaryDocComment: ToHtmlString(enumType.SummaryDocComment),
            RemarksDocComment: ToHtmlString(enumType.RemarksDocComment),
            ExampleDocComment: ToHtmlString(enumType.ExampleDocComment),
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
            Id: TemplateId.Of(enumMember),
            Name: enumMember.Name,
            Value: GetLanguageSpecificDefaultValue(enumMember.Value),
            Attributes: GetTemplateModels(enumMember.Attributes),
            SummaryDocComment: ToHtmlString(enumMember.SummaryDocComment),
            RemarksDocComment: ToHtmlString(enumMember.RemarksDocComment),
            ExampleDocComment: ToHtmlString(enumMember.ExampleDocComment),
            SeeAlsoDocComments: GetHtmlStrings(enumMember.SeeAlsoDocComments));
    }
}
