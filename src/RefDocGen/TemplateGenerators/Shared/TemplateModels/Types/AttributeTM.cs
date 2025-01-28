namespace RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;

/// <summary>
/// Represents the template model for an attribute assigned to a type or member.
/// </summary>
/// <param name="Type">Type of the attribute.</param>
/// <param name="ConstructorArguments">Array of attribute's constructor arguments.</param>
/// <param name="NamedArguments">Array of attribute's named arguments.</param>
public record AttributeTM(
    TypeLinkTM Type,
    string?[] ConstructorArguments,
    NamedAttributeArgumentTM[] NamedArguments);

/// <summary>
/// Represents the template model for an attribute's named argument.
/// </summary>
/// <param name="Name">Name of the argument.</param>
/// <param name="Value">Value of the argument.</param>
public record NamedAttributeArgumentTM(TypeLinkTM Name, string? Value);
