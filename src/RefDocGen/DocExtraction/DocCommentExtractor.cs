using RefDocGen.DocExtraction.Handlers;
using RefDocGen.DocExtraction.Handlers.Abstract;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.MemberData;
using RefDocGen.MemberData.Abstract;
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
    private readonly ClassData[] typeData;

    /// <summary>
    /// XML document containing the documentation comments
    /// </summary>
    private readonly XDocument xmlDocument;

    /// <summary>
    /// Dictionary of member comment handlers, identified by the member type identifiers
    /// </summary>
    private readonly Dictionary<string, MemberCommentHandler> memberCommentHandlers;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocCommentExtractor"/> class.
    /// </summary>
    /// <param name="docXmlPath">Path to the XML documentation file.</param>
    /// <param name="typeData">Array of class data to which the documentation comments will be added.</param>
    internal DocCommentExtractor(string docXmlPath, ClassData[] typeData)
    {
        this.typeData = typeData;

        memberCommentHandlers = new Dictionary<string, MemberCommentHandler>
        {
            ["F"] = new FieldCommentHandler(),
            ["P"] = new PropertyCommentHandler(),
            ["M"] = new MethodCommentHandler(),
            ["C"] = new ConstructorCommentHandler()
        };

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
        // try to get type / member name
        if (docCommentNode.TryGetNameAttribute(out var memberNameAttr))
        {
            string[] splitMemberName = memberNameAttr.Value.Split(':');
            (string memberIdentifier, string fullMemberName) = (splitMemberName[0], splitMemberName[1]);

            if (memberIdentifier == "T") // Type
            {
                AddTypeDocComment(fullMemberName, docCommentNode);
            }
            else // Type member
            {
                AddMemberDocComment(memberIdentifier, fullMemberName, docCommentNode);
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
            int index = Array.IndexOf(typeData, templateNode);
            typeData[index] = templateNode with { DocComment = summaryNode };
        }
    }

    /// <summary>
    /// Add doc comment to the given type member.
    /// </summary>
    /// <param name="memberTypeIdentitifer">Identifier of the member type.</param>
    /// <param name="fullMemberName">Fully qualified name of the member.</param>
    /// <param name="docCommentNode">Member doc comment XML node.</param>
    private void AddMemberDocComment(string memberTypeIdentitifer, string fullMemberName, XElement docCommentNode)
    {
        (string typeName, string memberName) = MemberNameExtractor.GetTypeAndMemberName(fullMemberName);
        var type = GetClassByItsName(typeName);

        if (memberCommentHandlers.TryGetValue(memberTypeIdentitifer, out var parser))
        {
            if (memberTypeIdentitifer == "M" && memberName.StartsWith("#ctor", StringComparison.InvariantCulture)) // TODO: add support for constructors
            {
                memberCommentHandlers["C"].AddDocumentation(type, memberName, docCommentNode);
            }
            else
            {
                parser.AddDocumentation(type, memberName, docCommentNode);
            }
        }
        else
        {
            // TODO: log unknown member
        }
    }

    /// <summary>
    /// Get <see cref="ClassData"/> object by its name.
    /// </summary>
    /// <param name="className">Name of the class to find.</param>
    /// <returns>Found <see cref="ClassData"/> object.</returns>
    private ClassData GetClassByItsName(string className)
    {
        return typeData.Single(m => m.Name == className);
    }

    internal static string GetMethodSignatureForXmlDoc(InvokableMemberData memberData)
    {
        if (memberData.Parameters.Length == 0)
        {
            return memberData.Name;
        }
        else
        {
            // Get the parameters in the format: System.String, System.Int32, etc.
            string parameterList = string.Join(",",
                    memberData.Parameters.Select(
                        p => p.ParameterInfo.ParameterType.FullName?.Replace('&', '@')
                    )
                );

            return memberData.Name + "(" + parameterList + ")";
        }
    }
}
