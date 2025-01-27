using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;

namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

/// <summary>
/// Represents the template model of a constructor.
/// </summary>
/// <param name="Id">Identifier of the constructor.</param>
/// <param name="Parameters">Collection of the constructor parameters.</param>
/// <param name="SummaryDocComment"><c>summary</c> documentation comment for the constructor. <c>null</c> if the doc comment is not provided.</param>
/// <param name="RemarksDocComment"><c>remarks</c> documentation comment for the constructor. <c>null</c> if the doc comment is not provided.</param>
/// <param name="Modifiers">Collection of the constructor modifiers (e.g. private, static, etc.)</param>
/// <param name="SeeAlsoDocComments">Collection of <c>seealso</c> documentation comments for the constructor.</param>
/// <param name="Exceptions">
/// A collection of user-documented exceptions (using the <c>exception</c> XML tag) the constructor might throw.
/// </param>
/// <param name="Attributes">Array of attributes applied to the constructor.</param>
public record ConstructorTM(
    string Id,
    ParameterTM[] Parameters,
    string[] Modifiers,
    AttributeTM[] Attributes,
    string? SummaryDocComment,
    string? RemarksDocComment,
    string[] SeeAlsoDocComments,
    ExceptionTM[] Exceptions);
