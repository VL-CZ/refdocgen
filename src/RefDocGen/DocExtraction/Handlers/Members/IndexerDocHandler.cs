using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Handlers.Tools;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding indexers.
/// </summary>
internal class IndexerDocHandler : IMemberDocHandler
{
    /// <inheritdoc/>
    public void AddDocumentation(ObjectTypeData type, string memberId, XElement memberDocComment)
    {
        if (type.Indexers.TryGetValue(memberId, out var indexer))
        {
            if (memberDocComment.TryGetSummaryElement(out var summaryNode))
            {
                indexer.DocComment = summaryNode;
            }

            var paramElements = memberDocComment.Descendants(XmlDocIdentifiers.Param);

            // add parameter doc comments
            ParameterDocHelper.Add(paramElements, indexer.Parameters);
        }
    }
}
