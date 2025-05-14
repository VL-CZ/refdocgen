using RefDocGen.CodeElements.Members.Abstract;
using System.Reflection;

namespace RefDocGen.CodeElements.Members.Abstract.Enum;

/// <summary>
/// Represents data of an enum member.
/// </summary>
public interface IEnumMemberData : IMemberData
{
    /// <summary>
    /// <see cref="System.Reflection.FieldInfo"/> object representing the enum member.
    /// </summary>
    FieldInfo FieldInfo { get; }

    /// <summary>
    /// Integral value of the enum member.
    /// </summary>
    object Value { get; }
}
