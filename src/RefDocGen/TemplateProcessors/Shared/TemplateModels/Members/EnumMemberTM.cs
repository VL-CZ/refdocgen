using RefDocGen.TemplateProcessors.Shared.Languages;
using RefDocGen.TemplateProcessors.Shared.TemplateModels.Types;

namespace RefDocGen.TemplateProcessors.Shared.TemplateModels.Members;

/// <summary>
/// Represents the template model for an enum member.
/// </summary>
/// <param name="Id">Identifier of the enum member.</param>
/// <param name="Name">Name of the enum member.</param>
/// <param name="SummaryDocComment"><c>summary</c> documentation comment for the enum member. <c>null</c> if the doc comment is not provided.</param>
/// <param name="RemarksDocComment"><c>remarks</c> documentation comment for the enum member. <c>null</c> if the doc comment is not provided.</param>
/// <param name="SeeAlsoDocComments">Collection of <c>seealso</c> documentation comments for the enum member.</param>
/// <param name="Value">String representation of the underlying integral value of the enum member.</param>
/// <param name="Attributes">Array of attributes applied to the member.</param>
/// <param name="ExampleDocComment"><c>example</c> documentation comment for the enum member. <c>null</c> if the doc comment is not provided.</param>
public record EnumMemberTM(
    string Id,
    string Name,
    LanguageSpecificData<string>? Value,
    AttributeTM[] Attributes,
    string? SummaryDocComment,
    string? RemarksDocComment,
    string? ExampleDocComment,
    string[] SeeAlsoDocComments);
