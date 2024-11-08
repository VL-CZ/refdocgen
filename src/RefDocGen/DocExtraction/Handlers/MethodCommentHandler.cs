using RefDocGen.DocExtraction.Handlers.Abstract;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.MemberData.Concrete;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding methods.
/// </summary>
internal class MethodCommentHandler : ExecutableMemberCommentHandler<MethodData>
{
    /// <inheritdoc/>
    protected override MethodData? GetTypeMember(ClassData type, string memberId)
    {
        return type.Methods.GetValueOrDefault(memberId);
    }

    /// <inheritdoc/>
    protected override void AssignMemberComments(MethodData member, XElement memberDocComment)
    {
        base.AssignMemberComments(member, memberDocComment);

        // add return value doc comment (if present)
        if (memberDocComment.TryGetReturnsElement(out var returnsNode))
        {
            member.ReturnValueDocComment = returnsNode;
        }
    }
}
