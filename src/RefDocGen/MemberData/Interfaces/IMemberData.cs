using System.Xml.Linq;

namespace RefDocGen.MemberData.Interfaces;

/// <summary>
/// Represents data of a type member (such as field, property or method).
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

    /// <summary>
    /// Checks whether the member is static.
    /// </summary>
    bool IsStatic { get; }

    /// <summary>
    /// Doc comment for the member.
    /// </summary>
    XElement DocComment { get; }
}
