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
            var enumValue = e.Values.FirstOrDefault(x => x.Name == memberId);

            if (enumValue == null)
            {
                return;
            }

            enumValue.DocComment = summaryNode;
        }
    }
}
