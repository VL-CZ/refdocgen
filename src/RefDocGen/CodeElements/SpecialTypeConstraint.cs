namespace RefDocGen.CodeElements;

/// <summary>
/// Represents special constraints that can be applied to generic type parameters.
/// </summary>
public enum SpecialTypeConstraint
{
    /// <summary>
    /// Indicates that the generic type parameter must be a reference type.
    /// </summary>
    ReferenceType,

    /// <summary>
    /// Indicates that the generic type parameter must be a non-nullable value type.
    /// </summary>
    NotNullableValueType,

    /// <summary>
    /// Indicates that the generic type parameter must have a parameterless constructor.
    /// </summary>
    DefaultConstructor
}

