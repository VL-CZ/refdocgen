using RefDocGen.DocExtraction.Handlers.Abstract;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.DocExtraction.Handlers.Concrete;
using RefDocGen.DocExtraction.Handlers.Concrete.Enum;
using RefDocGen.CodeElements;
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
    /// Dictionary of selected member comment handlers, identified by <see cref="MemberTypeId"/> identifiers.
    /// </summary>
    private readonly Dictionary<string, IMemberCommentHandler> memberCommentHandlers = new()
    {
        [MemberTypeId.Field] = new FieldCommentHandler(),
        [MemberTypeId.Property] = new PropertyCommentHandler(),
        [MemberTypeId.Method] = new MethodCommentHandler(),
    };

    /// <summary>
    /// Handler for constructor doc comments.
    /// </summary>
    private readonly ConstructorCommentHandler constructorCommentHandler = new();

    /// <summary>
    /// Handler for enum member doc comments.
    /// </summary>
    private readonly EnumMemberCommentHandler enumMemberCommentHandler = new();

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
        if (docCommentNode.TryGetSummaryElement(out var summaryNode))
        {
            if (typeRegistry.ObjectTypes.TryGetValue(typeId, out var type)) // the type is a value, reference or interface type
            {
                type.DocComment = summaryNode;

                var paramElements = docCommentNode.Descendants(XmlDocIdentifiers.TypeParam);

                foreach (var paramDocComment in paramElements) // add type param doc comments
                {
                    AddTypeParamComment(type, paramDocComment);
                }
            }
            else if (typeRegistry.Enums.TryGetValue(typeId, out var e)) // the type is an enum
            {
                e.DocComment = summaryNode;
            }
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
            if (memberCommentHandlers.TryGetValue(memberTypeId, out var handler))
            {
                if (memberTypeId == MemberTypeId.Method && memberName == ConstructorData.DefaultName) // The method is a constructor.
                {
                    constructorCommentHandler.AddDocumentation(type, memberId, docCommentNode);
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
            enumMemberCommentHandler.AddDocumentation(e, memberId, docCommentNode);
        }
    }

    /// <summary>
    /// Add the doc comment to the corresponding type parameter.
    /// </summary>
    /// <param name="type">The type containing the parameter.</param>
    /// <param name="docComment">XML node of the doc comment for the given type parameter.</param>
    private void AddTypeParamComment(ObjectTypeData type, XElement docComment)
    {
        if (docComment.TryGetNameAttribute(out var nameAttr))
        {
            string typeParamName = nameAttr.Value;

            if (type.TypeParameterDeclarations.TryGetValue(typeParamName, out var typeParam))
            {
                typeParam.DocComment = docComment;
            }
        }
    }
}
