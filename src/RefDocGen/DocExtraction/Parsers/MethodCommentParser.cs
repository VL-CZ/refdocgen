using RefDocGen.DocExtraction.Tools;
using RefDocGen.MemberData;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Parsers;

internal class MethodCommentParser : MemberCommentParser
{
    internal override void AddCommentTo(ClassData type, string memberName, XElement memberDocComment)
    {
        int index = GetTypeMemberIndex(type.Methods, memberName);

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

        var method = type.Methods[index];

        var paramElements = memberDocComment.Descendants("param");
        foreach (var paramElement in paramElements)
        {
            if (paramElement.TryGetNameAttribute(out var nameAttr))
            {
                string paramName = nameAttr.Value;
                var member = method.Parameters.Single(p => p.Name == paramName);
                int paramIndex = Array.IndexOf(method.Parameters, member);

                method.Parameters[paramIndex] = member with { DocComment = paramElement };
            }
        }
    }
}
