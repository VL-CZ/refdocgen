namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

/// <summary>
/// Represents the template model for a operator.
/// </summary>
/// <param name="Name">Name of the operator.</param>
/// <param name="Parameters">Collection of the operator parameters.</param>
/// <param name="ReturnType">Return type of the operator.</param>
/// <param name="ReturnsVoid">Checks whether the return type of the operator is <seealso cref="void"/>.</param>
/// <param name="DocComment">Documentation comment for the operator.</param>
/// <param name="ReturnsDocComment">Documentation comment for the operator's return value.</param>
/// <param name="Modifiers">Collection of modifiers for the operator (e.g. private, abstract, virtual, etc.)</param>
public record OperatorTM(string Name, IEnumerable<ParameterTM> Parameters, string ReturnType, bool ReturnsVoid, string DocComment, string ReturnsDocComment, IEnumerable<string> Modifiers);
