using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.TemplateGenerators.Default.TemplateModels;
using RefDocGen.TemplateGenerators.Tools;
using RefDocGen.TemplateGenerators.Tools.TypeName;

namespace RefDocGen.TemplateGenerators.Default.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the namespaces of a program.
/// </summary>
internal class NamespaceListTMCreator
{
    /// <summary>
    /// Creates an enumerable of <see cref="NamespaceTM"/> instances based on the provided <see cref="ITypeData"/>.
    /// </summary>
    /// <param name="typeData">The <see cref="ITypeData"/> instance representing the types.</param>
    /// <returns>An enumerable of <see cref="NamespaceTM"/> instances based on the provided <paramref name="typeData"/>.</returns>
    internal static IEnumerable<NamespaceTM> GetFrom(IReadOnlyList<ITypeData> typeData)
    {
        var groupedTypes = typeData.GroupBy(typeData => typeData.Namespace);

        var namespaceTemplateModels = new List<NamespaceTM>();

        foreach (var typeGroup in groupedTypes)
        {
            string? namespaceName = typeGroup.Key;
            if (namespaceName is not null)
            {
                var types = typeGroup
                    .Select(t => new TypeNameTemplateModel(
                        t.Id,
                        t.Kind.GetName(),
                        CSharpTypeName.Of(t),
                        t.DocComment.Value))
                    .OrderBy(t => t.Name);

                namespaceTemplateModels.Add(new NamespaceTM(namespaceName, types));
            }
        }

        return namespaceTemplateModels;
    }
}
