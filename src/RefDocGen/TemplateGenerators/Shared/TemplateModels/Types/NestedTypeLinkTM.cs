namespace RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;

/// <summary>
/// Represents the template model for a nested type, including a link to its definition.
/// </summary>
/// <param name="TypeKindName">Name of the type kind.</param>
/// <param name="TypeLink">Template model of the type including its link.</param>
public record NestedTypeLinkTM(string TypeKindName, TypeLinkTM TypeLink);
