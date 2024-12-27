namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

/// <summary>
/// Represents the template model for a field.
/// </summary>
/// <param name="Name">Name of the field.</param>
/// <param name="Type">Type of the field.</param>
/// <param name="SummaryDocComment">'summary' documentation comment for the field.</param>
/// <param name="RemarksDocComment">'remarks' documentation comment for the field.</param>
/// <param name="Modifiers">Collection of modifiers for the field (e.g. public, static, etc.)</param>
/// <param name="SeeAlsoDocComments">Collection of 'seealso' documentation comments for the field.</param>
/// <param name="ConstantValue">
/// Default value of the parameter as a string.
/// <para>
/// <see langword="null"/> if the parameter has no default value.
/// </para>
/// </param>
public record FieldTM(
    string Name,
    string Type,
    string SummaryDocComment,
    string RemarksDocComment,
    IEnumerable<string> Modifiers,
    IEnumerable<string> SeeAlsoDocComments,
    string? ConstantValue);
