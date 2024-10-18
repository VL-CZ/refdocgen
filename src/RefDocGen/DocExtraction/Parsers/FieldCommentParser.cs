using RefDocGen.MemberData;
using RefDocGen.MemberData.Abstract;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Parsers;

internal class FieldCommentParser : MemberCommentParser
{

    internal override void AddCommentTo(ClassData type, string memberName, XElement docCommentNode)
    {
        var summaryNode = docCommentNode.Element("summary") ?? DocCommentTools.EmptySummaryNode;

        int index = GetTypeMemberIndex(type.Fields, memberName);
        type.Fields[index] = type.Fields[index] with { DocComment = summaryNode };
    }
}
