using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Abstract class responsible for handling and adding XML doc comments to the corresponding type members.
/// </summary>
/// <typeparam name="T">Type of the member to which the documentation is added.</typeparam>
internal abstract class MemberDocHandler<T> : IMemberDocHandler where T : MemberData
{
    /// <summary>
    /// Get a dictionary of the given members (indexed by their IDs) contained in the specified type.
    /// </summary>
    /// <param name="type">The type containing the members.</param>
    /// <returns>A dictionary of the given members (indexed by their IDs) contained in the specified type.</returns>
    protected abstract IReadOnlyDictionary<string, T> GetMembers(ObjectTypeData type);

    /// <inheritdoc/>
    public void AddDocumentation(ObjectTypeData type, string memberId, XElement memberDocComment)
    {
        var member = GetMembers(type).GetValueOrDefault(memberId);

        if (member is null)
        {
            return; // TODO: log member not found
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

        // add raw doc comment
        member.RawDocComment = memberDocComment;

        // add other doc comments
        AddRemainingComments(member, memberDocComment);
    }

    /// <summary>
    /// Add doc comments other than 'summary' and 'remarks' to the given member.
    /// </summary>
    /// <remarks>
    /// Note that 'summary' and 'remarks' doc comments are already added automatically in the <see cref="AddDocumentation(ObjectTypeData, string, XElement)"/> method.
    /// </remarks>
    /// <param name="member">Member to which the documentation are added.</param>
    /// <param name="memberDocComment">Doc comment for the given member.</param>
    protected virtual void AddRemainingComments(T member, XElement memberDocComment) { }
}
