using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding operators.
/// </summary>
internal class OperatorDocumentationHandler : ExecutableMemberDocumentationHandler<OperatorData>
{
    /// <inheritdoc/>
    protected override OperatorData? GetTypeMember(ObjectTypeData type, string memberId)
    {
        return type.Operators.GetValueOrDefault(memberId);
    }

    /// <inheritdoc/>
    protected override void AssignMemberComments(OperatorData member, XElement memberDocComment)
    {
        base.AssignMemberComments(member, memberDocComment);

        // add return value doc comment (if present)
        if (memberDocComment.TryGetReturnsElement(out var returnsNode))
        {
            member.ReturnValueDocComment = returnsNode;
        }
    }
}
