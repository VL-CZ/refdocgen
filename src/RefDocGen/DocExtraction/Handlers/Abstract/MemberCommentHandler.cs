using RefDocGen.MemberData;
using RefDocGen.MemberData.Abstract;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Abstract;

/// <summary>
/// Abstract base class responsible for handling and adding XML doc comments to the corresponding type members.
/// </summary>
internal abstract class MemberCommentHandler
{
    /// <summary>
    /// Adds documentation to the given member.
    /// </summary>
    /// <param name="type">Type containing the member.</param>
    /// <param name="memberIdentifier">Identifier of the member. Consists of the member name and parameters string (if the member has them - e.g. a method).</param>
    /// <param name="memberDocComment">Doc comment for the member.</param>
    internal abstract void AddDocumentation(ClassData type, string memberIdentifier, XElement memberDocComment);

    /// <summary>
    /// Get index of the given member in the collection.
    /// </summary>
    /// <param name="memberCollection">Collection containing member data.</param>
    /// <param name="memberIdentifier">Name of the member to find.</param>
    /// <returns>Index of the given member in the collection.</returns>
    protected int GetTypeMemberIndex(IMemberData[] memberCollection, string memberIdentifier)
    {
        return memberCollection
            .Select((member, index) => (member, index))
            .Single(item => item.member.Name == memberIdentifier)
            .index;
    }
}
