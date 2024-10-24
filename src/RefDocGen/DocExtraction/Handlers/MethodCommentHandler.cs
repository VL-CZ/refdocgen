using RefDocGen.DocExtraction.Handlers.Abstract;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.MemberData;
using RefDocGen.MemberData.Abstract;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding methods.
/// </summary>
internal class MethodCommentHandler : InvokableMemberCommentHandler
{
    /// <inheritdoc/>
    protected override InvokableMemberData[] GetMemberCollection(ClassData type)
    {
        return type.Methods;
    }

    /// <inheritdoc/>
    protected override void AssignMemberComments(ClassData type, int memberIndex, XElement docComment)
    {
        base.AssignMemberComments(type, memberIndex, docComment);

        // add return value doc comment (if present)
        if (docComment.TryGetReturnsElement(out var returnsNode))
        {
            type.Methods[memberIndex] = type.Methods[memberIndex] with { ReturnValueDocComment = returnsNode };
        }
    }
}
