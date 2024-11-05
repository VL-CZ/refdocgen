namespace RefDocGen.TemplateGenerators.Default.TemplateModels;

/// <summary>
/// Represents the template model for a method parameter.
/// </summary>
/// <param name="Name">Name of the method parameter.</param>
/// <param name="Type">Type of the method parameter.</param>
/// <param name="DocComment">Documentation comment for the method parameter.</param>
/// <param name="Modifiers">Collection of the parameter modifiers (e.g. out, ref, etc.).</param>
/// <param name="IsOptional">Indicates whether the parameter is optional.</param>
public record ParameterTemplateModel(string Name, string Type, string DocComment, IEnumerable<string> Modifiers, bool IsOptional);
