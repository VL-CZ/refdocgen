namespace RefDocGen.TemplateProcessors;

/// <summary>
/// Represents any template model that can be identified by its ID.
/// </summary>
public interface ITemplateModelWithId
{
    /// <summary>
    /// ID of the template model.
    /// </summary>
    string Id { get; }
}
