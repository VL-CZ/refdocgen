namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

/// <summary>
/// Represents the template model of a constructor.
/// </summary>
/// <param name="Parameters">Collection of the constructor parameters.</param>
/// <param name="SummaryDocComment">'summary' documentation comment for the constructor.</param>
/// <param name="RemarksDocComment">'remarks' documentation comment for the constructor.</param>
/// <param name="Modifiers">Collection of the constructor modifiers (e.g. private, static, etc.)</param>
public record ConstructorTM(
    IEnumerable<ParameterTM> Parameters,
    string SummaryDocComment,
    string RemarksDocComment,
    IEnumerable<string> Modifiers);
