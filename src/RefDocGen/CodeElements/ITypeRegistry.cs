using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Enum;

namespace RefDocGen.CodeElements;

/// <summary>
/// Represents a registry of declared types.
/// </summary>
public interface ITypeRegistry
{
    /// <summary>
    /// A collection of declared value, reference and interface types.
    /// </summary>
    IEnumerable<IObjectTypeData> ObjectTypes { get; }

    /// <summary>
    /// A collection of declared enum types.
    /// </summary>
    IEnumerable<IEnumTypeData> Enums { get; }
}
