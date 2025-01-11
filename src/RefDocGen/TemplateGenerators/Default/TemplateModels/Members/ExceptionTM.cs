using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;

namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

/// <summary>
/// Represents the template model for a user-documented exception (using the <c>exception</c> XML tag).
/// </summary>
/// <param name="Type">Type of the exception.</param>
/// <param name="DocComment">Documentation comment provided to the exception. <c>null</c> if the doc comment is not provided.</param>
public record ExceptionTM(TypeLinkTM Type, string? DocComment);
