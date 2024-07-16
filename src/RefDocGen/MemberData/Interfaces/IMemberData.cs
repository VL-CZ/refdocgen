namespace RefDocGen.MemberData.Interfaces;

/// <summary>
/// Represents data of any type member (such as field, property or method).
/// </summary>
public interface IMemberData
{
    /// <summary>
    /// Name of the member.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Access modifier of the member.
    /// </summary>
    AccessModifier AccessModifier { get; }
}
