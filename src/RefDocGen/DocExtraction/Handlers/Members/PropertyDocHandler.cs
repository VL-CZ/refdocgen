using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding properties.
/// </summary>
internal class PropertyDocHandler : IMemberDocHandler
{
    /// <inheritdoc/>
    public void AddDocumentation(ObjectTypeData type, string memberId, XElement memberDocComment)
    {
        if (type.Properties.TryGetValue(memberId, out var property))
        {
            if (memberDocComment.TryGetSummaryElement(out var summaryNode))
            {
                property.SummaryDocComment = summaryNode;
            }

            if (memberDocComment.TryGetValueElement(out var valueNode))
            {
                property.ValueDocComment = valueNode;
            }
        }
    }
}
