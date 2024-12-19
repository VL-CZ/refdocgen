using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Concrete;
using RefDocGen.CodeElements.Concrete.Types;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers;

internal record MemberRecord(ObjectTypeData Type, string MemberId, XElement DocComment);

internal class InheritDocHandler
{
    private record MemberNode(ObjectTypeData Type, string MemberId);

    /// <summary>
    /// Registry of the declared types.
    /// </summary>
    private readonly TypeRegistry typeRegistry;
    private List<XElement> toReturn = [];

    /// <summary>
    /// Cache of the already resolved nodes and its corresponding doc comments.
    /// </summary>
    private readonly Dictionary<MemberNode, XElement> cache = [];
    private List<MemberRecord> inheritDocs;

    public InheritDocHandler(TypeRegistry typeRegistry, List<MemberRecord> inheritDocs)
    {
        this.typeRegistry = typeRegistry;
        this.inheritDocs = inheritDocs;
    }

    internal IEnumerable<XElement> Handle()
    {
        foreach (var inheritDoc in inheritDocs)
        {
            Handle(inheritDoc);
        }

        return toReturn;
    }

    private void Handle(MemberRecord memberRecord)
    {
        var node = new MemberNode(memberRecord.Type, memberRecord.MemberId);
        var docComment = DfsForDocumentation(node);

        if (docComment is null)
        {
            return;
        }

        memberRecord.DocComment.RemoveNodes();
        memberRecord.DocComment.Add(docComment.Nodes());

        toReturn.Add(memberRecord.DocComment);
    }

    /// <summary>
    /// Perform depth-first search for the documentation comment.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private XElement? DfsForDocumentation(MemberNode node)
    {
        // the member is not contained in the type -> return null
        if (!node.Type.AllMembers.TryGetValue(node.MemberId, out var member))
        {
            return null;
        }

        // the member is documented.
        if (member.RawDocComment is not null)
        {
            return member.RawDocComment;
        }

        // the member documentation is stored in the cache.
        if (cache.TryGetValue(node, out var cached))
        {
            return cached;
        }

        // get the documentation.
        var parentNodes = GetParentMemberNodes(node);

        foreach (var parentNode in parentNodes)
        {
            var parentDocComment = DfsForDocumentation(parentNode);

            if (parentDocComment is not null)
            {
                cache[node] = parentDocComment;
                return parentDocComment;
            }
        }

        return null;
    }

    /// <summary>
    /// Get nodes representing the members with same ID in the parent types (base class and interfaces) of the current node.
    /// </summary>
    /// <param name="node">Node representing the member of a type.</param>
    /// <returns>
    /// List of nodes representing the members with same ID in the parent types (base class and interfaces) of the current node.
    /// </returns>
    private List<MemberNode> GetParentMemberNodes(MemberNode node)
    {
        var parentTypes = new List<ITypeNameData>();

        var baseType = node.Type.BaseType;

        if (baseType is not null)
        {
            parentTypes.Add(baseType);
        }

        parentTypes.AddRange(node.Type.Interfaces);

        List<MemberNode> parentNodes = [];

        foreach (var parentType in parentTypes)
        {
            // convert the ID: TODO refactor
            string parentId = parentType.HasTypeParameters
                ? $"{parentType.FullName}`{parentType.TypeParameters.Count}"
                : parentType.Id;

            // the parent type is contained in the type registry
            if (typeRegistry.ObjectTypes.TryGetValue(parentId, out var parent))
            {
                parentNodes.Add(
                    new MemberNode(parent, node.MemberId)
                    );
            }
        }

        return parentNodes;
    }
}
