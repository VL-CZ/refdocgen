using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Concrete;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.InheritDoc;

internal abstract class InheritDocResolver<TNode>
    where TNode : notnull
{
    /// <summary>
    /// The registry of the declared types.
    /// </summary>
    private readonly TypeRegistry typeRegistry;

    /// <summary>
    /// Cache of the already resolved nodes and its corresponding doc comments.
    /// </summary>
    private readonly Dictionary<TNode, XElement?> cache = [];

    /// <summary>
    /// Initializes a new instance of <see cref="InheritDocResolver{TNode}"/> class.
    /// </summary>
    /// <param name="typeRegistry">The registry of the declared types.</param>
    public InheritDocResolver(TypeRegistry typeRegistry)
    {
        this.typeRegistry = typeRegistry;
    }

    /// <summary>
    /// Perform depth-first search resolvment of the documentation comment.
    /// </summary>
    /// <param name="node">Node, whose docummetation is to be resolved.</param>
    /// <returns>
    /// Raw doc comment of the parent member if found, <see langword="null"/> otherwise.
    /// </returns>
    internal XElement? DfsResolve(TNode node)
    {
        // the member is documented
        if (GetRawDocumentation(node) is XElement xmlDoc)
        {
            return xmlDoc;
        }

        // the member documentation is stored in the cache
        if (cache.TryGetValue(node, out var cached))
        {
            return cached;
        }

        // get the documentation from parent
        var parentNodes = GetParentNodes(node);

        foreach (var parentNode in parentNodes)
        {
            var parentDocComment = DfsResolve(parentNode);

            if (parentDocComment is not null)
            {
                cache[node] = parentDocComment;
                return parentDocComment;
            }
        }

        cache[node] = null;
        return null;
    }

    protected abstract XElement? GetRawDocumentation(TNode node);

    /// <summary>
    /// Get nodes representing the members with same ID in the parent types (base class and interfaces) of the current node.
    /// </summary>
    /// <param name="node">Node representing the member of a type.</param>
    /// <returns>
    /// List of nodes representing the members with same ID in the parent types (base class and interfaces) of the current node.
    /// </returns>
    protected abstract List<TNode> GetParentNodes(TNode node);

    protected List<TypeDeclaration> GetDeclaredParents(TypeDeclaration type)
    {
        var parentTypes = new List<ITypeNameData>();
        var result = new List<TypeDeclaration>();

        var baseType = type.BaseType;

        if (baseType is not null)
        {
            parentTypes.Add(baseType);
        }

        parentTypes.AddRange(type.Interfaces);

        foreach (var parentType in parentTypes)
        {
            // convert the ID: TODO refactor
            string parentId = parentType.HasTypeParameters
                ? $"{parentType.FullName}`{parentType.TypeParameters.Count}"
                : parentType.Id;

            // the parent type is contained in the type registry
            if (typeRegistry.ObjectTypes.TryGetValue(parentId, out var parent))
            {
                result.Add(parent);
            }
        }

        return result;
    }
}
