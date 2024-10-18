using RefDocGen.MemberData;
using RefDocGen.MemberData.Abstract;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Parsers;

internal abstract class MemberCommentParser
{
    /// <summary>
    /// Add doc comment to the given member.
    /// </summary>
    /// <param name="type">Type containing the member.</param>
    /// <param name="fieldName">Name of the member.</param>
    /// <param name="commentNode">Doc comment for the member.</param>
    internal abstract void AddCommentTo(ClassData type, string memberName, XElement memberDocComment);

    /// <summary>
    /// Get index of the given member in the collection.
    /// </summary>
    /// <param name="memberCollection">Collection containing member data.</param>
    /// <param name="memberName">Name of the member to find.</param>
    /// <returns>Index of the given member in the collection.</returns>
    protected int GetTypeMemberIndex(IMemberData[] memberCollection, string memberName)
    {
        return memberCollection
            .Select((member, index) => (member, index))
            .Single(item => item.member.Name == memberName)
            .index;
    }
}
