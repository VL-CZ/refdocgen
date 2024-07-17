namespace RefDocGen.TemplateGenerators;

/// <summary>
/// Represents any template model that can be identified by its name
/// </summary>
public interface INamedTemplateModel
{
    /// <summary>
    /// Name of the template model.
    /// </summary>
    string Name { get; }
}
