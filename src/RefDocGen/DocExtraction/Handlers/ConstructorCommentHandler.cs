using RefDocGen.DocExtraction.Handlers.Abstract;
using RefDocGen.MemberData;
using RefDocGen.MemberData.Abstract;

namespace RefDocGen.DocExtraction.Handlers;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding constructors.
/// </summary>
internal class ConstructorCommentHandler : InvokableMemberCommentHandler
{
    /// <inheritdoc/>
    protected override InvokableMemberData[] GetMemberCollection(ClassData type)
    {
        return type.Constructors;
    }
}
