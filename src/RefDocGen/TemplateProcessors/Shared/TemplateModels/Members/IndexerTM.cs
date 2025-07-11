using RefDocGen.TemplateProcessors.Shared.Languages;
using RefDocGen.TemplateProcessors.Shared.TemplateModels.Links;
using RefDocGen.TemplateProcessors.Shared.TemplateModels.Types;

namespace RefDocGen.TemplateProcessors.Shared.TemplateModels.Members;

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
/// <param name="HasSetter">Checks if the indexer has setter (returns true for init only setters as well).</param>
/// <param name="IsSetterInitOnly">Checks if the property has init only setter.</param>
/// <param name="GetterModifiers">Collection of the getter modifiers (possibly empty).</param>
/// <param name="SetterModifiers">Collection of the setter modifiers (possibly empty).</param>
/// <param name="SeeAlsoDocComments">Collection of <c>seealso</c> documentation comments for the indexer.</param>
/// <param name="Exceptions">
/// A collection of user-documented exceptions (using the <c>exception</c> XML tag) that the indexer might throw.
/// </param>
/// <param name="Attributes">Array of attributes applied to the indexer.</param>
/// <param name="InheritedFrom">
/// If the indexer is inherited, this represents the type from which it originates.
/// <c>null</c> if the indexer is not inherited.
/// </param>
/// <param name="BaseDeclaringType">
/// If the indexer overrides another member, this property returns the base type that originally declared the member.
/// <c>null</c> if the indexer doesn't override anything.
/// </param>
/// <param name="ExplicitInterfaceType">
/// If the indexer is an explicit implementation, the type of the interface that explicitly declared it is returned.
/// <c>null</c> if the indexer is not an explicit implementation.
/// </param>
/// <param name="ImplementedInterfaces">
/// Returns the types of the interfaces, whose part of contract this indexer implements.
/// </param>
/// <param name="ExampleDocComment"><c>example</c> documentation comment for the indexer. <c>null</c> if the doc comment is not provided.</param>
public record IndexerTM(
    string Id,
    GenericTypeLinkTM Type,
    ParameterTM[] Parameters,
    bool HasGetter,
    bool HasSetter,
    bool IsSetterInitOnly,
    LanguageSpecificData<string[]> Modifiers,
    LanguageSpecificData<string[]> GetterModifiers,
    LanguageSpecificData<string[]> SetterModifiers,
    AttributeTM[] Attributes,
    string? SummaryDocComment,
    string? RemarksDocComment,
    string? ValueDocComment,
    string? ExampleDocComment,
    string[] SeeAlsoDocComments,
    ExceptionTM[] Exceptions,
    CodeLinkTM? InheritedFrom,
    CodeLinkTM? BaseDeclaringType,
    CodeLinkTM? ExplicitInterfaceType,
    CodeLinkTM[] ImplementedInterfaces);
