using RefDocGen.CodeElements.Abstract.Members.Enum;
using RefDocGen.CodeElements.Abstract.Types.Enum;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Members;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Tools.Keywords;

namespace RefDocGen.TemplateGenerators.Default.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the individual enums.
/// </summary>
internal static class EnumTMCreator
{
    /// <summary>
    /// Creates a <see cref="EnumTypeTM"/> instance based on the provided <see cref="IEnumTypeData"/> object.
    /// </summary>
    /// <param name="enumData">The <see cref="IEnumTypeData"/> instance representing the enum.</param>
    /// <returns>A <see cref="EnumTypeTM"/> instance based on the provided <paramref name="enumData"/>.</returns>
    internal static EnumTypeTM GetFrom(IEnumTypeData enumData)
    {
        var enumMemberTMs = enumData.Members.Select(GetFrom);
        List<Keyword> modifiers = [enumData.AccessModifier.ToKeyword()];

        return new EnumTypeTM(
            enumData.Id,
            enumData.ShortName,
            enumData.Namespace,
            enumData.SummaryDocComment.Value,
            enumData.RemarksDocComment.Value,
            modifiers.GetStrings(),
            enumMemberTMs);
    }

    /// <summary>
    /// Creates a <see cref="EnumMemberTM"/> instance based on the provided <see cref="IEnumTypeData"/> object.
    /// </summary>
    /// <param name="enumMember">The <see cref="IEnumTypeData"/> instance representing the enum member.</param>
    /// <returns>A <see cref="EnumMemberTM"/> instance based on the provided <paramref name="enumMember"/>.</returns>
    internal static EnumMemberTM GetFrom(IEnumMemberData enumMember)
    {
        return new EnumMemberTM(enumMember.Name, enumMember.SummaryDocComment.Value, enumMember.RemarksDocComment.Value);
    }
}
