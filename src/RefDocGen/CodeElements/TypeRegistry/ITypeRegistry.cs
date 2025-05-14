using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.CodeElements.Types.Abstract.Enum;

namespace RefDocGen.CodeElements.TypeRegistry;

/// <summary>
/// Represents a registry of the declared types.
/// </summary>
public interface ITypeRegistry
{
    /// <summary>
    /// A collection of all declared class, struct and interface types.
    /// </summary>
    IEnumerable<IObjectTypeData> ObjectTypes { get; }

    /// <summary>
    /// A collection of all declared enum types.
    /// </summary>
    IEnumerable<IEnumTypeData> Enums { get; }

    /// <summary>
    /// A collection of all declared delegate types.
    /// </summary>
    IEnumerable<IDelegateTypeData> Delegates { get; }

    /// <summary>
    /// Gets the type with the given ID.
    /// </summary>
    /// <param name="typeId">ID of the type to return.</param>
    /// <returns>The type with the given ID, if found. <see langword="null"/> otherwise.</returns>
    ITypeDeclaration? GetDeclaredType(string typeId);

    /// <summary>
    /// A collection of all assemblies contained in the processed DLLs.
    /// </summary>
    IEnumerable<AssemblyData> Assemblies { get; }

    /// <summary>
    /// A collection of all namespaces contained in the processed DLLs.
    /// </summary>
    IEnumerable<NamespaceData> Namespaces { get; }
}
