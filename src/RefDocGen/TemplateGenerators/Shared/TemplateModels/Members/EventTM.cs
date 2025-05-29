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
/// <param name="BaseDeclaringType">
/// If the event overrides another member, this property returns the base type that originally declared the member.
/// <c>null</c> is returned if the event doesn't override anything.
/// </param>
/// <param name="ExplicitInterfaceType">
/// If the event is an explicit implementation, the type of the interface that explicitly declared it is returned.
/// <c>null</c> if the event is not an explicit implementation.
/// </param>
/// <param name="ImplementedInterfaces">
/// Returns the types of the interfaces, whose part of contract this event implements.
/// </param>
public record EventTM(
    string Id,
    string Name,
    TypeLinkTM Type,
    LanguageSpecificData<string[]> Modifiers,
    AttributeTM[] Attributes,
    string? SummaryDocComment,
    string? RemarksDocComment,
    string[] SeeAlsoDocComments,
    ExceptionTM[] Exceptions,
    TypeLinkTM? InheritedFrom,
    TypeLinkTM? BaseDeclaringType,
    TypeLinkTM? ExplicitInterfaceType,
    TypeLinkTM[] ImplementedInterfaces);
