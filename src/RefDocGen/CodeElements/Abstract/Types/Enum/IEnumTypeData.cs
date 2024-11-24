using RefDocGen.CodeElements.Abstract.Members.Enum;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Abstract.Types.Enum;

/// <summary>
/// Represents data of an enum type.
/// </summary>
public interface IEnumTypeData : ITypeNameBaseData
{
    /// <summary>
    /// Access modifier of the type.
    /// </summary>
    AccessModifier AccessModifier { get; }

    /// <summary>
    /// Documentation comment provided to the type.
    /// </summary>
    XElement DocComment { get; }

    /// <summary>
    /// Collection of declared enum members.
    /// </summary>
    IReadOnlyList<IEnumMemberData> Members { get; }
}
