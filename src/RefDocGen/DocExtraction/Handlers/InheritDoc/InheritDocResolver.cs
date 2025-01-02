//using RefDocGen.CodeElements.Abstract.Types.TypeName;
//using RefDocGen.CodeElements.Concrete.Types;
//using RefDocGen.CodeElements.Concrete;
//using System.Xml.Linq;
//using RefDocGen.CodeElements.Concrete.Members;
//using RefDocGen.Tools.Xml;

//namespace RefDocGen.DocExtraction.Handlers.InheritDoc;

//internal record MemberNode(MemberData Member, string? Xpath);

//internal class InheritDocResolver
//{
//    /// <summary>
//    /// The registry of the declared types.
//    /// </summary>
//    private readonly TypeRegistry typeRegistry;

//    /// <summary>
//    /// Cache of the already resolved nodes and its corresponding doc comments.
//    /// </summary>
//    //private readonly Dictionary<MemberNode, XElement?> cache = [];

//    /// <summary>
//    /// Initializes a new instance of <see cref="InheritDocResolver{TNode}"/> class.
//    /// </summary>
//    /// <param name="typeRegistry">The registry of the declared types.</param>
//    public InheritDocResolver(TypeRegistry typeRegistry)
//    {
//        this.typeRegistry = typeRegistry;
//    }

//    /// <summary>
//    /// Perform depth-first search resolvment of the documentation comment.
//    /// </summary>
//    /// <param name="node">Node, whose docummetation is to be resolved.</param>
//    /// <returns>
//    /// Raw doc comment of the parent member if found, <see langword="null"/> otherwise.
//    /// </returns>
//    internal XElement? DfsResolve(MemberNode node)
//    {
//        // literal -> no need to continue
//        if (GetLiteralDocumentation(node) is XElement literalDoc)
//        {
//            return literalDoc;
//        }
//        //else if (node.Member.RawDocComment is null) // no documentation found
//        //{
//        //    cache[node] = null;
//        //    return null;
//        //}

//        // the member documentation is stored in the cache
//        //if (cache.TryGetValue(node, out var cached))
//        //{
//        //    return cached is not null
//        //        ? new XElement(cached)
//        //        : null;
//        //}

//        // get the documentation from parent
//        var parentNodes = GetParentNodes(node);

//        foreach (var parentNode in parentNodes)
//        {
//            var parentDocComment = DfsResolve(parentNode);

//            if (parentDocComment is not null)
//            {
//                //cache[node] = parentDocComment;
//                return parentDocComment;
//            }
//        }


//    }

//    protected XElement? GetLiteralDocumentation(MemberNode node)
//    {
//        return node.Member.IsInheritDoc
//            ? null
//            : node.Member.RawDocComment;
//    }

//    private XElement? Select(XElement? element, string? xpath)
//    {
//        if (element is not null && xpath is not null)
//        {

//        }
//        else
//        {
//            return element;
//        }
//    }

//    /// <summary>
//    /// Get nodes representing the members with same ID in the parent types (base class and interfaces) of the current node.
//    /// </summary>
//    /// <param name="node">Node representing the member of a type.</param>
//    /// <returns>
//    /// List of nodes representing the members with same ID in the parent types (base class and interfaces) of the current node.
//    /// </returns>
//    protected List<MemberNode> GetParentNodes(MemberNode node)
//    {
//        var parents = GetDeclaredParents(node.Member.DeclaringType);

//        var result = new List<MemberNode>();

//        foreach (var p in parents)
//        {
//            if (p.AllMembers.TryGetValue(node.Member.Id, out var parentMember))
//            {
//                string? xpath = inheritDocElement.Attribute(XmlDocIdentifiers.Path)?.Value;
//                result.Add(new MemberNode(parentMember, xpath));
//            }
//        }

//        return result;
//    }

//    protected List<TypeDeclaration> GetDeclaredParents(TypeDeclaration type)
//    {
//        var parentTypes = new List<ITypeNameData>();
//        var result = new List<TypeDeclaration>();

//        var baseType = type.BaseType;

//        if (baseType is not null)
//        {
//            parentTypes.Add(baseType);
//        }

//        parentTypes.AddRange(type.Interfaces);

//        foreach (var parentType in parentTypes)
//        {
//            // convert the ID: TODO refactor
//            string parentId = parentType.HasTypeParameters
//                ? $"{parentType.FullName}`{parentType.TypeParameters.Count}"
//                : parentType.Id;

//            // the parent type is contained in the type registry
//            if (typeRegistry.ObjectTypes.TryGetValue(parentId, out var parent))
//            {
//                result.Add(parent);
//            }
//        }

//        return result;
//    }
//}//using RefDocGen.CodeElements.Abstract.Types.TypeName;
//using RefDocGen.CodeElements.Concrete.Types;
//using RefDocGen.CodeElements.Concrete;
//using System.Xml.Linq;
//using RefDocGen.CodeElements.Concrete.Members;
//using RefDocGen.Tools.Xml;

//namespace RefDocGen.DocExtraction.Handlers.InheritDoc;

//internal record MemberNode(MemberData Member, string? Xpath);

//internal class InheritDocResolver
//{
//    /// <summary>
//    /// The registry of the declared types.
//    /// </summary>
//    private readonly TypeRegistry typeRegistry;

//    /// <summary>
//    /// Cache of the already resolved nodes and its corresponding doc comments.
//    /// </summary>
//    //private readonly Dictionary<MemberNode, XElement?> cache = [];

//    /// <summary>
//    /// Initializes a new instance of <see cref="InheritDocResolver{TNode}"/> class.
//    /// </summary>
//    /// <param name="typeRegistry">The registry of the declared types.</param>
//    public InheritDocResolver(TypeRegistry typeRegistry)
//    {
//        this.typeRegistry = typeRegistry;
//    }

//    /// <summary>
//    /// Perform depth-first search resolvment of the documentation comment.
//    /// </summary>
//    /// <param name="node">Node, whose docummetation is to be resolved.</param>
//    /// <returns>
//    /// Raw doc comment of the parent member if found, <see langword="null"/> otherwise.
//    /// </returns>
//    internal XElement? DfsResolve(MemberNode node)
//    {
//        // literal -> no need to continue
//        if (GetLiteralDocumentation(node) is XElement literalDoc)
//        {
//            return literalDoc;
//        }
//        //else if (node.Member.RawDocComment is null) // no documentation found
//        //{
//        //    cache[node] = null;
//        //    return null;
//        //}

//        // the member documentation is stored in the cache
//        //if (cache.TryGetValue(node, out var cached))
//        //{
//        //    return cached is not null
//        //        ? new XElement(cached)
//        //        : null;
//        //}

//        // get the documentation from parent
//        var parentNodes = GetParentNodes(node);

//        foreach (var parentNode in parentNodes)
//        {
//            var parentDocComment = DfsResolve(parentNode);

//            if (parentDocComment is not null)
//            {
//                //cache[node] = parentDocComment;
//                return parentDocComment;
//            }
//        }


//    }

//    protected XElement? GetLiteralDocumentation(MemberNode node)
//    {
//        return node.Member.IsInheritDoc
//            ? null
//            : node.Member.RawDocComment;
//    }

//    private XElement? Select(XElement? element, string? xpath)
//    {
//        if (element is not null && xpath is not null)
//        {

//        }
//        else
//        {
//            return element;
//        }
//    }

//    /// <summary>
//    /// Get nodes representing the members with same ID in the parent types (base class and interfaces) of the current node.
//    /// </summary>
//    /// <param name="node">Node representing the member of a type.</param>
//    /// <returns>
//    /// List of nodes representing the members with same ID in the parent types (base class and interfaces) of the current node.
//    /// </returns>
//    protected List<MemberNode> GetParentNodes(MemberNode node)
//    {
//        var parents = GetDeclaredParents(node.Member.DeclaringType);

//        var result = new List<MemberNode>();

//        foreach (var p in parents)
//        {
//            if (p.AllMembers.TryGetValue(node.Member.Id, out var parentMember))
//            {
//                string? xpath = inheritDocElement.Attribute(XmlDocIdentifiers.Path)?.Value;
//                result.Add(new MemberNode(parentMember, xpath));
//            }
//        }

//        return result;
//    }

//    protected List<TypeDeclaration> GetDeclaredParents(TypeDeclaration type)
//    {
//        var parentTypes = new List<ITypeNameData>();
//        var result = new List<TypeDeclaration>();

//        var baseType = type.BaseType;

//        if (baseType is not null)
//        {
//            parentTypes.Add(baseType);
//        }

//        parentTypes.AddRange(type.Interfaces);

//        foreach (var parentType in parentTypes)
//        {
//            // convert the ID: TODO refactor
//            string parentId = parentType.HasTypeParameters
//                ? $"{parentType.FullName}`{parentType.TypeParameters.Count}"
//                : parentType.Id;

//            // the parent type is contained in the type registry
//            if (typeRegistry.ObjectTypes.TryGetValue(parentId, out var parent))
//            {
//                result.Add(parent);
//            }
//        }

//        return result;
//    }
//}
