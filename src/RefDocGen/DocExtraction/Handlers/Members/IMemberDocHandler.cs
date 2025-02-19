using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Interface used for handling and adding XML doc comments to the corresponding type members.
/// </summary>
/// <typeparam name="TType">Type of the type containing the member.</typeparam>
internal interface IMemberDocHandler<TType>
{
    /// <summary>
    /// Adds documentation to the given member.
    /// </summary>
    /// <param name="type">The type containing the member.</param>
    /// <param name="memberId">
    /// Identifier of the member extracted from the XML doc comment.
    /// Consists of the member name and parameters string (if the member has them - e.g. a method).
    /// </param>
    /// <param name="memberDocComment">Doc comment for the member.</param>
    void AddDocumentation(TType type, string memberId, XElement memberDocComment);
}
