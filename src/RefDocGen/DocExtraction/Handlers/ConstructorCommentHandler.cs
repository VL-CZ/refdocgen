using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Handlers.Abstract;
using RefDocGen.CodeElements.Concrete.Members;

namespace RefDocGen.DocExtraction.Handlers;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding constructors.
/// </summary>
internal class ConstructorCommentHandler : ExecutableMemberCommentHandler<ConstructorData>
{
    /// <inheritdoc/>
    protected override ConstructorData? GetTypeMember(TypeData type, string memberId)
    {
        return type.Constructors.GetValueOrDefault(memberId);
    }
}
