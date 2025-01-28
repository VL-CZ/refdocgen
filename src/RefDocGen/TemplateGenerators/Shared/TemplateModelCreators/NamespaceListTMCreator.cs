using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Abstract;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Delegate;
using RefDocGen.CodeElements.Abstract.Types.Enum;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Namespaces;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Shared.Tools;
using RefDocGen.TemplateGenerators.Shared.Tools.Names;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the namespaces of a program.
/// </summary>
internal class NamespaceListTMCreator
{
    /// <summary>
    /// Creates an enumerable of <see cref="NamespaceTM"/> instances based on the provided <see cref="IObjectTypeData"/>.
    /// </summary>
    /// <param name="typeData">The <see cref="IObjectTypeData"/> instance representing the types.</param>
    /// <returns>An enumerable of <see cref="NamespaceTM"/> instances based on the provided <paramref name="typeData"/>.</returns>
    internal static IEnumerable<NamespaceTM> GetFrom(ITypeRegistry typeData)
    {
        var groupedTypes = typeData.ObjectTypes.ToLookup(t => t.Namespace);
        var groupedEnums = typeData.Enums.ToLookup(e => e.Namespace);
        var groupedDelegates = typeData.Delegates.ToLookup(e => e.Namespace);

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

                // get namespace classes, value types and interfaces
                foreach (var typeKind in namespaceTypes.Keys)
                {
                    namespaceTypes[typeKind] = typeGroup // select the types of the given kind, ordered by their name
                        .Where(t => t.Kind == typeKind)
                        .Select(GetFrom)
                        .OrderBy(t => t.Name);
                }

                // get namespace enums
                var namespaceEnums = groupedEnums[namespaceName]
                    .Select(GetFrom)
                    .OrderBy(e => e.Name);

                // get namespace delegates
                var namespaceDelegates = groupedDelegates[namespaceName]
                    .Select(GetFrom)
                    .OrderBy(d => d.Name);

                namespaceTemplateModels.Add(
                    new NamespaceTM(
                        namespaceName,
                        namespaceTypes[TypeKind.Class],
                        namespaceTypes[TypeKind.ValueType],
                        namespaceTypes[TypeKind.Interface],
                        namespaceEnums,
                        namespaceDelegates
                        )
                );
            }
        }

        return namespaceTemplateModels;
    }

    /// <summary>
    /// Creates a <see cref="TypeNameTM"/> instance based on the provided <see cref="IObjectTypeData"/> object.
    /// </summary>
    /// <param name="type">The <see cref="IObjectTypeData"/> instance representing the type.</param>
    /// <returns>A <see cref="TypeNameTM"/> instance based on the provided <paramref name="type"/>.</returns>
    private static TypeNameTM GetFrom(IObjectTypeData type)
    {
        return new TypeNameTM(type.Id, type.Kind.GetName(), CSharpTypeName.Of(type), type.SummaryDocComment.Value);
    }

    /// <summary>
    /// Creates a <see cref="TypeNameTM"/> instance based on the provided <see cref="IEnumTypeData"/> object.
    /// </summary>
    /// <param name="enumData">The <see cref="IEnumTypeData"/> instance representing the enum.</param>
    /// <returns>A <see cref="TypeNameTM"/> instance based on the provided <paramref name="enumData"/>.</returns>
    private static TypeNameTM GetFrom(IEnumTypeData enumData)
    {
        return new TypeNameTM(enumData.Id, "enum", enumData.ShortName, enumData.SummaryDocComment.Value);
    }

    /// <summary>
    /// Creates a <see cref="TypeNameTM"/> instance based on the provided <see cref="IDelegateTypeData"/> object.
    /// </summary>
    /// <param name="delegateData">The <see cref="IDelegateTypeData"/> instance representing the delegate.</param>
    /// <returns>A <see cref="TypeNameTM"/> instance based on the provided <paramref name="delegateData"/>.</returns>
    private static TypeNameTM GetFrom(IDelegateTypeData delegateData)
    {
        return new TypeNameTM(delegateData.Id, "delegate", CSharpTypeName.Of(delegateData), delegateData.SummaryDocComment.Value);
    }
}
