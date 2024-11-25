using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Enum;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Concrete.Types.Enum;

namespace RefDocGen.CodeElements;

/// <summary>
/// Represents a registry of declared types.
/// </summary>
/// <param name="ObjectTypes">A collection of declared value, reference and interface types, indexed by their IDs.</param>
/// <param name="Enums">A collection of declared enum types, indexed by their IDs.</param>
internal record TypeRegistry(IReadOnlyDictionary<string, ObjectTypeData> ObjectTypes, IReadOnlyDictionary<string, EnumTypeData> Enums) : ITypeRegistry
{
    /// <inheritdoc/>
    IEnumerable<IObjectTypeData> ITypeRegistry.ObjectTypes => ObjectTypes.Values;

    /// <inheritdoc/>
    IEnumerable<IEnumTypeData> ITypeRegistry.Enums => Enums.Values;
}
