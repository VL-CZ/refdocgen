using RefDocGen.DocExtraction.Handlers;
using RefDocGen.DocExtraction.Handlers.Abstract;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.MemberData.Concrete;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction;

/// <summary>
/// Class responsible for extracting the XML documentation comments and adding them to the type data
/// </summary>
internal class DocCommentExtractor
{
    /// <summary>
    /// Array of type data to which the documentation comments will be added.
    /// </summary>
    private readonly TypeData[] typeData;

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
    /// Initializes a new instance of the <see cref="DocCommentExtractor"/> class.
    /// </summary>
    /// <param name="docXmlPath">Path to the XML documentation file.</param>
    /// <param name="typeData">Array of class data to which the documentation comments will be added.</param>
    internal DocCommentExtractor(string docXmlPath, TypeData[] typeData)
    {
        this.typeData = typeData;

        // load the document
        xmlDocument = XDocument.Load(docXmlPath);
    }

    /// <summary>
    /// Extract the XML documentation comments and add them to the provided types and members.
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
    /// Add doc comment to the corresponding type or a type member.
    /// </summary>
    /// <param name="docCommentNode">Doc comment XML node.</param>
    private void AddDocComment(XElement docCommentNode)
    {
        // identifiers, for further info, see https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/#id-strings
        const string typeIdentifier = "T";
        const string namespaceIdentifier = "N";

        // try to get type / member name
        if (docCommentNode.TryGetNameAttribute(out var memberNameAttr))
        {
            string[] splitMemberName = memberNameAttr.Value.Split(':');
            (string objectIdentifier, string fullObjectName) = (splitMemberName[0], splitMemberName[1]);

            if (objectIdentifier == typeIdentifier) // type
            {
                AddTypeDocComment(fullObjectName, docCommentNode);
            }
            else if (typeIdentifier == namespaceIdentifier) // namespace
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
    /// <param name="fullTypeName">Fully qualified type name.</param>
    /// <param name="docCommentNode">Type doc comment XML node.</param>
    private void AddTypeDocComment(string fullTypeName, XElement docCommentNode)
    {
        if (docCommentNode.TryGetSummaryElement(out var summaryNode))
        {
            var templateNode = GetClassByItsName(fullTypeName);
            templateNode.DocComment = summaryNode;
        }
    }

    /// <summary>
    /// Add doc comment to the given type member.
    /// </summary>
    /// <param name="memberTypeId">Identifier of the member type.</param>
    /// <param name="fullMemberName">Fully qualified name of the member.</param>
    /// <param name="docCommentNode">Member doc comment XML node.</param>
    private void AddMemberDocComment(string memberTypeId, string fullMemberName, XElement docCommentNode)
    {
        (string typeName, string memberName, string paramsString) = MemberSignatureParser.Parse(fullMemberName);
        var type = GetClassByItsName(typeName);
        string memberId = memberName + paramsString;

        if (memberCommentHandlers.TryGetValue(memberTypeId, out var parser))
        {
            if (memberTypeId == MemberTypeId.Method && memberName == ConstructorData.DefaultName) // The method is a constructor.
            {
                constructorCommentHandler.AddDocumentation(type, memberId, docCommentNode);
            }
            else
            {
                parser.AddDocumentation(type, memberId, docCommentNode);
            }
        }
        else
        {
            // TODO: log unknown member
        }
    }

    /// <summary>
    /// Get <see cref="TypeData"/> object by its name.
    /// </summary>
    /// <param name="className">Name of the class to find.</param>
    /// <returns>Found <see cref="TypeData"/> object.</returns>
    private TypeData GetClassByItsName(string className)
    {
        return typeData.Single(m => m.Name == className);
    }
}
