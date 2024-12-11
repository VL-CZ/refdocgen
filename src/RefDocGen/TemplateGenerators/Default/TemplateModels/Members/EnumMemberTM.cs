namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

/// <summary>
/// Represents the template model for an enum member.
/// </summary>
/// <param name="Name">Name of the enum member.</param>
/// <param name="SummaryDocComment">'summary' documentation comment for the enum member.</param>
/// <param name="RemarksDocComment">'remarks' documentation comment for the enum member.</param>
public record EnumMemberTM(string Name, string SummaryDocComment, string RemarksDocComment);
