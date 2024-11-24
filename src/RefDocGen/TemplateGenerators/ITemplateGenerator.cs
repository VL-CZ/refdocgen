using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Abstract.Types;

namespace RefDocGen.TemplateGenerators;

/// <summary>
/// Defines methods for generating templates using the <see cref="IObjectTypeData"/> objects.
/// </summary>
public interface ITemplateGenerator
{
    /// <summary>
    /// Generate the templates and populate them using the provided type data.
    /// </summary>
    /// <param name="types">A readonly list of <see cref="IObjectTypeData"/> objects containing the data to be used in the templates.</param>
    void GenerateTemplates(ITypeDeclarations typeDeclarations);
}
