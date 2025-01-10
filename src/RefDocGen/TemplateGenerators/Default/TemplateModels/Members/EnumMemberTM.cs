namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

/// <summary>
/// Represents the template model for an enum member.
/// </summary>
/// <param name="Name">Name of the enum member.</param>
/// <param name="SummaryDocComment"><c>summary</c> documentation comment for the enum member.</param>
/// <param name="RemarksDocComment"><c>remarks</c> documentation comment for the enum member.</param>
/// <param name="SeeAlsoDocComments">Collection of <c>seealso</c> documentation comments for the enum member.</param>
public record EnumMemberTM(
    string Name,
    string SummaryDocComment,
    string RemarksDocComment,
    IEnumerable<string> SeeAlsoDocComments);
