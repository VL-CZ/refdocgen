using System.Xml.Linq;

namespace RefDocGen.TemplateGenerators.Tools;

internal class HtmlCommentParser
{
    private readonly IReadOnlyDictionary<string, string> tags =
        new Dictionary<string, string>
        {
            ["summary"] = "div",
            ["para"] = "p",
            ["c"] = "kbd",
            ["code"] = "kbd",
        };

    internal string Parse(XElement docComment)
    {
        ParseElement(docComment);
        return docComment.ToString();
    }

    private void ParseElement(XElement element)
    {
        if (tags.TryGetValue(element.Name.ToString(), out string? htmlName))
        {
            element.Name = htmlName;
        }

        foreach (var child in element.Elements())
        {
            ParseElement(child);
        }
    }
}
