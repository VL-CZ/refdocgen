using RefDocGen.DocExtraction.Handlers.Abstract;
using RefDocGen.DocExtraction.Tools.Extensions;
using RefDocGen.MemberData;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding properties.
/// </summary>
internal class PropertyCommentHandler : MemberCommentHandler
{
    /// <inheritdoc/>
    internal override void AddDocumentation(ClassData type, string memberIdentifier, XElement docCommentNode)
    {
        if (docCommentNode.TryGetSummaryElement(out var summaryNode))
        {
            int index = GetTypeMemberIndex(type.Properties, memberIdentifier);
            type.Properties[index] = type.Properties[index] with { DocComment = summaryNode };
        }
    }
}
