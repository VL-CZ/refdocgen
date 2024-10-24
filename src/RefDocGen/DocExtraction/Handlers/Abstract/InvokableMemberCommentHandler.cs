using RefDocGen.DocExtraction.Tools;
using RefDocGen.DocExtraction.Tools.Extensions;
using RefDocGen.MemberData;
using RefDocGen.MemberData.Abstract;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Abstract;

/// <summary>
/// Abstract class responsible for handling and adding XML doc comments to the corresponding invokable type members.
/// <para>
/// See also <seealso cref="InvokableMemberData"/> class.
/// </para>
/// </summary>
internal abstract class InvokableMemberCommentHandler : MemberCommentHandler
{
    /// <summary>
    /// Get the specified invokable members of the given type.
    /// </summary>
    /// <param name="type">The type containing the members.</param>
    /// <returns>An array containing the type's members.</returns>
    protected abstract InvokableMemberData[] GetMemberCollection(ClassData type);

    /// <summary>
    /// Assign doc comments to the given member.
    /// <para>
    /// Note: this method doesn't assign any parameter doc comments.
    /// </para>
    /// </summary>
    /// <param name="type">The type containing the member.</param>
    /// <param name="memberIndex">Index of the member in the type's corresponding member collection.</param>
    /// <param name="memberDocComment">Doc comment for the member.</param>
    protected virtual void AssignMemberComments(ClassData type, int memberIndex, XElement memberDocComment)
    {
        var typeMembers = GetMemberCollection(type);

        // add summary doc comment (if present)
        if (memberDocComment.TryGetSummaryElement(out var summaryNode))
        {
            typeMembers[memberIndex] = typeMembers[memberIndex] with { DocComment = summaryNode };
        }
    }

    /// <inheritdoc/>
    internal override void AddDocumentation(ClassData type, string memberName, XElement memberDocComment)
    {
        var typeMembers = GetMemberCollection(type);

        var invokable = typeMembers
            .SingleOrDefault(method =>
                method.GetXmlDocSignature() == memberName
            );

        if (invokable is null)
        {
            return; // TODO: log comment not found   
        }

        int index = Array.IndexOf(typeMembers, invokable);

        AssignMemberComments(type, index, memberDocComment);

        var paramElements = memberDocComment.Descendants(XmlDocIdentifiers.Param);

        // add param doc comments
        foreach (var paramElement in paramElements)
        {
            AssignParamComment(invokable, paramElement);
        }
    }

    /// <summary>
    /// Assign parameter doc comment to the corresponding parameter.
    /// </summary>
    /// <param name="invokable">Invokable member (e.g. a method) containing the parameter.</param>
    /// <param name="paramDocComment">Doc comment for the parameter. (i.e. 'param' element)</param>
    private void AssignParamComment(InvokableMemberData invokable, XElement paramDocComment)
    {
        if (paramDocComment.TryGetNameAttribute(out var nameAttr))
        {
            string paramName = nameAttr.Value;
            var parameter = invokable.Parameters.Single(p => p.Name == paramName);
            int paramIndex = Array.IndexOf(invokable.Parameters, parameter);

            invokable.Parameters[paramIndex] = parameter with { DocComment = paramDocComment };
        }
    }
}

