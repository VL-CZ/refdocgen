using RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModels.Members;

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
/// <c>null</c> if the member doesn't override anything.
/// </param>
public record PropertyTM(
    string Id,
    string Name,
    TypeLinkTM Type,
    bool HasGetter,
    bool HasSetter,
    bool IsSetterInitOnly,
    string[] Modifiers,
    string[] GetterModifiers,
    string[] SetterModifiers,
    string? ConstantValue,
    AttributeTM[] Attributes,
    string? SummaryDocComment,
    string? RemarksDocComment,
    string? ValueDocComment,
    string[] SeeAlsoDocComments,
    ExceptionTM[] Exceptions,
    TypeLinkTM? InheritedFrom,
    TypeLinkTM? BaseDeclaringType);
