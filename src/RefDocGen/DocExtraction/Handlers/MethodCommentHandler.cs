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
#pragma warning disable CA1854
        if (type.Methods.ContainsKey(memberId))
        {
            // add summary doc comment (if present)
            if (memberDocComment.TryGetSummaryElement(out var summaryNode))
            {
                type.Methods[memberId] = type.Methods[memberId] with { DocComment = summaryNode };
            }

            // add return value doc comment (if present)
            if (memberDocComment.TryGetReturnsElement(out var returnsNode))
            {
                type.Methods[memberId] = type.Methods[memberId] with { ReturnValueDocComment = returnsNode };
            }
        }
#pragma warning restore CA1854
    }
}
