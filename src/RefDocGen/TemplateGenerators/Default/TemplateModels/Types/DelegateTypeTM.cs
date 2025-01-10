using RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Types;

/// <summary>
/// Represents the template model for a delegate.
/// </summary>
/// <param name="Id">Unique identifier of the delegate.</param>
/// <param name="Name">Name of the delegate.</param>
/// <param name="Namespace">Namespace containing the delegate.</param>
/// <param name="Modifiers">Collection of modifiers for the delegate (e.g., public, abstract).</param>
/// <param name="ReturnType">Return type of the delegate method.</param>
/// <param name="ReturnsVoid">Checks whether the return type of the delegate method is <seealso cref="void"/>.</param>
/// <param name="SummaryDocComment">'summary' documentation comment for the delegate.</param>
/// <param name="RemarksDocComment">'remarks' documentation comment for the delegate.</param>
/// <param name="ReturnsDocComment">Documentation comment for the delegate method's return value.</param>
/// <param name="Parameters">Collection of the delegate method parameters.</param>
/// <param name="TypeParameters">Template models of the generic type parameters declared in the delegate.</param>
/// <param name="Exceptions">
/// A collection of user-documented exceptions (using the 'exception' XML tag) that the delegate might throw.
/// </param>
/// <param name="SeeAlsoDocComments">Collection of <c>seealso</c> documentation comments for the delegate.</param>
public record DelegateTypeTM(
    string Id,
    string Name,
    string Namespace,
    string SummaryDocComment,
    string RemarksDocComment,
    IEnumerable<string> Modifiers,
    string ReturnsDocComment,
    TypeLinkTM ReturnType,
    bool ReturnsVoid,
    ParameterTM[] Parameters,
    TypeParameterTM[] TypeParameters,
    string[] SeeAlsoDocComments,
    ExceptionTM[] Exceptions) : ITemplateModelWithId;
