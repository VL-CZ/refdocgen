using RefDocGen.CodeElements.Abstract.Members.Enum;
using RefDocGen.CodeElements.Abstract.Types.Enum;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Members.Enum;
using RefDocGen.Tools;

namespace RefDocGen.CodeElements.Concrete.Types.Enum;

/// <summary>
/// Class representing data of an enum, including its members.
/// </summary>
internal class EnumTypeData : TypeDeclaration, IEnumTypeData
{
    /// <summary>
    /// Indicates whether the members have already been added.
    /// </summary>
    private bool membersAdded;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumTypeData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    public EnumTypeData(Type type) : base(type)
    {
    }

    /// <summary>
    /// Dictionary containing the enum members; keys are the corresponding member IDs.
    /// </summary>
    public IReadOnlyDictionary<string, EnumMemberData> Members { get; private set; } = new Dictionary<string, EnumMemberData>();

    /// <inheritdoc/>
    internal override IReadOnlyDictionary<string, MemberData> AllMembers { get; private protected set; } = new Dictionary<string, MemberData>();

    /// <inheritdoc/>
    IEnumerable<IEnumMemberData> IEnumTypeData.Members => Members.Values;

    /// <summary>
    /// Adds the enum members to the enum.
    /// </summary>
    /// <param name="members">Dictionary containing the enum members; keys are the corresponding member IDs.</param>
    internal void AddMembers(IReadOnlyDictionary<string, EnumMemberData> members)
    {
        if (membersAdded)
        {
            throw new InvalidOperationException($"The members have been already added to {Id} enum.");
        }

        Members = members;
        AllMembers = members.ToParent<string, EnumMemberData, MemberData>();

        membersAdded = true;
    }
}
