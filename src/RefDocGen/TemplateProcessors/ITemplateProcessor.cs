using Microsoft.Extensions.Logging;
using RefDocGen.CodeElements.TypeRegistry;

namespace RefDocGen.TemplateProcessors;

/// <summary>
/// Defines methods for processing templates using the <see cref="ITypeRegistry"/> data.
/// </summary>
public interface ITemplateProcessor
{
    /// <summary>
    /// Process the templates and populate them using the provided type data.
    /// </summary>
    /// <param name="typeRegistry">A registry of declared type data to be used in the templates.</param>
    /// <param name="outputDirectory">The directory, where the ouput will be stored.</param>
    /// <param name="logger">A logger instance.</param>
    /// <param name="projectName">Name of the assembly/project/solution to be documented (without file extension).</param>
    void ProcessTemplates(ITypeRegistry typeRegistry, string outputDirectory, string projectName, ILogger logger);
}
