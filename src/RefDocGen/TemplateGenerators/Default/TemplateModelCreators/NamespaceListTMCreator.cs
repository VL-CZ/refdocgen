using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Namespaces;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;
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
    internal static IEnumerable<NamespaceTM> GetFrom(ITypeDeclarations typeData)
    {
        var groupedTypes = typeData.Types.GroupBy(t => t.Namespace);
        var groupedEnums = typeData.Enums.GroupBy(e => e.Namespace);

        var namespaceTemplateModels = new List<NamespaceTM>();

        foreach (var typeGroup in groupedTypes)
        {
            string? namespaceName = typeGroup.Key;

            if (namespaceName is not null)
            {
                Dictionary<TypeKind, IEnumerable<TypeNameTM>> namespaceTypes = new()
                {
                    [TypeKind.Class] = [],
                    [TypeKind.ValueType] = [],
                    [TypeKind.Interface] = [],
                };

                foreach (var typeKind in namespaceTypes.Keys)
                {
                    namespaceTypes[typeKind] = typeGroup // select the types of the given kind, ordered by their name
                        .Where(t => t.Kind == typeKind)
                        .Select(GetFrom)
                        .OrderBy(t => t.Name);
                }

                // TODO: better performance
                var namespaceEnums = typeData.Enums
                    .Where(e => e.Namespace == namespaceName)
                    .Select(GetFrom)
                    .OrderBy(e => e.Name);

                namespaceTemplateModels.Add(
                    new NamespaceTM(
                        namespaceName,
                        namespaceTypes[TypeKind.Class],
                        namespaceTypes[TypeKind.ValueType],
                        namespaceTypes[TypeKind.Interface],
                        namespaceEnums
                        )
                );
            }
        }

        return namespaceTemplateModels;
    }

    /// <summary>
    /// Creates a <see cref="TypeNameTM"/> instance based on the provided <see cref="ITypeData"/> object.
    /// </summary>
    /// <param name="type">The <see cref="ITypeData"/> instance representing the type.</param>
    /// <returns>A <see cref="TypeNameTM"/> instance based on the provided <paramref name="type"/>.</returns>
    private static TypeNameTM GetFrom(ITypeData type)
    {
        return new TypeNameTM(type.Id, type.Kind.GetName(), CSharpTypeName.Of(type), type.DocComment.Value);
    }

    private static TypeNameTM GetFrom(IEnumData enumData)
    {
        return new TypeNameTM(enumData.Id, "enum", enumData.ShortName, enumData.DocComment.Value);
    }
}
