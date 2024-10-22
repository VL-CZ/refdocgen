using RefDocGen.DocExtraction.Tools;
using RefDocGen.MemberData;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Parsers;

internal class PropertyCommentParser : MemberCommentParser
{
    /// <inheritdoc/>
    internal override void AddCommentTo(ClassData type, string memberName, XElement docCommentNode)
    {
        if (docCommentNode.TryGetSummaryElement(out var summaryNode))
        {
            int index = GetTypeMemberIndex(type.Properties, memberName);
            type.Properties[index] = type.Properties[index] with { DocComment = summaryNode };
        }
    }
}
