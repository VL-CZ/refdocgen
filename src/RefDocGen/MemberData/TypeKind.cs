namespace RefDocGen.MemberData;

/// <summary>
/// Represents kind of a type.
/// </summary>
public enum TypeKind
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
