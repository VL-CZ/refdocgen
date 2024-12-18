using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Concrete;
using RefDocGen.CodeElements.Concrete.Types;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers;

internal record MemberRecord(ObjectTypeData Type, string MemberId, XElement DocComment);

internal class InheritDocHandler
{
    private record Node(ObjectTypeData Type, string MemberId);

    /// <summary>
    /// Registry of the declared types, to which the documentation comments will be added.
    /// </summary>
    private readonly TypeRegistry typeRegistry;
    private List<XElement> toReturn = [];

    private readonly Dictionary<Node, XElement> done = [];
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
        var node = new Node(memberRecord.Type, memberRecord.MemberId);
        var docComment = LoadComment(node);

        if (docComment is null)
        {
            return;
        }

        memberRecord.DocComment.RemoveNodes();
        memberRecord.DocComment.Add(docComment.Nodes());

        toReturn.Add(memberRecord.DocComment);
    }

    private XElement? LoadComment(Node node)
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
        if (done.TryGetValue(node, out var cached))
        {
            return cached;
        }

        // get the documentation.
        var parentNodes = GetParentNodes(node);

        foreach (var parentNode in parentNodes)
        {
            var parentDocComment = LoadComment(parentNode);

            if (parentDocComment is not null)
            {
                done[node] = parentDocComment;
                return parentDocComment;
            }
        }

        return null;
    }

    private List<Node> GetParentNodes(Node node)
    {
        var parentTypes = new List<ITypeNameData>();

        var baseType = node.Type.BaseType;

        if (baseType is not null)
        {
            parentTypes.Add(baseType);
        }

        parentTypes.AddRange(node.Type.Interfaces);

        List<Node> returnValues = [];

        foreach (var parentType in parentTypes)
        {
            var parentId = parentType.HasTypeParameters
                ? $"{parentType.FullName}`{parentType.TypeParameters.Count}"
                : parentType.Id;

            if (typeRegistry.ObjectTypes.TryGetValue(parentId, out var parent))
            {
                returnValues.Add(
                    new Node(parent, node.MemberId)
                    );
            }
        }

        return returnValues;
    }
}
