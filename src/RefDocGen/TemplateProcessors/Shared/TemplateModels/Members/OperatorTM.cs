using RefDocGen.TemplateProcessors.Shared.Languages;
using RefDocGen.TemplateProcessors.Shared.TemplateModels.Links;
using RefDocGen.TemplateProcessors.Shared.TemplateModels.Types;

namespace RefDocGen.TemplateProcessors.Shared.TemplateModels.Members;

/// <summary>
/// Represents the template model for a operator.
/// </summary>
/// <param name="Id">Identifier of the operator.</param>
/// <param name="Name">Name of the operator.</param>
/// <param name="Parameters">Collection of the operator parameters.</param>
/// <param name="ReturnType">Return type of the operator.</param>
/// <param name="ReturnsVoid">Checks whether the return type of the operator is <seealso cref="void"/>.</param>
/// <param name="SummaryDocComment"><c>summary</c> documentation comment for the operator. <c>null</c> if the doc comment is not provided.</param>
/// <param name="RemarksDocComment"><c>remarks</c> documentation comment for the operator. <c>null</c> if the doc comment is not provided.</param>
/// <param name="ReturnsDocComment">Documentation comment for the operator's return value. <c>null</c> if the doc comment is not provided.</param>
/// <param name="Modifiers">Collection of modifiers for the operator (e.g. private, abstract, virtual, etc.)</param>
/// <param name="TypeParameters">Template models of the generic type parameters declared in the operator.</param>
/// <param name="SeeAlsoDocComments">Collection of <c>seealso</c> documentation comments for the operator.</param>
/// <param name="Exceptions">
/// A collection of user-documented exceptions (using the <c>exception</c> XML tag) that the operator might throw.
/// </param>
/// <param name="Attributes">Array of attributes applied to the operator.</param>
/// <param name="InheritedFrom">
/// If the operator is inherited, this represents the type from which it originates.
/// <c>null</c> if the operator is not inherited.
/// </param>
/// <param name="BaseDeclaringType">
/// If the operator overrides another member, the base type that originally declared the operator is returned.
/// <c>null</c> if the operator doesn't override anything.
/// </param>
/// <param name="ExplicitInterfaceType">
/// If the operator is an explicit implementation, the type of the interface that explicitly declared the operator is returned.
/// <c>null</c> if the operator is not an explicit implementation.
/// </param>
/// <param name="ImplementedInterfaces">
/// Returns the types of the interfaces, whose part of contract this operator implements.
/// </param>
/// <param name="IsConversionOperator">Indicates whether the operator is a conversion operator.</param>
public record OperatorTM(
    string Id,
    LanguageSpecificData<string> Name,
    ParameterTM[] Parameters,
    TypeParameterTM[] TypeParameters,
    GenericTypeLinkTM ReturnType,
    bool ReturnsVoid,
    bool IsConversionOperator,
    LanguageSpecificData<string[]> Modifiers,
    AttributeTM[] Attributes,
    string? SummaryDocComment,
    string? RemarksDocComment,
    string? ReturnsDocComment,
    string[] SeeAlsoDocComments,
    ExceptionTM[] Exceptions,
    CodeLinkTM? InheritedFrom,
    CodeLinkTM? BaseDeclaringType,
    CodeLinkTM? ExplicitInterfaceType,
    CodeLinkTM[] ImplementedInterfaces);
