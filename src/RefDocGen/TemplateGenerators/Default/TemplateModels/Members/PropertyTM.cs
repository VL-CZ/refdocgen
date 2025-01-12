using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;

namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

/// <summary>
/// Represents the template model for a property.
/// </summary>
/// <param name="Name">Name of the property.</param>
/// <param name="Type">Type of the property.</param>
/// <param name="SummaryDocComment"><c>summary</c> documentation comment for the property. <c>null</c> if the doc comment is not provided.</param>
/// <param name="RemarksDocComment"><c>remarks</c> documentation comment for the property. <c>null</c> if the doc comment is not provided.</param>
/// <param name="ValueDocComment"><c>value</c> documentation comment for the property. <c>null</c> if the doc comment is not provided.</param>
/// <param name="Modifiers">Collection of property modifiers (e.g. public, static, etc.)</param>
/// <param name="HasGetter">Checks if the property has getter.</param>
/// <param name="HasSetter">Checks if the property has setter.</param>
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
public record PropertyTM(
    string Name,
    TypeLinkTM Type,
    string? SummaryDocComment,
    string? RemarksDocComment,
    string? ValueDocComment,
    IEnumerable<string> Modifiers,
    bool HasGetter,
    bool HasSetter,
    string[] GetterModifiers,
    string[] SetterModifiers,
    string[] SeeAlsoDocComments,
    ExceptionTM[] Exceptions,
    string? ConstantValue);
