namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

/// <summary>
/// Represents the template model for a user-documented exception (using the 'exception' XML tag).
/// </summary>
/// <param name="Name">Fully qualified name of the exception.</param>
/// <param name="DocComment">Documentation comment provided to the exception.</param>
public record ExceptionTM(string Name, string DocComment);
