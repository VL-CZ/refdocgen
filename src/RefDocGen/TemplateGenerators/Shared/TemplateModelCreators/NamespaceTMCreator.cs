using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Types;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.CodeElements.Types.Abstract.Enum;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Namespaces;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Shared.Tools;
using RefDocGen.TemplateGenerators.Shared.Tools.Names;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the namespaces of a program.
/// </summary>
internal class NamespaceTMCreator
{
    /// <summary>
    /// Creates an enumerable of <see cref="NamespaceTM"/> instances based on the provided <see cref="IObjectTypeData"/>.
    /// </summary>
    /// <param name="namespaceData">The <see cref="IObjectTypeData"/> instance representing the types.</param>
    /// <returns>An enumerable of <see cref="NamespaceTM"/> instances based on the provided <paramref name="namespaceData"/>.</returns>
    internal static NamespaceTM GetFrom(NamespaceData namespaceData)
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
            namespaceTypes[typeKind] = namespaceData.ObjectTypes // select the types of the given kind, ordered by their name
                .Where(t => t.Kind == typeKind)
                .Select(GetFrom)
                .OrderBy(t => t.Name);
        }

        // get namespace enums
        var namespaceEnums = namespaceData.Enums
            .Select(GetFrom)
            .OrderBy(e => e.Name);

        // get namespace delegates
        var namespaceDelegates = namespaceData.Delegates
            .Select(GetFrom)
            .OrderBy(d => d.Name);

        return new NamespaceTM(
            namespaceData.Name,
            namespaceTypes[TypeKind.Class],
            namespaceTypes[TypeKind.ValueType],
            namespaceTypes[TypeKind.Interface],
            namespaceEnums,
            namespaceDelegates
            );
    }

    internal static AssemblyTM GetFrom(AssemblyData assembly)
    {
        return new AssemblyTM(assembly.Name, assembly.Namespaces.Select(GetFrom));
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
        return new TypeNameTM(enumData.Id, "enum", CSharpTypeName.Of(enumData), enumData.SummaryDocComment.Value);
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
