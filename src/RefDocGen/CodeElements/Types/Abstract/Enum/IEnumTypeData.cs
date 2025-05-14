using RefDocGen.CodeElements.Members.Abstract.Enum;
using RefDocGen.CodeElements.Types.Abstract;

namespace RefDocGen.CodeElements.Types.Abstract.Enum;

/// <summary>
/// Represents data of an enum type.
/// </summary>
public interface IEnumTypeData : ITypeDeclaration
{
    /// <summary>
    /// Collection of declared enum members.
    /// </summary>
    IEnumerable<IEnumMemberData> Members { get; }
}
