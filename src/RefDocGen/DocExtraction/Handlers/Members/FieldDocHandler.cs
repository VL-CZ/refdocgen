using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding fields.
/// </summary>
internal class FieldDocHandler : IMemberDocHandler
{
    /// <inheritdoc/>
    public void AddDocumentation(ObjectTypeData type, string memberId, XElement memberDocComment)
    {
        if (type.Fields.TryGetValue(memberId, out var field))
        {
            if (memberDocComment.TryGetSummaryElement(out var summaryNode))
            {
                field.SummaryDocComment = summaryNode;
            }

            if (memberDocComment.TryGetRemarksElement(out var remarksNode))
            {
                field.RemarksDocComment = remarksNode;
            }
        }
    }
}
