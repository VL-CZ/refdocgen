using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Concrete;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace RefDocGen.DocExtraction.Handlers;

/// <summary>
/// Class responsible for handling the 'inheritdoc' comments and replacing them with the actual documentation.
/// </summary>
internal class InheritDocHandler
{
    private record MemberNode(ObjectTypeData Type, string MemberId);

    /// <summary>
    /// The registry of the declared types.
    /// </summary>
    private readonly TypeRegistry typeRegistry;

    /// <summary>
    /// Cache of the already resolved nodes and its corresponding doc comments.
    /// </summary>
    private readonly Dictionary<MemberNode, XElement> cache = [];

    /// <summary>
    /// Initializes a new instance of <see cref="InheritDocHandler"/> class.
    /// </summary>
    /// <param name="typeRegistry">The registry of the declared types.</param>
    public InheritDocHandler(TypeRegistry typeRegistry)
    {
        this.typeRegistry = typeRegistry;
    }

    /// <summary>
    /// Resolve the documentation for the selected member.
    /// </summary>
    /// <param name="type">The type containing the member.</param>
    /// <param name="memberId">Id of the member, whose documentation is to be resolved.</param>
    /// <returns>
    /// Enumerable of resolved documentation comments.
    /// <para>
    /// These comments are intended to replace the 'inheritdoc' element.
    /// </para>
    /// <para>
    /// If there are no suitable comments found, an empty enumerable is returned.
    /// </para>
    /// </returns>
    /// <remarks>
    /// The resolvement is done recursively using DFS, firstly we try to resolve the base class member (if existing),
    /// then we resolve the interface members one-by-one.
    /// </remarks>
    internal IEnumerable<XNode> Resolve(ObjectTypeData type, string memberId, XElement inheritDocElement)
    {
        XElement? resolvedDocComment;

        if (inheritDocElement.Attribute(XmlDocIdentifiers.Cref) is XAttribute crefAttr)
        {
            string[] splitMemberName = crefAttr.Value.Split(':');
            (string objectIdentifier, string fullObjectName) = (splitMemberName[0], splitMemberName[1]);

            if (objectIdentifier == MemberTypeId.Type) // type
            {
                resolvedDocComment = typeRegistry.GetType(fullObjectName)?.RawDocComment;
            }
            else // member
            {
                resolvedDocComment = typeRegistry.GetMember(fullObjectName)?.RawDocComment;
            }
        }
        else // no cref attribute -> resolve recursively
        {
            var node = new MemberNode(type, memberId);
            resolvedDocComment = DfsResolve(node);
        }

        if (inheritDocElement.Attribute(XmlDocIdentifiers.Path) is XAttribute xpathAttr)
        {
            string xpath = xpathAttr.Value;

            return resolvedDocComment?.XPathSelectElements(xpath) ?? [];
        }
        else
        {
            return resolvedDocComment?.Nodes() ?? [];
        }
    }

    /// <summary>
    /// Perform depth-first search resolvment of the documentation comment.
    /// </summary>
    /// <param name="node">Node, whose docummetation is to be resolved.</param>
    /// <returns>
    /// Raw doc comment of the parent member if found, <see langword="null"/> otherwise.
    /// </returns>
    private XElement? DfsResolve(MemberNode node)
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

        // get the documentation from parent.
        var parentNodes = GetParentMemberNodes(node);

        foreach (var parentNode in parentNodes)
        {
            var parentDocComment = DfsResolve(parentNode);

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
