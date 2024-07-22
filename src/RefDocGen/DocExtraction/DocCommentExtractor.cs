using RefDocGen.DocExtraction.Tools;
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
    /// Array of class data to which the documentation comments will be added.
    /// </summary>
    private readonly ClassData[] classData;

    /// <summary>
    /// XML document with the doc comments
    /// </summary>
    private readonly XDocument xmlDocument;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocCommentExtractor"/> class.
    /// </summary>
    /// <param name="docXmlPath">Path to the XML documentation file.</param>
    /// <param name="classData">Array of class data to which the documentation comments will be added.</param>
    internal DocCommentExtractor(string docXmlPath, ClassData[] classData)
    {
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
                else // Type member
                {
                    (string typeName, string memberName) = MemberNameExtractor.GetTypeAndMemberName(fullMemberName);
                    var type = GetClassByItsName(typeName);

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

    /// <summary>
    /// Add doc comment to the given field.
    /// </summary>
    /// <param name="type">Type containing the field.</param>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="commentNode">Doc comment for the field.</param>
    private void AddFieldComment(ClassData type, string fieldName, XElement commentNode)
    {
        int index = GetTypeMemberIndex(type.Fields, fieldName);
        type.Fields[index] = type.Fields[index] with { DocComment = commentNode };
    }

    /// <summary>
    /// Add doc comment to the given property.
    /// </summary>
    /// <param name="type">Type containing the propety.</param>
    /// <param name="fieldName">Name of the property.</param>
    /// <param name="commentNode">Doc comment for the property.</param>
    private void AddPropertyComment(ClassData type, string propertyName, XElement commentNode)
    {
        int index = GetTypeMemberIndex(type.Properties, propertyName);
        type.Properties[index] = type.Properties[index] with { DocComment = commentNode };
    }

    /// <summary>
    /// Add doc comment to the given method.
    /// </summary>
    /// <param name="type">Type containing the method.</param>
    /// <param name="fieldName">Name of the method.</param>
    /// <param name="commentNode">Doc comment for the method.</param>
    private void AddMethodComment(ClassData type, string methodNode, XElement commentNode)
    {
        int index = GetTypeMemberIndex(type.Methods, methodNode);
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

    /// <summary>
    /// Get <see cref="ClassData"/> object by its name.
    /// </summary>
    /// <param name="className">Name of the class to find.</param>
    /// <returns>Found <see cref="ClassData"/> object.</returns>
    private ClassData GetClassByItsName(string className)
    {
        return classData.Single(m => m.Name == className);
    }

    /// <summary>
    /// Get index of the given member in the collection.
    /// </summary>
    /// <param name="memberCollection">Collection containing member data.</param>
    /// <param name="memberName">Name of the member to find.</param>
    /// <returns>Index of the given member in the collection.</returns>
    private int GetTypeMemberIndex(IMemberData[] memberCollection, string memberName)
    {
        var member = memberCollection.Single(m => m.Name == memberName);
        return Array.IndexOf(memberCollection, member);
    }
}
