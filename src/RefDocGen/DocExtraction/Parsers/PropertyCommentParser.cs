using RefDocGen.MemberData;
using RefDocGen.MemberData.Abstract;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Parsers;

internal class PropertyCommentParser : MemberCommentParser
{
    internal override void AddCommentTo(ClassData type, string memberName, XElement docCommentNode)
    {
        var summaryNode = docCommentNode.Element("summary") ?? DocCommentTools.EmptySummaryNode;

        int index = GetTypeMemberIndex(type.Properties, memberName);
        type.Properties[index] = type.Properties[index] with { DocComment = summaryNode };
    }
}
