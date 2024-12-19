using RefDocGen.CodeElements.Concrete.Members.Enum;
using RefDocGen.CodeElements.Concrete.Types.Enum;

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
