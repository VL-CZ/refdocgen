using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;

namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

/// <summary>
/// Represents the template model for a method.
/// </summary>
/// <param name="Id">Identifier of the method.</param>
/// <param name="Name">Name of the method.</param>
/// <param name="Parameters">Collection of the method parameters.</param>
/// <param name="ReturnType">Return type of the method.</param>
/// <param name="ReturnsVoid">Checks whether the return type of the method is <seealso cref="void"/>.</param>
/// <param name="SummaryDocComment"><c>summary</c> documentation comment for the method. <c>null</c> if the doc comment is not provided.</param>
/// <param name="RemarksDocComment"><c>remarks</c> documentation comment for the method. <c>null</c> if the doc comment is not provided.</param>
/// <param name="ReturnsDocComment">Documentation comment for the method's return value. <c>null</c> if the doc comment is not provided.</param>
/// <param name="Modifiers">Collection of modifiers for the method (e.g. private, abstract, virtual, etc.)</param>
/// <param name="TypeParameters">Template models of the generic type parameters declared in the method.</param>
/// <param name="SeeAlsoDocComments">Collection of <c>seealso</c> documentation comments for the method.</param>
/// <param name="Exceptions">
/// A collection of user-documented exceptions (using the <c>exception</c> XML tag) that the method might throw.
/// </param>
public record MethodTM(
    string Id,
    string Name,
    ParameterTM[] Parameters,
    TypeParameterTM[] TypeParameters,
    TypeLinkTM ReturnType,
    bool ReturnsVoid,
    string[] Modifiers,
    string? SummaryDocComment,
    string? RemarksDocComment,
    string? ReturnsDocComment,
    string[] SeeAlsoDocComments,
    ExceptionTM[] Exceptions);
