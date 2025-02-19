using RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModels.Members;

/// <summary>
/// Represents the template model for an event.
/// </summary>
/// <param name="Id">Identifier of the event.</param>
/// <param name="Name">Name of the event.</param>
/// <param name="Type">Type of the event.</param>
/// <param name="SummaryDocComment"><c>summary</c> documentation comment for the event. <c>null</c> if the doc comment is not provided.</param>
/// <param name="RemarksDocComment"><c>remarks</c> documentation comment for the event. <c>null</c> if the doc comment is not provided.</param>
/// <param name="Modifiers">Collection of modifiers for the event (e.g. public, static, etc.)</param>
/// <param name="SeeAlsoDocComments">Collection of <c>seealso</c> documentation comments for the event.</param>
/// <param name="Exceptions">
/// A collection of user-documented exceptions (using the <c>exception</c> XML tag) that the event might throw.
/// </param>
/// <param name="Attributes">Array of attributes applied to the event.</param>
/// <param name="InheritedFrom">
/// If the event is inherited, this represents the type from which it originates.
/// <c>null</c> if the event is not inherited.
/// </param>
public record EventTM(
    string Id,
    string Name,
    TypeLinkTM Type,
    string[] Modifiers,
    AttributeTM[] Attributes,
    string? SummaryDocComment,
    string? RemarksDocComment,
    string[] SeeAlsoDocComments,
    ExceptionTM[] Exceptions,
    TypeLinkTM? InheritedFrom);
