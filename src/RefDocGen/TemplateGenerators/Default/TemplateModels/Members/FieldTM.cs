using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;

namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

/// <summary>
/// Represents the template model for a field.
/// </summary>
/// <param name="Name">Name of the field.</param>
/// <param name="Type">Type of the field.</param>
/// <param name="SummaryDocComment"><c>summary</c> documentation comment for the field.</param>
/// <param name="RemarksDocComment"><c>remarks</c> documentation comment for the field.</param>
/// <param name="Modifiers">Collection of modifiers for the field (e.g. public, static, etc.)</param>
/// <param name="SeeAlsoDocComments">Collection of <c>seealso</c> documentation comments for the field.</param>
/// <param name="ConstantValue">
/// Default value of the field as a string.
/// <para>
/// <see langword="null"/> if the field has no default value.
/// </para>
/// </param>
public record FieldTM(
    string Name,
    TypeLinkTM Type,
    string SummaryDocComment,
    string RemarksDocComment,
    IEnumerable<string> Modifiers,
    IEnumerable<string> SeeAlsoDocComments,
    string? ConstantValue);
