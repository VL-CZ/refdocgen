using RefDocGen.TemplateModels;
using System.Xml;

namespace RefDocGen;

public class DocCommentExtractor
{
    private readonly string docXmlPath;
    private readonly ClassTemplateModel[] models;

    public DocCommentExtractor(string docXmlPath, ClassTemplateModel[] models)
    {
        this.docXmlPath = docXmlPath;
        this.models = models;
    }

    public ClassTemplateModel[] GetTemplateModels()
    {
        AddComments();
        return models;
    }

    private void AddComments()
    {
        var xmlDoc = GetDocCommentsFile();
        var memberNodes = xmlDoc.SelectNodes("//member"); // TODO: check formal file specification
        foreach (XmlNode memberNode in memberNodes)
        {
            var memberAttr = memberNode.Attributes?["name"];
            var summaryNode = memberNode.SelectSingleNode("summary");

            if (summaryNode is not null && memberAttr is not null)
            {
                string summaryText = summaryNode.InnerText.Trim();
                string memberName = memberAttr.Value;

                string[] sp = memberName.Split(':');

                if (sp[0] == "T") // TODO: code quality
                {
                    string className = sp[1];
                    var templateNode = models.Where(m => m.Name == className).First();

                    templateNode.DocComment = summaryText;
                }
                else if (sp[0] == "F")
                {
                    string fullName = sp[1];
                    string[] nameParts = fullName.Split('.');

                    string fieldName = nameParts[^1];
                    string className = string.Join('.', nameParts, 0, nameParts.Length - 1);

                    var fieldNode = models.First(m => m.Name == className).Fields.First(f => f.Name == fieldName);

                    fieldNode.DocComment = summaryText;
                }
            }
        }
    }

    private XmlDocument GetDocCommentsFile()
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.Load(docXmlPath);
        return xmlDoc;
    }
}
