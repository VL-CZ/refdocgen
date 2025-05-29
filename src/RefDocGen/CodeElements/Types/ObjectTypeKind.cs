namespace RefDocGen.CodeElements.Types;

/// <summary>
/// Represents kind of an object type.
/// </summary>
public enum ObjectTypeKind
{
    /// <summary>
    /// Represents value type.
    /// </summary>
    ValueType,

    /// <summary>
    /// Represents class (a reference type that that's not an interface).
    /// </summary>
    Class,

    /// <summary>
    /// Represents an interface.
    /// </summary>
    Interface
}
