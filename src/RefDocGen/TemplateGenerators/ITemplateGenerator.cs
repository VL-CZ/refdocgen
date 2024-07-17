using RefDocGen.MemberData;

namespace RefDocGen.TemplateGenerators;

/// <summary>
/// Defines methods for generating templates using the <see cref="ClassData"/> objects.
/// </summary>
public interface ITemplateGenerator
{
    /// <summary>
    /// Generate the templates and populate them using the provided <paramref name="classes"/> data.
    /// </summary>
    /// <param name="classes">An array of <see cref="ClassData"/> objects containing the data to be used in the templates.</param>
    void GenerateTemplates(ClassData[] classes);
}
