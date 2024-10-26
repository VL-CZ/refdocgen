using RefDocGen.DocExtraction.Tools.Extensions;
using RefDocGen.MemberData.Implementation;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Abstract;

/// <summary>
/// Abstract class responsible for handling and adding XML doc comments to the corresponding invokable type members.
/// <para>
/// See also <seealso cref="InvokableMemberData"/> class.
/// </para>
/// </summary>
internal abstract class InvokableMemberCommentHandler : IMemberCommentHandler
{
    /// <summary>
    /// Get the member with the given <paramref name="memberId"/> contained in the given type.
    /// </summary>
    /// <param name="type">The type containing the member.</param>
    /// <param name="memberId">Id of the member to search.</param>
    /// <returns>The member with the given Id contained in the given type. If such member doesn't exist, null is returned</returns>
    protected abstract InvokableMemberData? GetTypeMember(ClassData type, string memberId);

    /// <summary>
    /// Assign doc comments to the given member.
    /// <para>
    /// Note: this method doesn't assign any parameter doc comments.
    /// </para>
    /// </summary>
    /// <param name="type">The type containing the member.</param>
    /// <param name="memberId">Id of the member in corresponding member collection.</param>
    /// <param name="memberDocComment">Doc comment for the member.</param>
    protected abstract void AssignMemberComments(ClassData type, string memberId, XElement memberDocComment);

    /// <inheritdoc/>
    public void AddDocumentation(ClassData type, string memberId, XElement memberDocComment)
    {
        var member = GetTypeMember(type, memberId);

        if (member is null)
        {
            return; // TODO: log comment not found   
        }

        // assign member (non-param) doc comments
        AssignMemberComments(type, memberId, memberDocComment);

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
    /// <param name="member">Invokable member (e.g. a method) containing the parameter.</param>
    /// <param name="paramDocComment">Doc comment for the parameter. (i.e. 'param' element)</param>
    private void AssignParamComment(InvokableMemberData member, XElement paramDocComment)
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

