using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Abstract.Members.Enum;

/// <summary>
/// Represents data of an enum member.
/// </summary>
public interface IEnumMemberData : IMemberData
{
    /// <summary>
    /// <see cref="System.Reflection.FieldInfo"/> object representing the enum member.
    /// </summary>
    FieldInfo FieldInfo { get; }
}
