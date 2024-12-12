using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Handlers.Tools;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
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

            if (memberDocComment.TryGetRemarksElement(out var remarksNode))
            {
                property.RemarksDocComment = remarksNode;
            }


            // add exception doc comments
            var exceptionsDocComments = memberDocComment.Descendants(XmlDocIdentifiers.Exception);
            property.Exceptions = ExceptionDocHelper.Parse(exceptionsDocComments);
        }
    }
}
