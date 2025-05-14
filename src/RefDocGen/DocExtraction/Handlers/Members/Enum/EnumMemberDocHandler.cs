using RefDocGen.CodeElements.Members.Concrete.Enum;
using RefDocGen.CodeElements.Types.Concrete.Enum;

namespace RefDocGen.DocExtraction.Handlers.Members.Enum;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding enum members.
/// </summary>
internal class EnumMemberDocHandler : MemberDocHandler<EnumTypeData, EnumMemberData>
{
    /// <inheritdoc/>
    protected override IReadOnlyDictionary<string, EnumMemberData> GetMembers(EnumTypeData type)
    {
        return type.Members;
    }
}
