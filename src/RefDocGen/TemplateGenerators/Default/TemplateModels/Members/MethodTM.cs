namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

/// <summary>
/// Represents the template model for a method.
/// </summary>
/// <param name="Name">Name of the method.</param>
/// <param name="Parameters">Collection of the method parameters.</param>
/// <param name="ReturnType">Return type of the method.</param>
/// <param name="ReturnsVoid">Checks whether the return type of the method is <seealso cref="void"/>.</param>
/// <param name="SummaryDocComment">'summary' documentation comment for the method.</param>
/// <param name="RemarksDocComment">'remarks' documentation comment for the method.</param>
/// <param name="ReturnsDocComment">Documentation comment for the method's return value.</param>
/// <param name="Modifiers">Collection of modifiers for the method (e.g. private, abstract, virtual, etc.)</param>
/// <param name="Exceptions">
/// A collection of user-documented exceptions (using the 'exception' XML tag) that the method might throw.
/// </param>
public record MethodTM(
    string Name,
    IEnumerable<ParameterTM> Parameters,
    string ReturnType,
    bool ReturnsVoid,
    string SummaryDocComment,
    string RemarksDocComment,
    string ReturnsDocComment,
    IEnumerable<string> Modifiers,
    IEnumerable<ExceptionTM> Exceptions);
