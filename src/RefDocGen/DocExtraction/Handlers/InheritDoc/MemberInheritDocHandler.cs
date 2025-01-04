using RefDocGen.CodeElements.Concrete;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.DocExtraction.Handlers.InheritDoc;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

/// <summary>
/// Class responsible for handling the 'inheritdoc' comments provided to type members and replacing them with the actual documentation.
/// </summary>
/// <remarks>
/// This class isn't intended to handle inheritdoc comments with 'cref' attribute (for further info, see <see cref="CrefInheritDocHandler"/>).
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
            if (p.AllMembers.TryGetValue(member.Id, out var parentMember))
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
