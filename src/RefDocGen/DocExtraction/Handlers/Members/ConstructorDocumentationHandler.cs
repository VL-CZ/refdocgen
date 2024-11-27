using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Concrete.Members;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding constructors.
/// </summary>
internal class ConstructorDocumentationHandler : ExecutableMemberDocumentationHandler<ConstructorData>
{
    /// <inheritdoc/>
    protected override ConstructorData? GetTypeMember(ObjectTypeData type, string memberId)
    {
        return type.Constructors.GetValueOrDefault(memberId);
    }
}
