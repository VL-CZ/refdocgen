using RefDocGen.TemplateProcessors.Shared.Languages;

namespace RefDocGen.TemplateProcessors.Shared.TemplateModels.Links;

/// <summary>
/// The template model representing a link to type or member definition, including its URL.
/// </summary>
/// <param name="Name">Name of the referenced type.</param>
/// <param name="Url">
/// URL of the type or member definition page.
/// <c>null</c> if the definition page isn't found.
/// </param>
/// <param name="MemberName">Name of the referenced member. <c>null</c>, if the link references a type.</param>
public record CodeLinkTM(LanguageSpecificData<string> Name, string? Url, string? MemberName = null);
