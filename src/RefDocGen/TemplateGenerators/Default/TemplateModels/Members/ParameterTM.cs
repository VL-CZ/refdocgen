namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

/// <summary>
/// Represents the template model for a method parameter.
/// </summary>
/// <param name="Name">Name of the method parameter.</param>
/// <param name="Type">Type of the method parameter.</param>
/// <param name="DocComment">Documentation comment for the method parameter.</param>
/// <param name="Modifiers">Collection of the parameter modifiers (e.g. out, ref, etc.).</param>
/// <param name="DefaultValue">
/// Default value of the parameter as a string.
/// <para>
/// <see langword="null"/> if the parameter has no default value.
/// </para>
/// </param>
public record ParameterTM(string Name, string Type, string DocComment, IEnumerable<string> Modifiers, string? DefaultValue);
