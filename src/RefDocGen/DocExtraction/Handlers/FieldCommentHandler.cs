using RefDocGen.DocExtraction.Handlers.Abstract;
using RefDocGen.DocExtraction.Tools.Extensions;
using RefDocGen.MemberData;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding fields.
/// </summary>
internal class FieldCommentHandler : MemberCommentHandler
{
    /// <inheritdoc/>
    internal override void AddDocumentation(ClassData type, string memberName, XElement docCommentNode)
    {
        if (docCommentNode.TryGetSummaryElement(out var summaryNode))
        {
            int index = GetTypeMemberIndex(type.Fields, memberName);
            type.Fields[index] = type.Fields[index] with { DocComment = summaryNode };
        }
    }
}
