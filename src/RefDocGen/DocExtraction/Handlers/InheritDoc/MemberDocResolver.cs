using RefDocGen.CodeElements.Concrete;
using RefDocGen.CodeElements.Concrete.Members;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.InheritDoc;

internal record MemberNode(MemberData Member);

/// <summary>
/// Class responsible for handling the 'inheritdoc' comments and replacing them with the actual documentation.
/// </summary>
internal class MemberDocResolver : InheritDocResolver<MemberNode>
{
    /// <inheritdoc/>
    public MemberDocResolver(TypeRegistry typeRegistry) : base(typeRegistry)
    {
    }

    protected override List<MemberNode> GetParentNodes(MemberNode node)
    {
        var parents = GetDeclaredParents(node.Member.DeclaringType);

        var result = new List<MemberNode>();

        foreach (var p in parents)
        {
            if (p.AllMembers.TryGetValue(node.Member.Id, out var parentMember))
            {
                result.Add(new MemberNode(parentMember));
            }
        }

        return result;
    }

    protected override XElement? GetRawDocumentation(MemberNode node)
    {
        return node.Member.RawDocComment;
    }
}
