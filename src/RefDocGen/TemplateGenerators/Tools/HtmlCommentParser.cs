using RefDocGen.CodeElements.Abstract;
using RefDocGen.CodeElements.Concrete;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Concrete.Types.Delegate;
using RefDocGen.CodeElements.Concrete.Types.Enum;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.TemplateGenerators.Tools;

internal class HtmlCommentParser
{
    private readonly ITypeRegistry typeRegistry;

    public HtmlCommentParser(ITypeRegistry typeRegistry)
    {
        this.typeRegistry = typeRegistry;
    }

    public HtmlCommentParser() // TODO: code smell
    {
        typeRegistry = new TypeRegistry(
                new Dictionary<string, ObjectTypeData>(),
                new Dictionary<string, EnumTypeData>(),
                new Dictionary<string, DelegateTypeData>()
            );
    }

    private readonly Dictionary<string, string> tags = new()
    {
        ["summary"] = "div",
        ["remarks"] = "div",

        ["returns"] = "div",
        ["param"] = "div",
        ["value"] = "div",

        ["para"] = "p",
        ["c"] = "code",
        ["item"] = "li",

        ["typeparam"] = "div",

        //["example"] = "div",
        //["description"] = "p",
        //["term"] = "strong",
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
        else if (element.Name == "seealso")
        {
            HandleSeeElement(element);
        }
        else if (element.Name == "paramref")
        {
            HandleParamRefElement(element);
        }
        else if (element.Name == "typeparamref")
        {
            HandleTypeParamRefElement(element);
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
        if (element.Attribute("href") is XAttribute hrefAttr)
        {
            element.Name = "a";

            if (!element.Nodes().Any())
            {
                element.Add(new XText(hrefAttr.Value));
            }

            return;
        }
        else if (element.TryGetAttribute("langword", out var attr))
        {
            element.RemoveAttributes();
            element.Name = "code";

            element.Add(new XText(attr.Value));
        }
        else if (element.TryGetCrefAttribute(out var codeRefAttr))
        {
            element.RemoveAttributes();

            string[] splitMemberName = codeRefAttr.Value.Split(':');
            (string objectIdentifier, string fullObjectName) = (splitMemberName[0], splitMemberName[1]);

            string typeId;
            string? memberId = null;

            if (objectIdentifier == MemberTypeId.Type) // type
            {
                typeId = fullObjectName;
            }
            else if (objectIdentifier == "!") // reference not found
            {
                element.Name = "span";

                // no child nodes -> add cref 
                if (!element.Nodes().Any())
                {
                    element.Add(new XText(fullObjectName));
                }
                return;
            }
            else // member
            {
                (typeId, string memberName, string paramsString) = MemberSignatureParser.Parse(fullObjectName);
                memberId = memberName + paramsString;
            }

            // type found
            if (typeRegistry.TryGetType(typeId, out _))
            {
                element.Name = "a";

                string target = typeId + ".html";

                // TODO: check if member is found

                if (memberId is not null)
                {
                    target += $"#{memberId}";
                }

                element.Add(
                    new XAttribute("href", target)
                );

                // no child nodes -> add cref 
                if (!element.Nodes().Any())
                {
                    element.Add(new XText(target));
                }
            }
            else // type not found
            {
                element.Name = "code";

                if (!element.Nodes().Any())
                {
                    element.Add(new XText(fullObjectName));
                }
            }
        }
    }

    private void HandleParamRefElement(XElement element)
    {
        string? name = element.Attribute("name")?.Value;

        if (name is null)
        {
            return;
        }

        element.Name = "code";
        element.RemoveAttributes();
        element.Add(new XText(name));
    }

    private void HandleTypeParamRefElement(XElement element)
    {
        string? name = element.Attribute("name")?.Value;

        if (name is null)
        {
            return;
        }

        element.Name = "code";
        element.RemoveAttributes();
        element.Add(new XText(name));
    }
}
