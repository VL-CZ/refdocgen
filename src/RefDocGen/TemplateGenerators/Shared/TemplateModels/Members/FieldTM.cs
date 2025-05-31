using RefDocGen.TemplateGenerators.Shared.Languages;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Links;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModels.Members;

/// <summary>
/// Represents the template model for a field.
/// </summary>
/// <param name="Id">Identifier of the field.</param>
/// <param name="Name">Name of the field.</param>
/// <param name="Type">Type of the field.</param>
/// <param name="SummaryDocComment"><c>summary</c> documentation comment for the field. <c>null</c> if the doc comment is not provided.</param>
/// <param name="RemarksDocComment"><c>remarks</c> documentation comment for the field. <c>null</c> if the doc comment is not provided.</param>
/// <param name="Modifiers">Collection of modifiers for the field (e.g. public, static, etc.)</param>
/// <param name="SeeAlsoDocComments">Collection of <c>seealso</c> documentation comments for the field.</param>
/// <param name="ConstantValue">
/// Default value of the field as a string.
/// <para>
/// <see langword="null"/> if the field has no default value.
/// </para>
/// </param>
/// <param name="Attributes">Array of attributes applied to the field.</param>
/// <param name="InheritedFrom">
/// If the field is inherited, this represents the type from which it originates.
/// <c>null</c> if the field is not inherited.
/// </param>
public record FieldTM(
    string Id,
    string Name,
    GenericTypeLinkTM Type,
    LanguageSpecificData<string[]> Modifiers,
    LanguageSpecificData<string>? ConstantValue,
    AttributeTM[] Attributes,
    string? SummaryDocComment,
    string? RemarksDocComment,
    string[] SeeAlsoDocComments,
    CodeLinkTM? InheritedFrom);
