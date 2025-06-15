namespace RefDocGen.TemplateProcessors.Shared.TemplateModels.Language;

/// <summary>
/// Template model representing a language used for displaying syntax.
/// </summary>
/// <param name="Name">Name of the language to by displayed.</param>
/// <param name="Id">Identifier of the language.</param>
/// <param name="ComponentsFolderName">Name of the folder inside the 'TemplateProcessors/Default/Templates/Components/LanguageSpecific' directory that contains the language-specific components.</param>
public record LanguageTM(string Name, string Id, string ComponentsFolderName);
