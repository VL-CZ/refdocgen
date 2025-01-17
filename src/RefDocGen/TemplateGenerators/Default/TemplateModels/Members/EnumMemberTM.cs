namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

/// <summary>
/// Represents the template model for an enum member.
/// </summary>
/// <param name="Id">Identifier of the enum member.</param> 
/// <param name="Name">Name of the enum member.</param>
/// <param name="SummaryDocComment"><c>summary</c> documentation comment for the enum member. <c>null</c> if the doc comment is not provided.</param>
/// <param name="RemarksDocComment"><c>remarks</c> documentation comment for the enum member. <c>null</c> if the doc comment is not provided.</param>
/// <param name="SeeAlsoDocComments">Collection of <c>seealso</c> documentation comments for the enum member.</param>
/// <param name="Value">String representation of the underlying integral value of the enum member.</param>
public record EnumMemberTM(
    string Id,
    string Name,
    string? Value,
    string? SummaryDocComment,
    string? RemarksDocComment,
    string[] SeeAlsoDocComments);
