using RefDocGen.CodeElements.Abstract;

namespace RefDocGen.TemplateGenerators;

/// <summary>
/// Defines methods for generating templates using the <see cref="ITypeRegistry"/> data.
/// </summary>
public interface ITemplateGenerator
{
    /// <summary>
    /// Generate the templates and populate them using the provided type data.
    /// </summary>
    /// <param name="typeRegistry">A registry of declared types to be used in the templates.</param>
    void GenerateTemplates(ITypeRegistry typeRegistry);
}
