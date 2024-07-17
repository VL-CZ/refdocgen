namespace RefDocGen.TemplateGenerators.Default.TemplateModels;

/// <summary>
/// Represents the template model for a method parameter.
/// </summary>
/// <param name="Name">Name of the method.</param>
/// <param name="Parameters">Collection of the method parameters.</param>
/// <param name="ReturnType">Return type of the method.</param>
/// <param name="DocComment">Documentation comment for the method,</param>
/// <param name="Modifiers">Collection of modifiers for the method (e.g. private, abstract, virtual, etc.)</param>
public record MethodTemplateModel(string Name, IEnumerable<MethodParameterTemplateModel> Parameters, string ReturnType, string DocComment, IEnumerable<string> Modifiers);
