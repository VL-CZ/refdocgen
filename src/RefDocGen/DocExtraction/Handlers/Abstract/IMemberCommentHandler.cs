using RefDocGen.CodeElements.Concrete.Types;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Abstract;

/// <summary>
/// Interface used for handling and adding XML doc comments to the corresponding type members.
/// </summary>
internal interface IMemberCommentHandler
{
    /// <summary>
    /// Adds documentation to the given member.
    /// </summary>
    /// <param name="type">Type containing the member.</param>
    /// <param name="memberId">
    /// Identifier of the member extracted from the XML doc comment.
    /// Consists of the member name and parameters string (if the member has them - e.g. a method).
    /// </param>
    /// <param name="memberDocComment">Doc comment for the member.</param>
    void AddDocumentation(TypeData type, string memberId, XElement memberDocComment);
}
