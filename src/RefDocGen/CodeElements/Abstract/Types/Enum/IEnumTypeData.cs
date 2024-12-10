using RefDocGen.CodeElements.Abstract.Members.Enum;

namespace RefDocGen.CodeElements.Abstract.Types.Enum;

/// <summary>
/// Represents data of an enum type.
/// </summary>
public interface IEnumTypeData : ITypeDeclaration
{
    /// <summary>
    /// Collection of declared enum members.
    /// </summary>
    IReadOnlyList<IEnumMemberData> Members { get; }
}
