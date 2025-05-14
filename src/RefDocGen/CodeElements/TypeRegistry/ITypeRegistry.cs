using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.CodeElements.Types.Abstract.Enum;

namespace RefDocGen.CodeElements;

/// <summary>
/// Represents a registry of the declared types.
/// </summary>
public interface ITypeRegistry
{
    /// <summary>
    /// A collection of the declared value, reference and interface types.
    /// </summary>
    IEnumerable<IObjectTypeData> ObjectTypes { get; }

    /// <summary>
    /// A collection of the declared enum types.
    /// </summary>
    IEnumerable<IEnumTypeData> Enums { get; }

    /// <summary>
    /// A collection of the declared delegate types.
    /// </summary>
    IEnumerable<IDelegateTypeData> Delegates { get; }

    /// <summary>
    /// Gets the type with the given ID.
    /// </summary>
    /// <param name="typeId">ID of the type to return.</param>
    /// <returns>The type with the given ID, if found. <see langword="null"/> otherwise.</returns>
    ITypeDeclaration? GetDeclaredType(string typeId);

    IEnumerable<AssemblyData> GetAssemblies();

    IEnumerable<NamespaceData> GetNamespaces();
}
