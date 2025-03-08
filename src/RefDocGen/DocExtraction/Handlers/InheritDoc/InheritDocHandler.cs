using RefDocGen.CodeElements.Concrete;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace RefDocGen.DocExtraction.Handlers.InheritDoc;

/// <summary>
/// Generic base class responsible for handling the 'inheritdoc' comments and replacing them with the actual documentation
/// </summary>
/// <typeparam name="TNode">Type of the nodes containing the documentation.</typeparam>
/// <remarks>
/// See <see cref="DfsResolve(TNode)"/> for the detailed description of the process.
/// </remarks>
internal abstract class InheritDocHandler<TNode>
    where TNode : notnull
{
    /// <summary>
    /// The registry of the declared types.
    /// </summary>
    protected readonly TypeRegistry typeRegistry;

    /// <summary>
    /// Set of visited nodes.
    /// </summary>
    private readonly HashSet<TNode> visited = [];

    /// <summary>
    /// Initializes a new instance of <see cref="InheritDocHandler{TNode}"/> class.
    /// </summary>
    /// <param name="typeRegistry">The registry of the declared types.</param>
    public InheritDocHandler(TypeRegistry typeRegistry)
    {
        this.typeRegistry = typeRegistry;
    }

    /// <summary>
    /// Kind of the inherit doc elements to be resolved.
    /// </summary>
    protected abstract InheritDocKind InheritDocKind { get; }

    /// <inheritdoc cref="DfsResolve(TNode)"/>
    internal void Resolve(TNode node)
    {
        visited.Clear(); // clear the visited nodes
        DfsResolve(node);
    }

    /// <summary>
    /// Resolve the documentation for the selected node.
    ///
    /// <para>
    /// The process consists of the following steps:
    /// </para>
    /// <list type="number">
    ///   <item>Select all <c>inheritdoc</c> elements contained in the node documentation.</item>
    ///   <item>
    ///         Resolve them one by one.
    ///
    ///         <para>
    ///           The resolvment is done recursively using DFS and also resolves the documentation of all visited nodes.
    ///         </para>
    ///         <para>
    ///            The end condition of recursion are:
    ///            <list type="bullet">
    ///                 <item>The node documentation has no <c>inheritdoc</c> elements (i.e. nothing to resolve)</item>
    ///                 <item>The node has no parent nodes (example: the type whose documentation is being resolved implements no interfaces and its base type is outside the analyzed assembly).</item>
    ///             </list>
    ///             Also note that the cycles are detected - the parent nodes that have already been visited are filtered out.
    ///         </para>
    ///   </item>
    ///   <item>Replace the <c>inheritdoc</c> elements with the resolved documentation (if there's no documentation resolved, the <c>inheritdoc</c> element is simply removed).</item>
    /// </list>
    /// <para>
    ///     As a result, all of the 'inheritdoc' elements in the mode documentation are replaced by the actual documentation (if possible).
    /// </para>
    /// </summary>
    /// <param name="node">The node whose documentation is to be resolved.</param>
    private void DfsResolve(TNode node)
    {
        var inheritDocs = GetNestedInheritDocs(node);

        if (inheritDocs.Count == 0)
        {
            return; // no nested inheritdocs -> return
        }

        _ = visited.Add(node); // mark the current node as visited

        // nested inheritdocs found -> resolve them one by one
        foreach (var inheritDocElement in inheritDocs)
        {
            DfsResolve(node, inheritDocElement);
        }
    }

    /// <summary>
    /// Resolve the <c>inheritdoc</c> XML element contained in the documentation of the given <paramref name="node"/>.
    ///
    /// <para>
    /// See <see cref="DfsResolve(TNode)"/> for detailed description of the resolvement process.
    /// </para>
    /// </summary>
    /// <param name="node">
    ///     <inheritdoc cref="DfsResolve(TNode)" path="/param[@name='node']/text()"/>
    /// </param>
    /// <param name="inheritDocElement">The <c>inheritdoc</c> XML element to resolve.</param>
    private void DfsResolve(TNode node, XElement inheritDocElement)
    {
        // get the parent nodes
        var parentNodes = GetParentNodes(node)
            .Where(p => !visited.Contains(p)); // filter out the already visited neighbours

        foreach (var parentNode in parentNodes)
        {
            DfsResolve(parentNode); // resolve the parent node

            var rawParentDoc = GetRawDocumentation(parentNode);

            if (rawParentDoc is null)
            {
                continue; // the parent node isn't documented -> continue with the next parent
            }

            IEnumerable<XNode> resolvedNodes;
            string? xpath = inheritDocElement.Attribute(XmlDocIdentifiers.Path)?.Value;

            if (xpath is not null) // XPath provided
            {
                xpath = XPathTools.MakeRelative(xpath);
                resolvedNodes = rawParentDoc.XPathSelectElements(xpath); // select the nodes matching XPath
            }
            else // no XPath provided
            {
                resolvedNodes = rawParentDoc.Nodes();
            }

            if (resolvedNodes.Any()) // there are resolved nodes from the parent -> replace the 'inheritdoc' element with them
            {
                inheritDocElement.ReplaceWith(resolvedNodes);
                return;
            }
        }

        inheritDocElement.Remove(); // can't resolve the inheritdoc -> remove the element (don't replace it with anything)
    }

    /// <summary>
    /// Gets list of nested inheritdoc elements contained in the provided <paramref name="node"/>.
    /// </summary>
    /// <param name="node">Node that is searched for the inheritdoc elements.</param>
    /// <returns>List of nested inheritdoc elements contained in <paramref name="node"/>.</returns>
    protected IReadOnlyList<XElement> GetNestedInheritDocs(TNode node)
    {
        return [.. GetRawDocumentation(node).GetInheritDocs(InheritDocKind)];
    }

    /// <summary>
    /// Gets raw XML documentation of the given node.
    /// </summary>
    /// <param name="node">The node whose documentation is to be returned.</param>
    /// <returns>Raw XML documentation of the given node.</returns>
    protected abstract XElement? GetRawDocumentation(TNode node);

    /// <summary>
    /// Get nodes representing the parent of <paramref name="node"/>.
    /// </summary>
    /// <param name="node">Node, whose parents are returned.</param>
    /// <returns>
    /// List of nodes representing the parents of <paramref name="node"/>.
    /// </returns>
    /// <remarks>
    /// Note that the order of parents is important, since the resolving is done recursively using DFS (i.e. if the documentation is resolved from the 1st parent, the 2nd isn't even visited).
    /// </remarks>
    protected abstract IReadOnlyList<TNode> GetParentNodes(TNode node);
}
