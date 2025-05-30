namespace RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;

/// <summary>
/// Represents the template model for a user-documented exception (using the <c>exception</c> XML tag).
/// </summary>
/// <param name="Type">Type of the exception.</param>
/// <param name="DocComment">Documentation comment provided to the exception. <c>null</c> if the doc comment is not provided.</param>
public record ExceptionTM(CodeLinkTM Type, string? DocComment);
