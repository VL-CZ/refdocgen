using RefDocGen.DocExtraction.Handlers.Abstract;
using RefDocGen.DocExtraction.Tools.Extensions;
using RefDocGen.MemberData.Concrete;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding fields.
/// </summary>
internal class FieldCommentHandler : IMemberCommentHandler
{
    /// <inheritdoc/>
    public void AddDocumentation(ClassData type, string memberIdentifier, XElement docCommentNode)
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
