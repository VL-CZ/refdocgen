using RefDocGen.CodeElements.TypeRegistry;

namespace RefDocGen.TemplateGenerators;

/// <summary>
/// Defines methods for processing templates using the <see cref="ITypeRegistry"/> data.
/// </summary>
public interface ITemplateProcessor
{
    /// <summary>
    /// Generate the templates and populate them using the provided type data.
    /// </summary>
    /// <param name="typeRegistry">A registry of declared types to be used in the templates.</param>
    /// <param name="outputDirectory"></param>
    void ProcessTemplates(ITypeRegistry typeRegistry, string outputDirectory);
}
