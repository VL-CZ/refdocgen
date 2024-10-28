using RefDocGen.DocExtraction.Handlers.Abstract;
using RefDocGen.MemberData.Concrete;

namespace RefDocGen.DocExtraction.Handlers;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding constructors.
/// </summary>
internal class ConstructorCommentHandler : ExecutableMemberCommentHandler<ConstructorData>
{
    /// <inheritdoc/>
    protected override ConstructorData? GetTypeMember(ClassData type, string memberId)
    {
        return type.Constructors.GetValueOrDefault(memberId);
    }
}
