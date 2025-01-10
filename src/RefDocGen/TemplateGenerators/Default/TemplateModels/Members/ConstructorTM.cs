namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

/// <summary>
/// Represents the template model of a constructor.
/// </summary>
/// <param name="Parameters">Collection of the constructor parameters.</param>
/// <param name="SummaryDocComment"><c>summary</c> documentation comment for the constructor.</param>
/// <param name="RemarksDocComment"><c>remarks</c> documentation comment for the constructor.</param>
/// <param name="Modifiers">Collection of the constructor modifiers (e.g. private, static, etc.)</param>
/// <param name="SeeAlsoDocComments">Collection of <c>seealso</c> documentation comments for the constructor.</param>
/// <param name="Exceptions">
/// A collection of user-documented exceptions (using the <c>exception</c> XML tag) the constructor might throw.
/// </param>
public record ConstructorTM(
    IEnumerable<ParameterTM> Parameters,
    string SummaryDocComment,
    string RemarksDocComment,
    IEnumerable<string> Modifiers,
    IEnumerable<string> SeeAlsoDocComments,
    ExceptionTM[] Exceptions);
