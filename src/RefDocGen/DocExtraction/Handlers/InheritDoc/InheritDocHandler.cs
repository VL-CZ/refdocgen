using Microsoft.CodeAnalysis.CSharp.Syntax;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Concrete;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace RefDocGen.DocExtraction.Handlers.InheritDoc;

/// <summary>
/// Class responsible for handling the 'inheritdoc' comments and replacing them with the actual documentation.
/// </summary>
internal class InheritDocHandler
{
    /// <summary>
    /// The registry of the declared types.
    /// </summary>
    private readonly TypeRegistry typeRegistry;

    //private readonly TypeDocResolver typeDocResolver;
    //private readonly MemberDocResolver memberDocResolver;
    //private readonly InheritDocResolver docResolver;

    /// <summary>
    /// Initializes a new instance of <see cref="InheritDocHandler"/> class.
    /// </summary>
    /// <param name="typeRegistry">The registry of the declared types.</param>
    public InheritDocHandler(TypeRegistry typeRegistry)
    {
        this.typeRegistry = typeRegistry;

        //typeDocResolver = new TypeDocResolver(typeRegistry);
        //memberDocResolver = new MemberDocResolver(typeRegistry);

        //docResolver = new InheritDocResolver(typeRegistry);
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
    //internal IEnumerable<XNode> Resolve(TypeDeclaration type, XElement inheritDocElement)
    //{
    //    XElement? resolvedDocComment;

    //    if (inheritDocElement.Attribute(XmlDocIdentifiers.Cref) is XAttribute crefAttr)
    //    {
    //        string[] splitMemberName = crefAttr.Value.Split(':');
    //        (string objectIdentifier, string fullObjectName) = (splitMemberName[0], splitMemberName[1]);

    //        resolvedDocComment = objectIdentifier == MemberTypeId.Type
    //            ? (typeRegistry.GetDeclaredType(fullObjectName)?.RawDocComment)
    //            : (typeRegistry.GetMember(fullObjectName)?.RawDocComment);
    //    }
    //    else // no cref attribute -> resolve recursively
    //    {
    //        var node = new TypeNode(type);
    //        resolvedDocComment = typeDocResolver.DfsResolve(node);
    //    }

    //    if (inheritDocElement.Attribute(XmlDocIdentifiers.Path) is XAttribute xpathAttr)
    //    {
    //        string xpath = XPathTools.MakeRelative(xpathAttr.Value);

    //        return resolvedDocComment?.XPathSelectElements(xpath) ?? [];
    //    }
    //    else
    //    {
    //        return resolvedDocComment?.Nodes() ?? [];
    //    }
    //}

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
    internal void Resolve(MemberData member)
    {
        //if (inheritDocElement.Attribute(XmlDocIdentifiers.Cref) is XAttribute crefAttr)
        //{
        //    string[] splitMemberName = crefAttr.Value.Split(':');
        //    (string objectIdentifier, string fullObjectName) = (splitMemberName[0], splitMemberName[1]);

        //    resolvedDocComment = objectIdentifier == MemberTypeId.Type
        //        ? (typeRegistry.GetDeclaredType(fullObjectName)?.RawDocComment)
        //        : (typeRegistry.GetMember(fullObjectName)?.RawDocComment);
        //}
        //else // no cref attribute -> resolve recursively
        //{

        var inheritDocs = member.RawDocComment?.Elements(XmlDocIdentifiers.InheritDoc).ToList() ?? [];

        foreach (var inheritDocElement in inheritDocs)
        {
            DfsResolve(member, inheritDocElement);
        }
    }

    // <summary>
    /// Perform depth-first search resolvment of the documentation comment.
    /// </summary>
    /// <param name="node">Node, whose docummetation is to be resolved.</param>
    /// <returns>
    /// Raw doc comment of the parent member if found, <see langword="null"/> otherwise.
    /// </returns>
    internal void DfsResolve(MemberData node, XElement inheritDocElement)
    {
        // literal -> no need to continue
        if (GetLiteralDocumentation(node) is not null)
        {
            return;
        }
        //else if (node.Member.RawDocComment is null) // no documentation found
        //{
        //    cache[node] = null;
        //    return null;
        //}

        // the member documentation is stored in the cache
        //if (cache.TryGetValue(node, out var cached))
        //{
        //    return cached is not null
        //        ? new XElement(cached)
        //        : null;
        //}

        // get the documentation from parent
        var parentNodes = GetParentNodes(node);

        foreach (var parentNode in parentNodes)
        {
            Resolve(parentNode);

            if (parentNode.RawDocComment is not null)
            {
                //cache[node] = parentDocComment;
                inheritDocElement.ReplaceWith(parentNode.RawDocComment.Nodes());
            }
        }


    }

    protected XElement? GetLiteralDocumentation(MemberData member)
    {
        return member.IsInheritDoc
            ? null
            : member.RawDocComment;
    }

    /// <summary>
    /// Get nodes representing the members with same ID in the parent types (base class and interfaces) of the current node.
    /// </summary>
    /// <param name="member">Node representing the member of a type.</param>
    /// <returns>
    /// List of nodes representing the members with same ID in the parent types (base class and interfaces) of the current node.
    /// </returns>
    protected List<MemberData> GetParentNodes(MemberData member)
    {
        var parents = GetDeclaredParents(member.DeclaringType);

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
