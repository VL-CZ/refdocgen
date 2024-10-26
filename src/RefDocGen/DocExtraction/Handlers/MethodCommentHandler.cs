using RefDocGen.DocExtraction.Handlers.Abstract;
using RefDocGen.DocExtraction.Tools.Extensions;
using RefDocGen.MemberData.Implementation;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding methods.
/// </summary>
internal class MethodCommentHandler : InvokableMemberCommentHandler
{
    /// <inheritdoc/>
    protected override InvokableMemberData? GetTypeMember(ClassData type, string memberId)
    {
        return type.Methods.GetValueOrDefault(memberId);
    }

    /// <inheritdoc/>
    protected override void AssignMemberComments(ClassData type, string memberId, XElement memberDocComment)
    {
        if (type.Methods.TryGetValue(memberId, out var method))
        {
            // add summary doc comment (if present)
            if (memberDocComment.TryGetSummaryElement(out var summaryNode))
            {
                method.DocComment = summaryNode;
            }

            // add return value doc comment (if present)
            if (memberDocComment.TryGetReturnsElement(out var returnsNode))
            {
                method.ReturnValueDocComment = returnsNode;
            }
        }
    }
}
