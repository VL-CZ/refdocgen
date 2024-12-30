using RefDocGen.CodeElements.Abstract.Members.Enum;
using RefDocGen.CodeElements.Abstract.Types.Enum;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Members.Enum;
using RefDocGen.Tools;
using System.Collections.Immutable;

namespace RefDocGen.CodeElements.Concrete.Types.Enum;

/// <summary>
/// Class representing data of an enum, including its members.
/// </summary>
internal class EnumTypeData : TypeDeclaration, IEnumTypeData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumTypeData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    /// <param name="members">Dictionary containing the enum members; keys are the corresponding member IDs.</param>
    public EnumTypeData(Type type, IReadOnlyDictionary<string, EnumMemberData> members) : base(type)
    {
        Members = members;
        AllMembers = members.ToParent<string, EnumMemberData, MemberData>();
    }

    /// <summary>
    /// Dictionary containing the enum members; keys are the corresponding member IDs.
    /// </summary>
    public IReadOnlyDictionary<string, EnumMemberData> Members { get; }

    /// <inheritdoc/>
    internal override IReadOnlyDictionary<string, MemberData> AllMembers { get; }

    /// <inheritdoc/>
    IEnumerable<IEnumMemberData> IEnumTypeData.Members => Members.Values;
}
