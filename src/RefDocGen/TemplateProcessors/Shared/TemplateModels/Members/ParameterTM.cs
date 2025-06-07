using RefDocGen.TemplateProcessors.Shared.Languages;
using RefDocGen.TemplateProcessors.Shared.TemplateModels.Links;
using RefDocGen.TemplateProcessors.Shared.TemplateModels.Types;

namespace RefDocGen.TemplateProcessors.Shared.TemplateModels.Members;

/// <summary>
/// Represents the template model for a method parameter.
/// </summary>
/// <param name="Name">Name of the method parameter.</param>
/// <param name="Type">Type of the method parameter.</param>
/// <param name="DocComment">Documentation comment for the method parameter. <c>null</c> if the doc comment is not provided.</param>
/// <param name="Modifiers">Collection of the parameter modifiers (e.g. <c>out</c>, <c>ref</c>, etc.).</param>
/// <param name="DefaultValue">
/// Default value of the parameter as a string.
/// <para>
/// <see langword="null"/> if the parameter has no default value.
/// </para>
/// </param>
/// <param name="Attributes">Array of attributes applied to the parameter.</param>
public record ParameterTM(
    string Name,
    GenericTypeLinkTM Type,
    LanguageSpecificData<string[]> Modifiers,
    AttributeTM[] Attributes,
    LanguageSpecificData<string>? DefaultValue,
    string? DocComment);
