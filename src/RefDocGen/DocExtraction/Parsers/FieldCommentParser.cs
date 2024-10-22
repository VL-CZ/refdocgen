using RefDocGen.DocExtraction.Tools;
using RefDocGen.MemberData;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Parsers;

internal class FieldCommentParser : MemberCommentParser
{
    /// <inheritdoc/>
    internal override void AddCommentTo(ClassData type, string memberName, XElement docCommentNode)
    {
        if (docCommentNode.TryGetSummaryElement(out var summaryNode))
        {
            int index = GetTypeMemberIndex(type.Fields, memberName);
            type.Fields[index] = type.Fields[index] with { DocComment = summaryNode };
        }
    }
}
