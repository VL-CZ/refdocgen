using RefDocGen.MemberData;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction;

/// <summary>
/// Class responsible for extracting the XML documentation comments and adding them to the type data
/// </summary>
internal class DocCommentExtractor
{
    /// <summary>
    /// Path to the XML documentation file.
    /// </summary>
    private readonly string docXmlPath;

    /// <summary>
    /// Array of class data to which the documentation comments will be added.
    /// </summary>
    private readonly ClassData[] classData;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocCommentExtractor"/> class.
    /// </summary>
    /// <param name="docXmlPath">Path to the XML documentation file.</param>
    /// <param name="classData">Array of class data to which the documentation comments will be added.</param>
    internal DocCommentExtractor(string docXmlPath, ClassData[] classData)
    {
        this.docXmlPath = docXmlPath;
        this.classData = classData;
    }

    /// <summary>
    /// Extrac the XMl documentation comments and add them to the provided type data
    /// </summary>
    internal void AddComments()
    {
        var xmlDoc = GetDocCommentsFile();
        var memberNodes = xmlDoc.Descendants("member");

        foreach (var memberNode in memberNodes)
        {
            var memberAttr = memberNode.Attribute("name");
            var summaryNode = memberNode.Element("summary");

            if (summaryNode is not null && memberAttr is not null)
            {
                string summaryText = summaryNode.Value.Trim();
                string memberName = memberAttr.Value;

                string[] sp = memberName.Split(':');

                if (sp[0] == "T") // Class
                {
                    string className = sp[1];
                    var templateNode = classData.First(m => m.Name == className);

                    int index = Array.IndexOf(classData, templateNode);

                    classData[index] = templateNode with { DocComment = summaryNode };
                }
                else if (sp[0] == "F") // Field
                {
                    string fullName = sp[1];
                    string[] nameParts = fullName.Split('.');

                    string fieldName = nameParts[^1];
                    string className = string.Join('.', nameParts, 0, nameParts.Length - 1);

                    var type = classData.First(m => m.Name == className);
                    var fieldNode = type.Fields.First(f => f.Name == fieldName);

                    int index = Array.IndexOf(type.Fields, fieldNode);

                    type.Fields[index] = fieldNode with { DocComment = summaryNode };
                }
                else if (sp[0] == "P") // Property
                {
                    string fullName = sp[1];
                    string[] nameParts = fullName.Split('.');

                    string propertyName = nameParts[^1];
                    string className = string.Join('.', nameParts, 0, nameParts.Length - 1);

                    var type = classData.First(m => m.Name == className);
                    var propertyNode = type.Properties.First(p => p.Name == propertyName);

                    int index = Array.IndexOf(type.Properties, propertyNode);

                    type.Properties[index] = propertyNode with { DocComment = summaryNode };
                }
                else if (sp[0] == "M") // Method
                {
                    string fullName = sp[1];
                    string[] nameParts = fullName.Split('(')[0].Split('.');

                    string fullMethodName = nameParts[^1];
                    string className = string.Join('.', nameParts, 0, nameParts.Length - 1);

                    var type = classData.First(m => m.Name == className);
                    var methodNode = type.Methods.First(p => p.Name == fullMethodName);

                    int index = Array.IndexOf(type.Methods, methodNode);

                    type.Methods[index] = methodNode with { DocComment = summaryNode };
                }
            }
        }
    }

    /// <summary>
    /// Get the XML documentation file represented as a <see cref="XDocument"/>.
    /// </summary>
    /// <returns>The XML documentation file represented as a <see cref="XDocument"/>.</returns>
    private XDocument GetDocCommentsFile()
    {
        return XDocument.Load(docXmlPath);
    }
}
