using RefDocGen.DocExtraction.Tools.Extensions;
using RefDocGen.MemberData.Abstract;
using RefDocGen.MemberData.Concrete;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Abstract;

internal abstract class ValueMemberCommentHandler<T> : IMemberCommentHandler where T : IMemberData
{
    protected abstract T? GetMember(ClassData type, string memberId);

    public void AddDocumentation(ClassData type, string memberId, XElement memberDocComment)
    {
        var member = GetMember(type, memberId);

        if (member is null)
        {
            return;
        }

        if (memberDocComment.TryGetSummaryElement(out var summaryNode))
        {
            //member.DocComment = summaryNode;
        }
    }
}
