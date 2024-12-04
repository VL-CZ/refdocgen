using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Delegate;
using RefDocGen.CodeElements.Abstract.Types.Enum;

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
}
