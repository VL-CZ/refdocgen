using RefDocGen.MemberData;
using RefDocGen.MemberData.Interfaces;
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

    private readonly XDocument xmlDocument;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocCommentExtractor"/> class.
    /// </summary>
    /// <param name="docXmlPath">Path to the XML documentation file.</param>
    /// <param name="classData">Array of class data to which the documentation comments will be added.</param>
    internal DocCommentExtractor(string docXmlPath, ClassData[] classData)
    {
        this.docXmlPath = docXmlPath;
        this.classData = classData;

        // load the document
        xmlDocument = XDocument.Load(docXmlPath);
    }

    /// <summary>
    /// Extrac the XMl documentation comments and add them to the provided type data
    /// </summary>
    internal void AddComments()
    {
        var memberNodes = xmlDocument.Descendants("member");

        foreach (var memberNode in memberNodes)
        {
            var memberNameAttr = memberNode.Attribute("name");
            // var summaryNode = memberNode.Element("summary");

            if (memberNameAttr is not null)
            {
                //string summaryText = summaryNode.Value.Trim();
                string[] splitMemberName = memberNameAttr.Value.Split(':');

                (string memberIdentifier, string fullMemberName) = (splitMemberName[0], splitMemberName[1]);

                if (memberIdentifier == "T") // Type
                {
                    var templateNode = GetClassByItsName(fullMemberName);
                    int index = Array.IndexOf(classData, templateNode);
                    classData[index] = templateNode with { DocComment = memberNode };
                }
                else
                {
                    (string className, string memberName) = GetClassAndMemberName(fullMemberName);
                    var type = GetClassByItsName(className);

                    switch (memberIdentifier)
                    {
                        case "F":
                            AddFieldComment(type, memberName, memberNode);
                            break;
                        case "P":
                            AddPropertyComment(type, memberName, memberNode);
                            break;
                        case "M":
                            AddMethodComment(type, memberName, memberNode);
                            break;
                        default:
                            throw new ArgumentException($"Unknown member identifier: {memberIdentifier}");
                    }
                }
            }
        }
    }

    private void AddFieldComment(ClassData type, string memberName, XElement commentNode)
    {
        int index = GetClassMemberIndex(type.Fields, memberName);
        type.Fields[index] = type.Fields[index] with { DocComment = commentNode };
    }

    private void AddPropertyComment(ClassData type, string memberName, XElement commentNode)
    {
        int index = GetClassMemberIndex(type.Properties, memberName);
        type.Properties[index] = type.Properties[index] with { DocComment = commentNode };
    }

    private void AddMethodComment(ClassData type, string memberName, XElement commentNode)
    {
        int index = GetClassMemberIndex(type.Methods, memberName);
        var method = type.Methods[index];
        type.Methods[index] = method with { DocComment = commentNode };

        var paramElements = commentNode.Descendants("param");
        foreach (var paramElement in paramElements)
        {
            var nameAttr = paramElement.Attribute("name");
            if (nameAttr is not null)
            {
                string paramName = nameAttr.Value;
                var member = method.Parameters.Single(p => p.Name == paramName);
                int paramIndex = Array.IndexOf(method.Parameters, member);

                method.Parameters[index] = member with { DocComment = paramElement };
            }
        }
    }

    private (string className, string memberName) GetClassAndMemberName(string fullMemberName)
    {
        string memberNameWithoutParameters = fullMemberName.Split('(')[0]; // this is done because of methods
        string[] nameParts = memberNameWithoutParameters.Split('.');
        string memberName = nameParts[^1];
        string className = string.Join('.', nameParts, 0, nameParts.Length - 1);

        return (className, memberName);
    }

    private ClassData GetClassByItsName(string className)
    {
        return classData.Single(m => m.Name == className);
    }

    private int GetClassMemberIndex(IMemberData[] memberCollection, string memberName)
    {
        var member = memberCollection.Single(m => m.Name == memberName);
        return Array.IndexOf(memberCollection, member);
    }
}
