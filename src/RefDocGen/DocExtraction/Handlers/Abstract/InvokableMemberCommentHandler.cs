using RefDocGen.DocExtraction.Tools;
using RefDocGen.MemberData;
using RefDocGen.MemberData.Abstract;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Abstract;

internal abstract class InvokableMemberCommentHandler : MemberCommentHandler
{
    protected abstract InvokableMemberData[] GetMemberCollection(ClassData type);

    protected virtual void UpdateComment(ClassData type, int memberIndex, XElement docComment)
    {
        var typeMembers = GetMemberCollection(type);

        // add summary doc comment (if present)
        if (docComment.TryGetSummaryElement(out var summaryNode))
        {
            typeMembers[memberIndex] = typeMembers[memberIndex] with { DocComment = summaryNode };
        }
    }

    /// <inheritdoc/>
    internal override void AddCommentTo(ClassData type, string memberName, XElement memberDocComment)
    {
        var typeMembers = GetMemberCollection(type);

        var invokable = typeMembers
            .SingleOrDefault(method => DocCommentExtractor.GetMethodSignatureForXmlDoc(method) == memberName);

        if (invokable is null)
        {
            return; // TODO: log comment not found   
        }

        int index = Array.IndexOf(typeMembers, invokable);

        UpdateComment(type, index, memberDocComment);

        var paramElements = memberDocComment.Descendants(XmlDocIdentifiers.Param);

        // add param doc comments
        foreach (var paramElement in paramElements)
        {
            AddParamComment(invokable, paramElement);
        }
    }

    /// <summary>
    /// Add parameter comment to the corresponding member parameter.
    /// </summary>
    /// <param name="invokable">Method containing the parameter.</param>
    /// <param name="paramDocComment">Doc comment for the parameter. (i.e. 'param' element)</param>
    private void AddParamComment(InvokableMemberData invokable, XElement paramDocComment)
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

