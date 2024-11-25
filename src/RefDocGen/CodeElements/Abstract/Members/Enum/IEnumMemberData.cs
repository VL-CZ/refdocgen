using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Abstract.Members.Enum;

/// <summary>
/// Represents data of an enum member.
/// </summary>
public interface IEnumMemberData
{
    /// <summary>
    /// <see cref="System.Reflection.FieldInfo"/> object representing the enum member.
    /// </summary>
    FieldInfo FieldInfo { get; }

    /// <summary>
    /// Id of the enum member.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Name of the enum member.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Doc comment for the enum member.
    /// </summary>
    XElement DocComment { get; }
}
