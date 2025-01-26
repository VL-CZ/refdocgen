using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;

namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

/// <summary>
/// Represents the template model for an indexer.
/// </summary>
/// <param name="Id">Identifier of the indexer.</param>
/// <param name="Type">Type of the indexer.</param>
/// <param name="Parameters">Collection of index parameters.</param>
/// <param name="SummaryDocComment"><c>summary</c> documentation comment for the indexer. <c>null</c> if the doc comment is not provided.</param>
/// <param name="RemarksDocComment"><c>remarks</c> documentation comment for the indexer. <c>null</c> if the doc comment is not provided.</param>
/// <param name="ValueDocComment"><c>value</c> documentation comment for the indexer. <c>null</c> if the doc comment is not provided.</param>
/// <param name="Modifiers">Collection of indexer modifiers (e.g. public, static, etc.)</param>
/// <param name="HasGetter">Checks if the indexer has getter.</param>
/// <param name="HasSetter">Checks if the indexer has setter.</param>
/// <param name="GetterModifiers">Collection of the getter modifiers (possibly empty).</param>
/// <param name="SetterModifiers">Collection of the setter modifiers (possibly empty).</param>
/// <param name="SeeAlsoDocComments">Collection of <c>seealso</c> documentation comments for the indexer.</param>
/// <param name="Exceptions">
/// A collection of user-documented exceptions (using the <c>exception</c> XML tag) that the indexer might throw.
/// </param>
/// <param name="Attributes">Array of attributes applied to the indexer.</param>
public record IndexerTM(
    string Id,
    TypeLinkTM Type,
    ParameterTM[] Parameters,
    bool HasGetter,
    bool HasSetter,
    string[] Modifiers,
    string[] GetterModifiers,
    string[] SetterModifiers,
    AttributeTM[] Attributes,
    string? SummaryDocComment,
    string? RemarksDocComment,
    string? ValueDocComment,
    string[] SeeAlsoDocComments,
    ExceptionTM[] Exceptions);
