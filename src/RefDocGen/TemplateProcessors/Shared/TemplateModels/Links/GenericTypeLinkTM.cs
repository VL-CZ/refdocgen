namespace RefDocGen.TemplateProcessors.Shared.TemplateModels.Links;

/// <summary>
/// The template model representing a link to generic type definition, including a URL to the definition of all generic parameters.
/// </summary>
/// <param name="TypeLink"><see cref="CodeLinkTM"/> representing the type, excluding its generic paramters.</param>
/// <param name="TypeParameters">
/// <see cref="GenericTypeLinkTM"> instance representing the type paramters of the type.</see>
/// </param>
public record GenericTypeLinkTM(CodeLinkTM TypeLink, GenericTypeLinkTM[] TypeParameters);
