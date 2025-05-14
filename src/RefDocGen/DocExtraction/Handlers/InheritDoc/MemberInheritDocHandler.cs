using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Members.Concrete;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.InheritDoc;

/// <summary>
/// Class responsible for handling the 'inheritdoc' comments provided to type members and replacing them with the actual documentation.
/// </summary>
/// <remarks>
/// This class isn't intended to handle inheritdoc comments with 'cref' attribute (for further info, see <see cref="InheritDocCrefHandler"/>).
/// </remarks>
internal class MemberInheritDocHandler : InheritDocHandler<MemberData>
{
    /// <inheritdoc/>
    public MemberInheritDocHandler(TypeRegistry typeRegistry) : base(typeRegistry)
    {
    }

    /// <inheritdoc/>
    protected override InheritDocKind InheritDocKind => InheritDocKind.NonCref;

    /// <inheritdoc/>
    protected override IReadOnlyList<MemberData> GetParentNodes(MemberData member)
    {
        var parents = typeRegistry.GetDeclaredBaseTypes(member.ContainingType);

        var result = new List<MemberData>();

        foreach (var p in parents)
        {
            if (p.AllMembers.TryGetValue(member.Id, out var parentMember)) // TODO: explicit interface implementation & inheriting generic type
            {
                result.Add(parentMember);
            }
        }

        return result;
    }

    /// <inheritdoc/>
    protected override XElement? GetRawDocumentation(MemberData node)
    {
        return node.RawDocComment;
    }
}
