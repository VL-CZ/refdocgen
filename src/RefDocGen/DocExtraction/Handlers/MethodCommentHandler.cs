using RefDocGen.DocExtraction.Tools;
using RefDocGen.MemberData;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding methods.
/// </summary>
internal class MethodCommentHandler : MemberCommentHandler
{
    /// <inheritdoc/>
    internal override void AddCommentTo(ClassData type, string memberName, XElement memberDocComment)
    {
        var method = type.Methods
            .SingleOrDefault(method => DocCommentExtractor.GetMethodSignatureForXmlDoc(method) == memberName);

        if (method is null)
        {
            return; // TODO: log comment not found   
        }

        int index = Array.IndexOf(type.Methods, method);

        // add summary doc comment (if present)
        if (memberDocComment.TryGetSummaryElement(out var summaryNode))
        {
            type.Methods[index] = type.Methods[index] with { DocComment = summaryNode };
        }

        // add return value doc comment (if present)
        if (memberDocComment.TryGetReturnsElement(out var returnsNode))
        {
            type.Methods[index] = type.Methods[index] with { ReturnValueDocComment = returnsNode };
        }

        var paramElements = memberDocComment.Descendants(XmlDocIdentifiers.Param);

        // add param doc comments
        foreach (var paramElement in paramElements)
        {
            AddParamComment(method, paramElement);
        }
    }

    /// <summary>
    /// Add parameter comment to the corresponding method parameter.
    /// </summary>
    /// <param name="method">Method containing the parameter.</param>
    /// <param name="paramDocComment">Doc comment for the parameter. (i.e. 'param' element)</param>
    private void AddParamComment(MethodData method, XElement paramDocComment)
    {
        if (paramDocComment.TryGetNameAttribute(out var nameAttr))
        {
            string paramName = nameAttr.Value;
            var member = method.Parameters.Single(p => p.Name == paramName);
            int paramIndex = Array.IndexOf(method.Parameters, member);

            method.Parameters[paramIndex] = member with { DocComment = paramDocComment };
        }
    }
}
