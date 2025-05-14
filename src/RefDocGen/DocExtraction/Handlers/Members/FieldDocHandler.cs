using RefDocGen.CodeElements.Members.Concrete;
using RefDocGen.CodeElements.Types.Concrete;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding fields.
/// </summary>
internal class FieldDocHandler : MemberDocHandler<ObjectTypeData, FieldData>
{
    /// <inheritdoc/>
    protected override IReadOnlyDictionary<string, FieldData> GetMembers(ObjectTypeData type)
    {
        return type.Fields;
    }
}
