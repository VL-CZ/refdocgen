using RefDocGen.MemberData;
using System.Xml;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction;

public class DocCommentExtractor
{
    private readonly string docXmlPath;
    private readonly ClassData[] models;

    public DocCommentExtractor(string docXmlPath, ClassData[] models)
    {
        this.docXmlPath = docXmlPath;
        this.models = models;
    }

    public ClassData[] ExtractComments()
    {
        AddComments();
        return models;
    }

    private void AddComments()
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
                    var templateNode = models.First(m => m.Name == className);

                    int index = Array.IndexOf(models, templateNode);

                    models[index] = templateNode with { DocComment = summaryNode };
                }
                else if (sp[0] == "F") // Field
                {
                    string fullName = sp[1];
                    string[] nameParts = fullName.Split('.');

                    string fieldName = nameParts[^1];
                    string className = string.Join('.', nameParts, 0, nameParts.Length - 1);

                    var type = models.First(m => m.Name == className);
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

                    var type = models.First(m => m.Name == className);
                    var propertyNode = type.Properties.First(p => p.Name == propertyName);

                    int index = Array.IndexOf(type.Properties, propertyNode);

                    type.Properties[index] = propertyNode with { DocComment = summaryNode };
                }
            }
        }
    }

    private XDocument GetDocCommentsFile()
    {
        return XDocument.Load(docXmlPath);
    }
}
