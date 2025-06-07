namespace RefDocGen.TemplateProcessors.Shared.TemplateModels.Language;

/// <summary>
/// Template model representing a language used for displaying syntax.
/// </summary>
/// <param name="Name">Name of the language to by displayed.</param>
/// <param name="Id">Identifier of the language.</param>
public record LanguageTM(string Name, string Id);
