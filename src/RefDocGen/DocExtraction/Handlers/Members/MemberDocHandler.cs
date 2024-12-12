using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Members;

internal abstract class MemberDocHandler<T> : IMemberDocHandler where T : MemberData
{
    /// <summary>
    /// Get the member with the given <paramref name="memberId"/> contained in the given type.
    /// </summary>
    /// <param name="type">The type containing the member.</param>
    /// <param name="memberId">Id of the member to get.</param>
    /// <returns>The member with the given Id contained in the given type. If such member doesn't exist, <see langword="null"/> is returned.</returns>
    protected abstract IReadOnlyDictionary<string, T> GetMembers(ObjectTypeData type);

    public void AddDocumentation(ObjectTypeData type, string memberId, XElement memberDocComment)
    {
        var member = GetMembers(type).GetValueOrDefault(memberId);

        if (member is null)
        {
            return; // TODO: log comment not found
        }

        // add 'summary' doc comment
        if (memberDocComment.TryGetSummaryElement(out var summaryNode))
        {
            member.SummaryDocComment = summaryNode;
        }

        // add 'remarks' doc comment
        if (memberDocComment.TryGetRemarksElement(out var remarksNode))
        {
            member.RemarksDocComment = remarksNode;
        }
    }

    protected virtual void AddRemainingComments(T member, XElement memberDocComment) { }
}
