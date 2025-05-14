using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;
using RefDocGen.DocExtraction.Handlers.Members;
using RefDocGen.DocExtraction.Handlers.Members.Enum;
using RefDocGen.DocExtraction.Handlers.Types;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Handlers.InheritDoc;

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
    /// Dictionary of the selected member documentation handlers, identified by <see cref="CodeElementId"/> identifiers.
    /// </summary>
    private readonly Dictionary<string, IMemberDocHandler<ObjectTypeData>> memberDocHandlers = new()
    {
        [CodeElementId.Field] = new FieldDocHandler(),
        [CodeElementId.Property] = new PropertyDocHandler(),
        [CodeElementId.Method] = new MethodDocHandler(),
        [CodeElementId.Event] = new EventDocHandler()
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
    /// Handler for the 'inheritdoc' member comments.
    /// </summary>
    private readonly MemberInheritDocHandler memberInheritDocHandler;

    /// <summary>
    /// Handler for the 'inheritdoc' type comments.
    /// </summary>
    private readonly TypeInheritDocHandler typeInheritDocHandler;

    /// <summary>
    /// Handler for the 'inheritdoc' cref comments.
    /// </summary>
    private readonly InheritDocCrefHandler crefInheritDocHandler;

    /// <summary>
    /// List of members, that have 'inheritdoc' documentation without 'cref' attribute.
    /// </summary>
    private readonly List<MemberData> inheritDocMembers = [];

    /// <summary>
    /// List of types, that have 'inheritdoc' documentation without 'cref' attribute.
    /// </summary>
    private readonly List<TypeDeclaration> inheritDocTypes = [];

    /// <summary>
    /// Checks whether we're in the phase of adding resolved inheritdocs.
    /// </summary>
    private bool addingInheritedDocs;

    /// <summary>
    /// Paths to the XML documentation files.
    /// </summary>
    private readonly string[] docXmlPaths;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocCommentExtractor"/> class.
    /// </summary>
    /// <param name="docXmlPaths">Paths to the XML documentation files.</param>
    /// <param name="typeRegistry">Registry of the declared types, to which the documentation comments will be added.</param>
    internal DocCommentExtractor(string[] docXmlPaths, TypeRegistry typeRegistry)
    {
        this.typeRegistry = typeRegistry;
        this.docXmlPaths = docXmlPaths;

        memberInheritDocHandler = new(typeRegistry);
        typeInheritDocHandler = new(typeRegistry);
        crefInheritDocHandler = new(typeRegistry);
    }

    /// <summary>
    /// Extract the XML documentation comments and add them to the provided types and type members.
    /// </summary>
    internal void AddComments()
    {
        foreach (var xmlPath in docXmlPaths)
        {
            // load the document (preserve the whitespace, as the documentation is to be converted into HTML)
            var xmlDocument = XDocument.Load(xmlPath, LoadOptions.PreserveWhitespace);

            var memberNodes = xmlDocument.Descendants(XmlDocIdentifiers.Member);

            // add the doc comments
            foreach (var memberNode in memberNodes)
            {
                AddDocComment(memberNode);
            }
        }

        // from now on, we're adding the resolved inheritdocs
        addingInheritedDocs = true;

        // resolve member inheritdoc comments (excluding 'cref')
        foreach (var member in inheritDocMembers)
        {
            memberInheritDocHandler.Resolve(member);
            AddInheritedDocComment(member.RawDocComment);
        }

        // resolve type inheritdoc comments (excluding 'cref')
        foreach (var type in inheritDocTypes)
        {
            typeInheritDocHandler.Resolve(type);
            AddInheritedDocComment(type.RawDocComment);
        }

        // resolve 'cref' inheritdoc comments
        ResolveCrefInheritDocs();

        foreach (var member in typeRegistry.AllTypes.Values.SelectMany(t => t.AllMembers.Values))
        {
            if (member.IsInherited)
            {
                member.RawDocComment = new XElement("member", new XElement("inheritdoc"));
                memberInheritDocHandler.Resolve(member);

                if (member.RawDocComment?.Nodes().Any() ?? false)
                {
                    AddMemberDocComment(member.MemberKindId, $"{member.ContainingType.Id}.{member.Id}", member.RawDocComment);
                }
            }
        }
    }

    /// <summary>
    /// Add doc comment to the corresponding type or type member.
    /// </summary>
    /// <param name="docComment">Doc comment to add.</param>
    private void AddDocComment(XElement docComment)
    {
        // try to get type / member name
        if (docComment.TryGetNameAttribute(out var memberNameAttr))
        {
            string[] splitMemberName = memberNameAttr.Value.Split(':');
            (string objectIdentifier, string fullObjectName) = (splitMemberName[0], splitMemberName[1]);

            if (objectIdentifier == CodeElementId.Type) // type
            {
                AddTypeDocComment(fullObjectName, docComment);
            }
            else if (objectIdentifier == CodeElementId.Namespace) // namespace
            {
                // do nothing
            }
            else // Type member
            {
                AddMemberDocComment(objectIdentifier, fullObjectName, docComment);
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
        if (docCommentNode.GetInheritDocs(InheritDocKind.NonCref).Any()
            && !addingInheritedDocs
            && typeRegistry.GetDeclaredType(typeId) is TypeDeclaration t) // non-cref inheritdoc found
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
    /// <param name="memberKindId">Identifier of the member kind.</param>
    /// <param name="fullMemberName">Fully qualified name of the member.</param>
    /// <param name="docCommentNode">XML node of the doc comment for the given type member.</param>
    private void AddMemberDocComment(string memberKindId, string fullMemberName, XElement docCommentNode)
    {
        (string typeName, string memberName, string paramsString) = MemberSignatureParser.Parse(fullMemberName);

        string memberId = memberName + paramsString;

        if (typeRegistry.ObjectTypes.TryGetValue(typeName, out var type)) // member of a value, reference or interface type
        {
            if (docCommentNode.GetInheritDocs(InheritDocKind.NonCref).Any()
                && !addingInheritedDocs
                && type.AllMembers.TryGetValue(memberId, out var member)) // non-cref inheritdoc found
            {
                inheritDocMembers.Add(member);
            }

            if (memberKindId == CodeElementId.Method && memberName == ConstructorData.DefaultName) // the member is a constructor
            {
                constructorDocHandler.AddDocumentation(type, memberId, docCommentNode);
            }
            if (memberKindId == CodeElementId.Method && OperatorData.MethodNames.Contains(memberName)) // an operator
            {
                operatorDocHandler.AddDocumentation(type, memberId, docCommentNode);
            }
            if (memberKindId == CodeElementId.Property && type.Indexers.ContainsKey(memberId)) // an indexer
            {
                indexerDocHandler.AddDocumentation(type, memberId, docCommentNode);
            }
            else if (memberDocHandlers.TryGetValue(memberKindId, out var handler))
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

    /// <summary>
    /// Add the inherited doc comment to the corresponding type or type member.
    /// Does nothing if <paramref name="docComment"/> is <c>null</c> or empty.
    /// </summary>
    /// <param name="docComment">Doc comment to add.</param>
    private void AddInheritedDocComment(XElement? docComment)
    {
        if (docComment?.Nodes().Any() ?? false)
        {
            AddDocComment(docComment);
        }
    }

    /// <summary>
    /// Checks all types and members for inheritdoc comments with 'cref' attribute and resolves them (if possible).
    /// </summary>
    private void ResolveCrefInheritDocs()
    {
        foreach (var (_, type) in typeRegistry.AllTypes)
        {
            // check the type doc comment
            if (type.RawDocComment?.GetInheritDocs(InheritDocKind.Cref).Any() ?? false)
            {
                crefInheritDocHandler.Resolve(type.RawDocComment);
                AddInheritedDocComment(type.RawDocComment);
            }

            // check all member doc comments
            foreach (var (_, member) in type.AllMembers)
            {
                if (member.RawDocComment?.GetInheritDocs(InheritDocKind.Cref).Any() ?? false)
                {
                    crefInheritDocHandler.Resolve(member.RawDocComment);
                    AddInheritedDocComment(member.RawDocComment);
                }
            }
        }
    }
}
