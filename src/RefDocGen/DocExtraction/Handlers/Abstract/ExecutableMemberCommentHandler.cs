using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;
using RefDocGen.CodeElements.Concrete.Members;

namespace RefDocGen.DocExtraction.Handlers.Abstract;

/// <summary>
/// Abstract class responsible for handling and adding XML doc comments to the corresponding executable type members.
/// <para>
/// See also <seealso cref="ExecutableMemberData"/> class.
/// </para>
/// </summary>
/// <typeparam name="T">TODO</typeparam>
internal abstract class ExecutableMemberCommentHandler<T> : IMemberCommentHandler where T : ExecutableMemberData
{
    /// <summary>
    /// Get the member with the given <paramref name="memberId"/> contained in the given type.
    /// </summary>
    /// <param name="type">The type containing the member.</param>
    /// <param name="memberId">Id of the member to search.</param>
    /// <returns>The member with the given Id contained in the given type. If such member doesn't exist, null is returned</returns>
    protected abstract T? GetTypeMember(TypeData type, string memberId);

    /// <summary>
    /// Assign doc comments to the given member.
    /// <para>
    /// Note: this method doesn't assign any parameter doc comments.
    /// </para>
    /// </summary>
    /// <param name="member">Member to which the comment is assigned.</param>
    /// <param name="memberDocComment">Doc comment for the member.</param>
    protected virtual void AssignMemberComments(T member, XElement memberDocComment)
    {
        // add summary doc comment (if present)
        if (memberDocComment.TryGetSummaryElement(out var summaryNode))
        {
            member.DocComment = summaryNode;
        }
    }

    /// <inheritdoc/>
    public void AddDocumentation(TypeData type, string memberId, XElement memberDocComment)
    {
        var member = GetTypeMember(type, memberId);

        if (member is null)
        {
            return; // TODO: log comment not found
        }

        // assign member (non-param) doc comments
        AssignMemberComments(member, memberDocComment);

        var paramElements = memberDocComment.Descendants(XmlDocIdentifiers.Param);

        // assign param doc comments
        foreach (var paramElement in paramElements)
        {
            AssignParamComment(member, paramElement);
        }
    }

    /// <summary>
    /// Assign parameter doc comment to the corresponding parameter.
    /// </summary>
    /// <param name="member">Executable member (e.g. a method) containing the parameter.</param>
    /// <param name="paramDocComment">Doc comment for the parameter. (i.e. 'param' element)</param>
    private void AssignParamComment(ExecutableMemberData member, XElement paramDocComment)
    {
        if (paramDocComment.TryGetNameAttribute(out var nameAttr))
        {
            string paramName = nameAttr.Value;
            var parameter = member.Parameters.FirstOrDefault(p => p.Name == paramName);

            if (parameter is null)
            {
                // TODO: log parameter not found
                return;
            }

            parameter.DocComment = paramDocComment;
        }
    }
}

