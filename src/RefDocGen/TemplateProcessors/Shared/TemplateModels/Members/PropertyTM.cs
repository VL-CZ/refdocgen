using RefDocGen.TemplateProcessors.Shared.Languages;
using RefDocGen.TemplateProcessors.Shared.TemplateModels.Links;
using RefDocGen.TemplateProcessors.Shared.TemplateModels.Types;

namespace RefDocGen.TemplateProcessors.Shared.TemplateModels.Members;

/// <summary>
/// Represents the template model for a property.
/// </summary>
/// <param name="Id">Identifier of the property.</param>
/// <param name="Name">Name of the property.</param>
/// <param name="Type">Type of the property.</param>
/// <param name="SummaryDocComment"><c>summary</c> documentation comment for the property. <c>null</c> if the doc comment is not provided.</param>
/// <param name="RemarksDocComment"><c>remarks</c> documentation comment for the property. <c>null</c> if the doc comment is not provided.</param>
/// <param name="ValueDocComment"><c>value</c> documentation comment for the property. <c>null</c> if the doc comment is not provided.</param>
/// <param name="Modifiers">Collection of property modifiers (e.g. public, static, etc.)</param>
/// <param name="HasGetter">Checks if the property has getter.</param>
/// <param name="HasSetter">Checks if the property has setter (returns true for init only setters as well).</param>
/// <param name="IsSetterInitOnly">Checks if the property has init only setter.</param>
/// <param name="GetterModifiers">Collection of the getter modifiers (possibly empty).</param>
/// <param name="SetterModifiers">Collection of the setter modifiers (possibly empty).</param>
/// <param name="SeeAlsoDocComments">Collection of <c>seealso</c> documentation comments for the property.</param>
/// <param name="Exceptions">
/// A collection of user-documented exceptions (using the <c>exception</c> XML tag) that the property might throw.
/// </param>
/// <param name="ConstantValue">
/// Default value of the parameter as a string.
/// <para>
/// <see langword="null"/> if the parameter has no default value.
/// </para>
/// </param>
/// <param name="Attributes">Array of attributes applied to the property.</param>
/// <param name="InheritedFrom">
/// If the property is inherited, this represents the type from which it originates.
/// <c>null</c> if the property is not inherited.
/// </param>
/// <param name="BaseDeclaringType">
/// If the property overrides another member, this property returns the base type that originally declared the member.
/// <c>null</c> if the property doesn't override anything.
/// </param>
/// <param name="ExplicitInterfaceType">
/// If the property is an explicit implementation, the type of the interface that explicitly declared the property is returned.
/// <c>null</c> if the property is not an explicit implementation.
/// </param>
/// <param name="ImplementedInterfaces">
/// Returns the types of the interfaces, whose part of contract this property implements.
/// </param>
public record PropertyTM(
    string Id,
    string Name,
    GenericTypeLinkTM Type,
    bool HasGetter,
    bool HasSetter,
    bool IsSetterInitOnly,
    LanguageSpecificData<string[]> Modifiers,
    LanguageSpecificData<string[]> GetterModifiers,
    LanguageSpecificData<string[]> SetterModifiers,
    LanguageSpecificData<string>? ConstantValue,
    AttributeTM[] Attributes,
    string? SummaryDocComment,
    string? RemarksDocComment,
    string? ValueDocComment,
    string[] SeeAlsoDocComments,
    ExceptionTM[] Exceptions,
    CodeLinkTM? InheritedFrom,
    CodeLinkTM? BaseDeclaringType,
    CodeLinkTM? ExplicitInterfaceType,
    CodeLinkTM[] ImplementedInterfaces);
