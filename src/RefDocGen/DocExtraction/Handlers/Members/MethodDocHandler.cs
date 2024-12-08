using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Concrete.Members;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding methods.
/// </summary>
internal class MethodDocHandler : ExecutableMemberDocHandler<MethodData>
{
    /// <inheritdoc/>
    protected override MethodData? GetTypeMember(ObjectTypeData type, string memberId)
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
