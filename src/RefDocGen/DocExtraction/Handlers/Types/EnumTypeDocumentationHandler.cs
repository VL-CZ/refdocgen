using RefDocGen.CodeElements.Concrete.Types.Enum;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Types;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding enum types.
/// </summary>
internal class EnumTypeDocumentationHandler
{
    /// <summary>
    /// Adds documentation to the given enum type.
    /// </summary>
    /// <param name="e">Enum type to which the documentation is added.</param>
    /// <param name="docComment">Doc comment for the enum.</param>
    internal void AddDocumentation(EnumTypeData e, XElement docComment)
    {
        if (docComment.TryGetSummaryElement(out var summaryNode))
        {
            e.DocComment = summaryNode;
        }
    }
}
