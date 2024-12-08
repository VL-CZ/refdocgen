using System.Xml.Linq;

namespace RefDocGen.TemplateGenerators.Tools;

internal class HtmlCommentParser
{
    internal static XElement Parse(XElement docComment)
    {
        docComment.Name = "div";

        foreach (var item in docComment.Elements())
        {
            if (item.Name == "para")
            {
                item.Name = "p";
            }
        }

        return docComment;
    }
}
