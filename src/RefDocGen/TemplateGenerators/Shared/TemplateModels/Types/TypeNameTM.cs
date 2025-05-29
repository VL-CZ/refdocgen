using RefDocGen.TemplateGenerators.Shared.Languages;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;

/// <summary>
/// Represents the template model for a type, including its name and other data.
/// </summary>
/// <param name="Id">Unique identifier of the type.</param>
/// <param name="TypeKindName">Name of the type kind.</param>
/// <param name="Name">Name of the type.</param>
/// <param name="DocComment">Documentation comment for the type.</param>
public record TypeNameTM(string Id, string TypeKindName, LanguageSpecificData<string> Name, string? DocComment);
