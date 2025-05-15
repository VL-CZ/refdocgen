using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.CodeElements.Types.Abstract.Enum;

namespace RefDocGen.CodeElements;

/// <summary>
/// Represents a namespace together with the types contained in it.
/// </summary>
/// <param name="Name">Name of the namespace.</param>
/// <param name="ObjectTypes">Enumerable of classes, structures and interfaces contained in the namespace.</param>
/// <param name="Delegates">Delegates contained in the namespace.</param>
/// <param name="Enums">Enums contained in the namespace.</param>
public record NamespaceData(string Name, IEnumerable<IObjectTypeData> ObjectTypes, IEnumerable<IDelegateTypeData> Delegates, IEnumerable<IEnumTypeData> Enums);
