using RefDocGen.DocExtraction.Tools;
using RefDocGen.MemberData;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers;

internal class ConstructorCommentHandler : MemberCommentHandler
{
    /// <inheritdoc/>
    internal override void AddCommentTo(ClassData type, string memberName, XElement memberDocComment)
    {
        var method = type.Constructors
            .SingleOrDefault(method => DocCommentExtractor.GetMethodSignatureForXmlDoc(method) == memberName);

        if (method is null)
        {
            return; // TODO: log comment not found   
        }

        int index = Array.IndexOf(type.Constructors, method);

        // add summary doc comment (if present)
        if (memberDocComment.TryGetSummaryElement(out var summaryNode))
        {
            type.Constructors[index] = type.Constructors[index] with { DocComment = summaryNode };
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
    private void AddParamComment(ConstructorData method, XElement paramDocComment)
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
