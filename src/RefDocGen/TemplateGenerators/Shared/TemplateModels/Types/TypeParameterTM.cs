using RefDocGen.TemplateGenerators.Shared.Languages;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Links;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;

/// <summary>
/// Represents the template model for a generic type parameter declaration.
/// </summary>
/// <param name="Name">Name of the type parameter.</param>
/// <param name="DocComment">Documentation comment for the type parameter. <c>null</c> if the doc comment is not provided.</param>
/// <param name="Modifiers">Collection of the type parameter modifiers (e.g. in, out, etc.).</param>
/// <param name="TypeConstraints">Collection of the type constraint names of the type parameter.</param>
/// <param name="SpecialConstraints">Collection of the special constraints of the type parameter.</param>
public record TypeParameterTM(
    string Name,
    string? DocComment,
    LanguageSpecificData<string[]> Modifiers,
    GenericTypeLinkTM[] TypeConstraints,
    LanguageSpecificData<string[]> SpecialConstraints);
