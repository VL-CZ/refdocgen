using RefDocGen.MemberData.Abstract;

namespace RefDocGen.TemplateGenerators;

/// <summary>
/// Defines methods for generating templates using the <see cref="IClassData"/> objects.
/// </summary>
public interface ITemplateGenerator
{
    /// <summary>
    /// Generate the templates and populate them using the provided <paramref name="classes"/> data.
    /// </summary>
    /// <param name="classes">A readonly list of <see cref="IClassData"/> objects containing the data to be used in the templates.</param>
    void GenerateTemplates(IReadOnlyList<IClassData> classes);
}
