using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Abstract class responsible for handling and adding XML doc comments to the corresponding type members.
/// </summary>
/// <typeparam name="TType">Type of the type containing the member</typeparam>
/// <typeparam name="TMember">Type of the member to which the documentation is added.</typeparam>
internal abstract class MemberDocHandler<TType, TMember> : IMemberDocHandler<TType>
    where TMember : MemberData
    where TType : TypeDeclaration
{
    /// <summary>
    /// Get a dictionary of the given members (indexed by their IDs) contained in the specified type.
    /// </summary>
    /// <param name="type">The type containing the members.</param>
    /// <returns>A dictionary of the given members (indexed by their IDs) contained in the specified type.</returns>
    protected abstract IReadOnlyDictionary<string, TMember> GetMembers(TType type);

    /// <inheritdoc/>
    public void AddDocumentation(TType type, string memberId, XElement memberDocComment)
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

        // add 'seealso' doc comments
        member.SeeAlsoDocComments = memberDocComment.Elements(XmlDocIdentifiers.SeeAlso);

        // add raw doc comment
        member.RawDocComment = memberDocComment;

        // add other doc comments
        AddRemainingComments(member, memberDocComment);
    }

    /// <summary>
    /// Add doc comments other than 'summary' and 'remarks' to the given member.
    /// </summary>
    /// <remarks>
    /// Note that 'summary' and 'remarks' doc comments are already added automatically in the <see cref="AddDocumentation(TType, string, XElement)"/> method.
    /// </remarks>
    /// <param name="member">Member to which the documentation are added.</param>
    /// <param name="memberDocComment">Doc comment for the given member.</param>
    protected virtual void AddRemainingComments(TMember member, XElement memberDocComment) { }
}
