using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;
using RefDocGen.DocExtraction.Handlers.Members;
using RefDocGen.DocExtraction.Handlers.Members.Enum;
using RefDocGen.DocExtraction.Handlers.Types;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete;
using RefDocGen.CodeElements.Concrete.Types;

namespace RefDocGen.DocExtraction;

/// <summary>
/// Class responsible for extracting the XML documentation comments and adding them to the type data
/// </summary>
internal class DocCommentExtractor
{

    /// <summary>
    /// Registry of the declared types, to which the documentation comments will be added.
    /// </summary>
    private readonly TypeRegistry typeRegistry;

    /// <summary>
    /// XML document containing the documentation comments.
    /// </summary>
    private readonly XDocument xmlDocument;

    /// <summary>
    /// Dictionary of the selected member documentation handlers, identified by <see cref="MemberTypeId"/> identifiers.
    /// </summary>
    private readonly Dictionary<string, IMemberDocHandler<ObjectTypeData>> memberDocHandlers = new()
    {
        [MemberTypeId.Field] = new FieldDocHandler(),
        [MemberTypeId.Property] = new PropertyDocHandler(),
        [MemberTypeId.Method] = new MethodDocHandler(),
    };

    /// <summary>
    /// Handler for the constructor doc comments.
    /// </summary>
    private readonly ConstructorDocHandler constructorDocHandler = new();

    /// <summary>
    /// Handler for the operator doc comments.
    /// </summary>
    private readonly OperatorDocHandler operatorDocHandler = new();

    /// <summary>
    /// Handler for the indexer doc comments.
    /// </summary>
    private readonly IndexerDocHandler indexerDocHandler = new();

    /// <summary>
    /// Handler for the enum member doc comments.
    /// </summary>
    private readonly EnumMemberDocHandler enumMemberDocHandler = new();

    /// <summary>
    /// Handler for the type doc comments.
    /// </summary>
    private readonly TypeDocHandler typeDocHandler = new();

    /// <summary>
    /// Handler for the delegate type doc comments.
    /// </summary>
    private readonly DelegateTypeDocHandler delegateTypeDocHandler = new();

    /// <summary>
    /// Handler for the 'inheritdoc' doc comments.
    /// </summary>
    private readonly MemberInheritDocHandler memberInheritDocHandler;
    private readonly TypeInheritDocHandler typeInheritDocHandler;
    private readonly CrefInheritDocHandler crefInheritDocHandler;

    /// <summary>
    /// List of member records, that have 'inheritdoc' documentation.
    /// </summary>
    private readonly List<MemberData> inheritDocMembers = [];

    /// <summary>
    /// List of member records, that have 'inheritdoc' documentation.
    /// </summary>
    private readonly List<TypeDeclaration> inheritDocTypes = [];


    private bool inheriting;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocCommentExtractor"/> class.
    /// </summary>
    /// <param name="docXmlPath">Path to the XML documentation file.</param>
    /// <param name="typeRegistry">Registry of the declared types, to which the documentation comments will be added.</param>
    internal DocCommentExtractor(string docXmlPath, TypeRegistry typeRegistry)
    {
        this.typeRegistry = typeRegistry;
        memberInheritDocHandler = new(typeRegistry);
        typeInheritDocHandler = new(typeRegistry);
        crefInheritDocHandler = new(typeRegistry);

        // load the document
        xmlDocument = XDocument.Load(docXmlPath);
    }

    /// <summary>
    /// Extract the XML documentation comments and add them to the provided types and type members.
    /// </summary>
    internal void AddComments()
    {
        var memberNodes = xmlDocument.Descendants(XmlDocIdentifiers.Member);

        foreach (var memberNode in memberNodes)
        {
            AddDocComment(memberNode);
        }

        inheriting = true;

        // resolve inheritdoc comments
        foreach (var member in inheritDocMembers)
        {
            memberInheritDocHandler.DfsResolve(member);
            InheritDocumentation(member);
        }

        // resolve inheritdoc comments
        foreach (var type in inheritDocTypes)
        {
            typeInheritDocHandler.DfsResolve(type);
            InheritDocumentation(type);
        }

        // handle non-cref types & members
        foreach (var (_, type) in typeRegistry.ObjectTypes)
        {
            if (type.RawDocComment?.GetInheritDocs(InheritDocType.Cref).Any() ?? false)
            {
                crefInheritDocHandler.DfsResolve(type.RawDocComment);
                InheritDocumentation(type);
            }

            foreach (var (_, member) in type.AllMembers)
            {
                if (member.RawDocComment?.GetInheritDocs(InheritDocType.Cref).Any() ?? false)
                {
                    crefInheritDocHandler.DfsResolve(member.RawDocComment);
                    InheritDocumentation(member);
                }
            }
        }
    }

    /// <summary>
    /// Replaces the 'inheritdoc' doc comment of the member with the actual documentation.
    /// </summary>
    /// <param name="type">The member whose documentation is to be inherited.</param>
    private void InheritDocumentation(MemberData type)
    {
        if (type.RawDocComment?.Nodes().Any() ?? false)
        {
            AddDocComment(type.RawDocComment);
        }
    }

    private void InheritDocumentation(TypeDeclaration type)
    {
        if (type.RawDocComment?.Nodes().Any() ?? false)
        {
            AddDocComment(type.RawDocComment);
        }
    }

    /// <summary>
    /// Add doc comment to the corresponding type or type member.
    /// </summary>
    /// <param name="docCommentNode">Doc comment XML node.</param>
    private void AddDocComment(XElement docCommentNode)
    {
        // try to get type / member name
        if (docCommentNode.TryGetNameAttribute(out var memberNameAttr))
        {
            string[] splitMemberName = memberNameAttr.Value.Split(':');
            (string objectIdentifier, string fullObjectName) = (splitMemberName[0], splitMemberName[1]);

            if (objectIdentifier == MemberTypeId.Type) // type
            {
                AddTypeDocComment(fullObjectName, docCommentNode);
            }
            else if (objectIdentifier == MemberTypeId.Namespace) // namespace
            {
                // do nothing
            }
            else // Type member
            {
                AddMemberDocComment(objectIdentifier, fullObjectName, docCommentNode);
            }
        }
        else
        {
            // TODO: log unknown member / type
        }
    }

    /// <summary>
    /// Add doc comment to the given type.
    /// </summary>
    /// <param name="typeId">Id of the type.</param>
    /// <param name="docCommentNode">XML node of the doc comment for the given type.</param>
    private void AddTypeDocComment(string typeId, XElement docCommentNode)
    {
        // inheritdoc - TODO: update
        if (docCommentNode.GetInheritDocs(InheritDocType.NonCref).Any()
            && !inheriting
            && typeRegistry.GetDeclaredType(typeId) is TypeDeclaration t)
        {
            inheritDocTypes.Add(t);
        }

        if (typeRegistry.ObjectTypes.TryGetValue(typeId, out var type)) // the type is an 'object' type
        {
            typeDocHandler.AddDocumentation(type, docCommentNode);
        }
        else if (typeRegistry.Enums.TryGetValue(typeId, out var e)) // an enum type
        {
            typeDocHandler.AddDocumentation(e, docCommentNode);
        }
        else if (typeRegistry.Delegates.TryGetValue(typeId, out var d)) // a delegate type
        {
            delegateTypeDocHandler.AddDocumentation(d, docCommentNode);
        }
    }

    /// <summary>
    /// Add the doc comment to the corresponding type member.
    /// </summary>
    /// <param name="memberTypeId">Identifier of the member type.</param>
    /// <param name="fullMemberName">Fully qualified name of the member.</param>
    /// <param name="docCommentNode">XML node of the doc comment for the given type member.</param>
    private void AddMemberDocComment(string memberTypeId, string fullMemberName, XElement docCommentNode)
    {
        (string typeName, string memberName, string paramsString) = MemberSignatureParser.Parse(fullMemberName);

        string memberId = memberName + paramsString;

        if (typeRegistry.ObjectTypes.TryGetValue(typeName, out var type)) // member of a value, reference or interface type
        {
            // inheritdoc - TODO: update
            if (docCommentNode.GetInheritDocs(InheritDocType.NonCref).Any()
                && !inheriting
                && type.AllMembers.TryGetValue(memberId, out var member))
            {
                inheritDocMembers.Add(member);
            }

            if (memberTypeId == MemberTypeId.Method && memberName == ConstructorData.DefaultName) // the member is a constructor
            {
                constructorDocHandler.AddDocumentation(type, memberId, docCommentNode);
            }
            if (memberTypeId == MemberTypeId.Method && OperatorData.MethodNames.Contains(memberName)) // an operator
            {
                operatorDocHandler.AddDocumentation(type, memberId, docCommentNode);
            }
            if (memberTypeId == MemberTypeId.Property && type.Indexers.ContainsKey(memberId)) // an indexer
            {
                indexerDocHandler.AddDocumentation(type, memberId, docCommentNode);
            }
            else if (memberDocHandlers.TryGetValue(memberTypeId, out var handler))
            {
                handler.AddDocumentation(type, memberId, docCommentNode);
            }
            else
            {
                // TODO: log unknown member
            }
        }
        else if (typeRegistry.Enums.TryGetValue(typeName, out var e)) // member of an enum
        {
            enumMemberDocHandler.AddDocumentation(e, memberId, docCommentNode);
        }
    }
}
