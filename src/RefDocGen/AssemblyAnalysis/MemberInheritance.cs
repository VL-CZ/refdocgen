namespace RefDocGen.AssemblyAnalysis;

/// <summary>
/// Defines the inheritance rules for members.
/// </summary>
internal enum MemberInheritance
{
    /// <summary>
    /// Do not inherit any members.
    /// </summary>
    None,

    /// <summary>
    /// Inherit all members from base types.
    /// </summary>
    All,

    /// <summary>
    /// Inherit all members except those declared in <see cref="object"/> or <see cref="ValueType"/>.
    /// </summary>
    NonObject
}
