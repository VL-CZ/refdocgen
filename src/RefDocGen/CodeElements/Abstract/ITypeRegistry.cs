using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Delegate;
using RefDocGen.CodeElements.Abstract.Types.Enum;
using RefDocGen.CodeElements.Concrete.Members;
using System.Diagnostics.CodeAnalysis;

namespace RefDocGen.CodeElements.Abstract;

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
    /// Tries to get the type with the given ID.
    /// </summary>
    /// <param name="typeId">ID of the type to search for.</param>
    /// <param name="type">The type with the given ID, if found. <see langword="null"/> otherwise.</param>
    /// <returns><c>true</c> if the type is found in the registry, <c>false</c> otherwise.</returns>
    bool TryGetType(string typeId, [MaybeNullWhen(false)] out ITypeDeclaration? type);
}
