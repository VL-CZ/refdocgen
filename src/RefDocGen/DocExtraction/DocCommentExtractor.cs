using RefDocGen.DocExtraction.Parsers;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.MemberData;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction;

/// <summary>
/// Class responsible for extracting the XML documentation comments and adding them to the type data
/// </summary>
internal class DocCommentExtractor
{
    /// <summary>
    /// Array of class data to which the documentation comments will be added.
    /// </summary>
    private readonly ClassData[] classData;

    /// <summary>
    /// XML document with the doc comments
    /// </summary>
    private readonly XDocument xmlDocument;

    private readonly Dictionary<string, MemberCommentParser> commentParsers;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocCommentExtractor"/> class.
    /// </summary>
    /// <param name="docXmlPath">Path to the XML documentation file.</param>
    /// <param name="classData">Array of class data to which the documentation comments will be added.</param>
    internal DocCommentExtractor(string docXmlPath, ClassData[] classData)
    {
        this.classData = classData;

        commentParsers = new Dictionary<string, MemberCommentParser>
        {
            ["F"] = new FieldCommentParser(),
            ["P"] = new PropertyCommentParser(),
            ["M"] = new MethodCommentParser()
        };

        // load the document
        xmlDocument = XDocument.Load(docXmlPath);
    }

    /// <summary>
    /// Extract the XML documentation comments and add them to the provided type data.
    /// </summary>
    internal void AddComments()
    {
        var memberNodes = xmlDocument.Descendants(MagicStrings.Member);

        foreach (var memberNode in memberNodes)
        {
            AddDocComment(memberNode);
        }
    }

    private void AddDocComment(XElement docCommentNode)
    {
        if (docCommentNode.TryGetNameAttribute(out var memberNameAttr))
        {
            //string summaryText = summaryNode.Value.Trim();
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
    }

    private void AddTypeDocComment(string fullTypeName, XElement docCommentNode)
    {
        if (docCommentNode.TryGetSummaryElement(out var summaryNode))
        {
            var templateNode = GetClassByItsName(fullTypeName);
            int index = Array.IndexOf(classData, templateNode);
            classData[index] = templateNode with { DocComment = summaryNode };
        }
    }

    private void AddMemberDocComment(string memberIdentifier, string fullMemberName, XElement docCommentNode)
    {
        (string typeName, string memberName) = MemberNameExtractor.GetTypeAndMemberName(fullMemberName);
        var type = GetClassByItsName(typeName);

        if (commentParsers.TryGetValue(memberIdentifier, out var parser))
        {
            if (memberIdentifier == "M" && memberName.StartsWith("#ctor", StringComparison.InvariantCulture)) // TODO: add support for constructors
            {
                return;
            }
            else
            {
                parser.AddCommentTo(type, memberName, docCommentNode);
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
        return classData.Single(m => m.Name == className);
    }
}
