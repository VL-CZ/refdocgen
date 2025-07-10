using RefDocGen.CodeElements.Members.Concrete;
using RefDocGen.CodeElements.TypeRegistry;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.InheritDoc;

/// <summary>
/// Class responsible for handling the 'inheritdoc' comments provided to type members and replacing them with the actual documentation.
/// </summary>
/// <remarks>
/// This class isn't intended to handle inheritdoc comments with 'cref' attribute (for further info, see <see cref="CrefInheritDocResolver"/>).
/// </remarks>
internal class MemberInheritDocResolver : InheritDocResolver<MemberData>
{
    /// <inheritdoc/>
    public MemberInheritDocResolver(TypeRegistry typeRegistry) : base(typeRegistry)
    {
    }

    /// <inheritdoc/>
    protected override InheritDocKind InheritDocKind => InheritDocKind.NonCref;

    /// <inheritdoc/>
    protected override IReadOnlyList<MemberData> GetParentNodes(MemberData member)
    {
        if (member.Name == "AddRange" && member.ContainingType.ShortName == "MyStringCollection")
        {
            int x = 0;
        }

        var result = typeRegistry.GetDeclaredBaseMembers(member);

        return result;
    }

    /// <inheritdoc/>
    protected override XElement? GetRawDocumentation(MemberData node)
    {
        return node.RawDocComment;
    }
}
