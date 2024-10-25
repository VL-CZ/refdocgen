using RefDocGen.DocExtraction.Handlers.Abstract;
using RefDocGen.DocExtraction.Tools.Extensions;
using RefDocGen.MemberData;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding properties.
/// </summary>
internal class PropertyCommentHandler : IMemberCommentHandler
{
    /// <inheritdoc/>
    public void AddDocumentation(ClassData type, string memberIdentifier, XElement docCommentNode)
    {
        if (docCommentNode.TryGetSummaryElement(out var summaryNode))
        {
            if (type.Properties.TryGetValue(memberIdentifier, out var property))
            {
                type.Properties[memberIdentifier] = property with { DocComment = summaryNode };
            }
        }
    }
}
