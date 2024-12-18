using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Delegate;
using RefDocGen.CodeElements.Abstract.Types.Enum;
using RefDocGen.CodeElements.Abstract;
using RefDocGen.CodeElements.Concrete.Types.Enum;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Concrete.Types.Delegate;
using RefDocGen.CodeElements.Abstract.Types.TypeName;

namespace RefDocGen.CodeElements.Concrete;

/// <summary>
/// Represents a registry of declared types.
/// </summary>
/// <param name="ObjectTypes">A collection of the declared value, reference and interface types, indexed by their IDs.</param>
/// <param name="Enums">A collection of the declared enum types, indexed by their IDs.</param>
/// <param name="Delegates">A collection of the declared delegate types, indexed by their IDs.</param>
internal record TypeRegistry(
    IReadOnlyDictionary<string, ObjectTypeData> ObjectTypes,
    IReadOnlyDictionary<string, EnumTypeData> Enums,
    IReadOnlyDictionary<string, DelegateTypeData> Delegates
    ) : ITypeRegistry
{
    /// <inheritdoc/>
    IEnumerable<IObjectTypeData> ITypeRegistry.ObjectTypes => ObjectTypes.Values;

    /// <inheritdoc/>
    IEnumerable<IEnumTypeData> ITypeRegistry.Enums => Enums.Values;

    /// <inheritdoc/>
    IEnumerable<IDelegateTypeData> ITypeRegistry.Delegates => Delegates.Values;
}
