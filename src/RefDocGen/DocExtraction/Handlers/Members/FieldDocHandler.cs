using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding fields.
/// </summary>
internal class FieldDocHandler : MemberDocHandler<FieldData>
{
    /// <inheritdoc/>
    protected override IReadOnlyDictionary<string, FieldData> GetMembers(ObjectTypeData type)
    {
        return type.Fields;
    }
}
