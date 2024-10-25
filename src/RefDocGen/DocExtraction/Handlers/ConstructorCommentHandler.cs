using RefDocGen.DocExtraction.Handlers.Abstract;
using RefDocGen.DocExtraction.Tools.Extensions;
using RefDocGen.MemberData;
using RefDocGen.MemberData.Abstract;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding constructors.
/// </summary>
internal class ConstructorCommentHandler : InvokableMemberCommentHandler
{
    /// <inheritdoc/>
    protected override void AssignMemberComments(ClassData type, string memberId, XElement memberDocComment)
    {
        // add summary doc comment (if present)
        if (memberDocComment.TryGetSummaryElement(out var summaryNode))
        {
            if (type.Constructors.TryGetValue(memberId, out var ctor))
            {
                type.Constructors[memberId] = ctor with { DocComment = summaryNode };
            }
        }
    }

    /// <inheritdoc/>
    protected override InvokableMemberData? GetTypeMember(ClassData type, string memberId)
    {
        return type.Constructors.GetValueOrDefault(memberId);
    }
}
