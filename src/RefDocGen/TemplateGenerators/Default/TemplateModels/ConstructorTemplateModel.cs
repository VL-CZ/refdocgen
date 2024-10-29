namespace RefDocGen.TemplateGenerators.Default.TemplateModels;

/// <summary>
/// Represents the template model of a constructor.
/// </summary>
/// <param name="Parameters">Collection of the constructor parameters.</param>
/// <param name="DocComment">Documentation comment for the constructor.</param>
/// <param name="Modifiers">Collection of the constructor modifiers (e.g. private, static, etc.)</param>
public record ConstructorTemplateModel(IEnumerable<ParameterTemplateModel> Parameters, string DocComment, IEnumerable<string> Modifiers);
