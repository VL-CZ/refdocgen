using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Handlers.Abstract;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding fields.
/// </summary>
internal class FieldCommentHandler : IMemberCommentHandler
{
    /// <inheritdoc/>
    public void AddDocumentation(TypeData type, string memberIdentifier, XElement docCommentNode)
    {
        if (docCommentNode.TryGetSummaryElement(out var summaryNode))
        {
            if (type.Fields.TryGetValue(memberIdentifier, out var field))
            {
                field.DocComment = summaryNode;
            }
        }
    }
}
