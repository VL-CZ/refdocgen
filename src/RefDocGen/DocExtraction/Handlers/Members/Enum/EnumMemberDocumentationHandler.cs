using RefDocGen.CodeElements.Concrete.Types.Enum;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Members.Enum;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding enum members.
/// </summary>
internal class EnumMemberDocumentationHandler
{
    /// <summary>
    /// Add documentation to the given enum member.
    /// </summary>
    /// <param name="e">Enum containing the member.</param>
    /// <param name="memberId">ID of the enum member.</param>
    /// <param name="memberDocComment">Doc comment for the member.</param>
    public void AddDocumentation(EnumTypeData e, string memberId, XElement memberDocComment)
    {
        if (memberDocComment.TryGetSummaryElement(out var summaryNode))
        {
            if (e.Members.TryGetValue(memberId, out var enumValue))
            {
                enumValue.DocComment = summaryNode;
            }
        }
    }
}
