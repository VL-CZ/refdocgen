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

    private readonly TypeDocResolver typeDocResolver;
    private readonly MemberDocResolver memberDocResolver;

    /// <summary>
    /// Initializes a new instance of <see cref="InheritDocHandler"/> class.
    /// </summary>
    /// <param name="typeRegistry">The registry of the declared types.</param>
    public InheritDocHandler(TypeRegistry typeRegistry)
    {
        this.typeRegistry = typeRegistry;

        typeDocResolver = new TypeDocResolver(typeRegistry);
        memberDocResolver = new MemberDocResolver(typeRegistry);
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
    internal IEnumerable<XNode> Resolve(TypeDeclaration type, XElement inheritDocElement)
    {
        XElement? resolvedDocComment;

        if (inheritDocElement.Attribute(XmlDocIdentifiers.Cref) is XAttribute crefAttr)
        {
            string[] splitMemberName = crefAttr.Value.Split(':');
            (string objectIdentifier, string fullObjectName) = (splitMemberName[0], splitMemberName[1]);

            resolvedDocComment = objectIdentifier == MemberTypeId.Type
                ? (typeRegistry.GetDeclaredType(fullObjectName)?.RawDocComment)
                : (typeRegistry.GetMember(fullObjectName)?.RawDocComment);
        }
        else // no cref attribute -> resolve recursively
        {
            var node = new TypeNode(type);
            resolvedDocComment = typeDocResolver.DfsResolve(node);
        }

        if (inheritDocElement.Attribute(XmlDocIdentifiers.Path) is XAttribute xpathAttr)
        {
            string xpath = XPathTools.MakeRelative(xpathAttr.Value);

            return resolvedDocComment?.XPathSelectElements(xpath) ?? [];
        }
        else
        {
            return resolvedDocComment?.Nodes() ?? [];
        }
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
    internal IEnumerable<XNode> Resolve(MemberData member, XElement inheritDocElement)
    {
        XElement? resolvedDocComment;

        if (inheritDocElement.Attribute(XmlDocIdentifiers.Cref) is XAttribute crefAttr)
        {
            string[] splitMemberName = crefAttr.Value.Split(':');
            (string objectIdentifier, string fullObjectName) = (splitMemberName[0], splitMemberName[1]);

            resolvedDocComment = objectIdentifier == MemberTypeId.Type
                ? (typeRegistry.GetDeclaredType(fullObjectName)?.RawDocComment)
                : (typeRegistry.GetMember(fullObjectName)?.RawDocComment);
        }
        else // no cref attribute -> resolve recursively
        {
            var node = new MemberNode(member);
            resolvedDocComment = memberDocResolver.DfsResolve(node);
        }

        if (inheritDocElement.Attribute(XmlDocIdentifiers.Path) is XAttribute xpathAttr)
        {
            string xpath = XPathTools.MakeRelative(xpathAttr.Value);

            return resolvedDocComment?.XPathSelectElements(xpath) ?? [];
        }
        else
        {
            return resolvedDocComment?.Nodes() ?? [];
        }
    }
}
