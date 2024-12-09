using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.TemplateGenerators.Tools;

internal class HtmlCommentParser
{
    private readonly Dictionary<string, string> tags = new()
    {
        ["summary"] = "div",
        ["para"] = "p",
        ["c"] = "kbd",
        //["example"] = "div",
        //["exception"] = "div",
        ["item"] = "li",
        //["description"] = "p",
        //["term"] = "strong",
        //["remarks"] = "div",
        ["returns"] = "div",
        //["seealso"] = "a",
        ["param"] = "div",
        ["typeparam"] = "div",
        //["value"] = "div",
        //["include"] = "div",
        //["inheritdoc"] = "div",
        //["permission"] = "div",
    };

    internal string Parse(XElement docComment)
    {
        ParseElement(docComment);
        return docComment.ToString();
    }

    private void ParseElement(XElement element)
    {
        if (element.Name == "list")
        {
            HandleListElement(element);
        }
        else if (element.Name == "code")
        {
            HandleCodeElement(element);
        }
        else if (element.Name == "see")
        {
            HandleSeeElement(element);
        }
        else if (tags.TryGetValue(element.Name.ToString(), out string? htmlName))
        {
            element.Name = htmlName;
        }

        foreach (var child in element.Elements())
        {
            ParseElement(child);
        }
    }

    private void HandleListElement(XElement element)
    {
        string listType = element.Attribute("type")?.Value ?? "bullet";

        var types = new Dictionary<string, string>()
        {
            ["bullet"] = "ul",
            ["number"] = "ol",
            ["table"] = "ul", // TODO: add 
        };

        if (types.TryGetValue(listType, out string? newName))
        {
            element.Name = newName;
            element.RemoveAttributes();
        }

    }

    private void HandleCodeElement(XElement element)
    {
        if (element.Attribute("skip") is not null)
        {
            return;
        }

        element.Name = "pre";
        var children = element.Nodes();

        var codeElement = new XElement("code", new XAttribute("skip", true));
        codeElement.Add(children);

        element.RemoveNodes();
        element.Add(codeElement);
    }

    private void HandleSeeElement(XElement element)
    {
        element.Name = "a";

        if (element.Attribute("href") is not null)
        {
            return;
        }
        else if (element.TryGetAttribute("langword", out var attr))
        {
            var codeElement = new XElement("code", new XAttribute("skip", true), new XText(attr.Value));

            element.Add(codeElement);
            element.RemoveAttributes();
        }
        //else if (element.Attribute("cref") is not null) // TODO: handle cref
        //{

        //}
    }
}
