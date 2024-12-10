using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;
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

            // assign param doc comments
            foreach (var paramElement in paramElements)
            {
                AssignParamComment(indexer, paramElement);
            }
        }
    }

    /// <summary>
    /// Assign parameter doc comment to the corresponding parameter.
    /// </summary>
    /// <param name="indexer">Executable member (e.g. a method) containing the parameter.</param>
    /// <param name="paramDocComment">Doc comment for the parameter. (i.e. 'param' element)</param>
    private void AssignParamComment(IndexerData indexer, XElement paramDocComment)
    {
        if (paramDocComment.TryGetNameAttribute(out var nameAttr))
        {
            string paramName = nameAttr.Value;
            var parameter = indexer.Parameters.FirstOrDefault(p => p.Name == paramName);

            if (parameter is null)
            {
                // TODO: log parameter not found
                return;
            }

            parameter.DocComment = paramDocComment;
        }
    }
}
