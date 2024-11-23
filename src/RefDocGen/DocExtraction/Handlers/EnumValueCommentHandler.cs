using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers;

internal class EnumValueCommentHandler
{
    public void AddDocumentation(EnumData e, string memberId, XElement memberDocComment)
    {
        if (memberDocComment.TryGetSummaryElement(out var summaryNode))
        {
            if (e.Values.TryGetValue(memberId, out var enumValue))
            {
                enumValue.DocComment = summaryNode;
            }
        }
    }
}
