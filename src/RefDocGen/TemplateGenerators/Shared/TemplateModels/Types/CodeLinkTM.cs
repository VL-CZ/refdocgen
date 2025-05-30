using RefDocGen.TemplateGenerators.Shared.Languages;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;

/// <summary>
/// Represents the template model for a type or member, including a URL to its definition.
/// </summary>
/// <param name="Name">Name of the referenced type.</param>
/// <param name="Url">
/// URL of the type or member definition page.
/// <c>null</c> if the definition page isn't found.
/// </param>
/// <param name="MemberName">Name of the referenced member. <c>null</c>, if the link references a type.</param>
public record CodeLinkTM(LanguageSpecificData<string> Name, string? Url, string? MemberName = null);

/// <summary>
/// Represents the template model for a type, including a URL to its definition.
/// </summary>
/// <param name="Name">Name of the type.</param>
/// <param name="Url">
/// URL of the type definition page.
/// <c>null</c> if the type definition page isn't found.
/// </param>
public record GenericCodeLinkTM(CodeLinkTM TypeLink, GenericCodeLinkTM[] TypeParameters);
