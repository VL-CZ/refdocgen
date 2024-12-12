namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

/// <summary>
/// Represents the template model for a field.
/// </summary>
/// <param name="Name">Name of the field.</param>
/// <param name="Type">Type of the field.</param>
/// <param name="SummaryDocComment">'summary' documentation comment for the field.</param>
/// <param name="RemarksDocComment">'remarks' documentation comment for the field.</param>
/// <param name="Modifiers">Collection of modifiers for the field (e.g. public, static, etc.)</param>
public record FieldTM(string Name, string Type, string SummaryDocComment, string RemarksDocComment, IEnumerable<string> Modifiers);
