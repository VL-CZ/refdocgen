using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements;
using RefDocGen.DocExtraction.Handlers.Members;
using RefDocGen.DocExtraction.Handlers.Members.Enum;
using RefDocGen.DocExtraction.Handlers.Types;

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
    /// Dictionary of selected member documentation handlers, identified by <see cref="MemberTypeId"/> identifiers.
    /// </summary>
    private readonly Dictionary<string, IMemberDocumentationHandler> memberDocHandlers = new()
    {
        [MemberTypeId.Field] = new FieldDocumentationHandler(),
        [MemberTypeId.Property] = new PropertyDocumentationHandler(),
        [MemberTypeId.Method] = new MethodDocumentationHandler(),
    };

    /// <summary>
    /// Handler for the constructor doc comments.
    /// </summary>
    private readonly ConstructorDocumentationHandler constructorDocHandler = new();

    /// <summary>
    /// Handler for the enum member doc comments.
    /// </summary>
    private readonly EnumMemberDocumentationHandler enumMemberDocHandler = new();

    /// <summary>
    /// Handler for the object type doc comments.
    /// </summary>
    private readonly ObjectTypeDocumentationHandler objectTypeDocHandler = new();

    /// <summary>
    /// Handler for the enum type doc comments.
    /// </summary>
    private readonly EnumTypeDocumentationHandler enumTypeDocHandler = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="DocCommentExtractor"/> class.
    /// </summary>
    /// <param name="docXmlPath">Path to the XML documentation file.</param>
    /// <param name="typeRegistry">Registry of the declared types, to which the documentation comments will be added.</param>
    internal DocCommentExtractor(string docXmlPath, TypeRegistry typeRegistry)
    {
        this.typeRegistry = typeRegistry;

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
        if (typeRegistry.ObjectTypes.TryGetValue(typeId, out var type)) // the type is an 'object' type
        {
            objectTypeDocHandler.AddDocumentation(type, docCommentNode);
        }
        else if (typeRegistry.Enums.TryGetValue(typeId, out var e)) // the type is an enum
        {
            enumTypeDocHandler.AddDocumentation(e, docCommentNode);
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
            if (memberDocHandlers.TryGetValue(memberTypeId, out var handler))
            {
                if (memberTypeId == MemberTypeId.Method && memberName == ConstructorData.DefaultName) // The method is a constructor.
                {
                    constructorDocHandler.AddDocumentation(type, memberId, docCommentNode);
                }
                else
                {
                    handler.AddDocumentation(type, memberId, docCommentNode);
                }
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
