using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding constructors.
/// </summary>
internal class ConstructorDocHandler : ExecutableMemberDocHandler<ConstructorData>
{
    /// <inheritdoc/>
    protected override ConstructorData? GetTypeMember(ObjectTypeData type, string memberId)
    {
        return type.Constructors.GetValueOrDefault(memberId);
    }
}
