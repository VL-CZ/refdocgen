using RefDocGen.TemplateGenerators.Shared.Languages;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;

/// <summary>
/// Represents the template model for a type, including a URL to its definition.
/// </summary>
/// <param name="Name">Name of the type.</param>
/// <param name="Url">
/// URL of the type definition page.
/// <c>null</c> if the type definition page isn't found.
/// </param>
public record TypeLinkTM(LanguageSpecificData<string> Name, string? Url);


/// <summary>
/// Represents the template model for a type, including a URL to its definition.
/// </summary>
/// <param name="Name">Name of the type.</param>
/// <param name="Url">
/// URL of the type definition page.
/// <c>null</c> if the type definition page isn't found.
/// </param>
public record GenericTypeLinkTM(string Name, string? Url, GenericTypeLinkTM[] TypeParameters);
