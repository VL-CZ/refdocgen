using RefDocGen.TemplateGenerators.Shared.Languages;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;

/// <summary>
/// Represents the template model for a type declaration name.
/// </summary>
public interface ITypeDeclarationNameTM
{
    /// <summary>
    /// The name of the type.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Collection of modifiers for the type (e.g., public, abstract).
    /// </summary>
    LanguageSpecificData<string[]> Modifiers { get; }

    /// <summary>
    /// Template models of the generic type parameters contained in the type.
    /// </summary>
    TypeParameterTM[] TypeParameters { get; }

    /// <summary>
    /// The type that contains the declaration of this type.
    /// <para>
    /// <c>null</c> if the type is not nested.
    /// </para>
    /// </summary>
    TypeLinkTM? DeclaringType { get; }
}

