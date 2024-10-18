using RefDocGen.MemberData;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Parsers;

internal class MethodCommentParser : MemberCommentParser
{
    internal override void AddCommentTo(ClassData type, string memberName, XElement memberDocComment)
    {
        var summaryNode = memberDocComment.Element("summary") ?? DocCommentTools.EmptySummaryNode;
        var returnsNode = memberDocComment.Element("returns") ?? DocCommentTools.EmptyReturnsNode;

        int index = GetTypeMemberIndex(type.Methods, memberName);
        var method = type.Methods[index];
        type.Methods[index] = method with { DocComment = summaryNode, ReturnValueDocComment = returnsNode };

        var paramElements = memberDocComment.Descendants("param");
        foreach (var paramElement in paramElements)
        {
            var nameAttr = paramElement.Attribute("name");
            if (nameAttr is not null)
            {
                string paramName = nameAttr.Value;
                var member = method.Parameters.Single(p => p.Name == paramName);
                int paramIndex = Array.IndexOf(method.Parameters, member);

                method.Parameters[paramIndex] = member with { DocComment = paramElement };
            }
        }
    }
}
