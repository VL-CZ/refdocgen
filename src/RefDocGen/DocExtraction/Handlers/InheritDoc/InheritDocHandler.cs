using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Concrete;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace RefDocGen.DocExtraction.Handlers.InheritDoc;

/// <summary>
/// Class responsible for handling the 'inheritdoc' comments and replacing them with the actual documentation.
/// </summary>
internal abstract class InheritDocHandler<TNode>
{
    /// <summary>
    /// The registry of the declared types.
    /// </summary>
    private readonly TypeRegistry typeRegistry;

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
    internal void DfsResolve(TNode node)
    {
        var rawDoc = GetRawDocumentation(node);

        // literal -> no need to continue
        if (IsLiteralDoc(rawDoc))
        {
            return;
        }

        var inheritDocs = GetNestedInheritDocs(rawDoc);

        foreach (var inheritDocElement in inheritDocs)
        {
            DfsResolve(node, inheritDocElement);
        }
    }

    /// <summary>
    /// Perform depth-first search resolvment of the documentation comment.
    /// </summary>
    /// <param name="node">Node, whose docummetation is to be resolved.</param>
    /// <returns>
    /// Raw doc comment of the parent member if found, <see langword="null"/> otherwise.
    /// </returns>
    private void DfsResolve(TNode node, XElement inheritDocElement)
    {
        // get the documentation from parent
        var parentNodes = GetParentNodes(node);

        foreach (var parentNode in parentNodes)
        {
            DfsResolve(parentNode);

            var rawParentDoc = GetRawDocumentation(parentNode);

            if (rawParentDoc is not null)
            {
                string? xpath = inheritDocElement.Attribute(XmlDocIdentifiers.Path)?.Value;

                if (xpath is not null)
                {
                    xpath = XPathTools.MakeRelative(xpath);

                    var xpathNodes = rawParentDoc.XPathSelectElements(xpath);
                    inheritDocElement.ReplaceWith(xpathNodes);
                }
                else
                {
                    inheritDocElement.ReplaceWith(rawParentDoc.Nodes());
                }
                return;
            }
        }
    }

    protected bool IsLiteralDoc(XElement? rawDocComment)
    {
        return !rawDocComment?.Descendants(XmlDocIdentifiers.InheritDoc).Any() ?? false;
    }

    protected List<XElement> GetNestedInheritDocs(XElement? rawDocComment)
    {
        return rawDocComment?.Descendants(XmlDocIdentifiers.InheritDoc)
            .ToList() ?? [];
    }

    /// <summary>
    /// Get nodes representing the members with same ID in the parent types (base class and interfaces) of the current node.
    /// </summary>
    /// <param name="member">Node representing the member of a type.</param>
    /// <returns>
    /// List of nodes representing the members with same ID in the parent types (base class and interfaces) of the current node.
    /// </returns>
    protected abstract List<TNode> GetParentNodes(TNode member);

    protected abstract XElement? GetRawDocumentation(TNode member);

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
