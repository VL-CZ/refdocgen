using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Handlers.Abstract;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Concrete;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding properties.
/// </summary>
internal class PropertyCommentHandler : IMemberCommentHandler
{
    /// <inheritdoc/>
    public void AddDocumentation(ObjectTypeData type, string memberIdentifier, XElement docCommentNode)
    {
        if (docCommentNode.TryGetSummaryElement(out var summaryNode))
        {
            if (type.Properties.TryGetValue(memberIdentifier, out var property))
            {
                property.DocComment = summaryNode;
            }
        }
    }
}