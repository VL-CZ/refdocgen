using RefDocGen.CodeElements.Abstract;
using RefDocGen.CodeElements.Concrete;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Concrete.Types.Delegate;
using RefDocGen.CodeElements.Concrete.Types.Enum;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.TemplateGenerators.Tools;

internal abstract class DefaultCommentParser
{
    private XElement GetEmptyDescendant(XElement element)
    {
        return element.Descendants().Single(n => n.IsEmpty);
    }

    private void Annotate(XElement element)
    {
        var attribute = new XAttribute("refdocgen-generated", true);

        element.Add(attribute);

        foreach (var e in element.Elements())
        {
            Annotate(e);
        }
    }

    protected readonly ITypeRegistry typeRegistry;

    protected virtual XElement ParagraphElement => new("p");

    protected virtual XElement BulletListElement => new("ul");

    protected virtual XElement NumberListElement => new("ol");

    protected virtual XElement ListItemElement => new("li");

    protected virtual XElement InlineCodeElement => new("code");

    protected virtual XElement CodeBlockElement => new("pre",
                                                        new XElement("code")
                                                    );

    protected virtual XElement ExampleElement => new("div");

    protected virtual XElement ParamRefElement => new("xxx");

    protected virtual XElement TypeParamRefElement => new("code");

    protected virtual XElement SeeCrefElement => new("a");

    protected virtual XElement SeeHrefElement => new("a");

    protected virtual XElement SeeLangwordElement => new("a");

    protected virtual XElement SeeCrefNotFoundElement => new("code");

    private string[] toRemove = ["summary", "remarks", "returns", "exception", "value"];

    protected DefaultCommentParser(ITypeRegistry typeRegistry)
    {
        this.typeRegistry = typeRegistry;
    }

    protected DefaultCommentParser() // TODO: code smell
    {
        typeRegistry = new TypeRegistry(
                new Dictionary<string, ObjectTypeData>(),
                new Dictionary<string, EnumTypeData>(),
                new Dictionary<string, DelegateTypeData>()
            );
    }

    internal string Parse(XElement docComment)
    {
        ParseElement(docComment);
        return docComment.ToString();
    }

    private void ParseElement(XElement element)
    {
        if (element.Attribute("refdocgen-generated") is null)
        {

            if (element.Name == "para")
            {
                HandleParagraphElement(element);
            }
            else if (element.Name == "list")
            {
                HandleListElement(element);
            }
            else if (element.Name == "item")
            {
                HandleListItemElement(element);
            }
            else if (element.Name == "c")
            {
                HandleInlineCodeElement(element);
            }
            else if (element.Name == "code")
            {
                HandleCodeBlockElement(element);
            }
            else if (element.Name == "example")
            {
                HandleExampleElement(element);
            }
            else if (element.Name == "see")
            {
                HandleSeeElement(element);
            }
            else if (element.Name == "seealso")
            {
                HandleSeeElement(element); // TODO: add
            }
            else if (element.Name == "paramref")
            {
                HandleParamRefElement(element);
            }
            else if (element.Name == "typeparamref")
            {
                HandleTypeParamRefElement(element);
            }
            else if (toRemove.Contains(element.Name.LocalName))
            {
                element.Name = "div";
            }
        }

        foreach (var child in element.Elements())
        {
            ParseElement(child);
        }
    }

    protected virtual void HandleListElement(XElement element)
    {
        string listType = element.Attribute("type")?.Value ?? "bullet";

        var types = new Dictionary<string, XElement>()
        {
            ["bullet"] = BulletListElement,
            ["number"] = NumberListElement,
            ["table"] = BulletListElement, // TODO: add 
        };

        if (types.TryGetValue(listType, out var newElement))
        {
            element.ReplaceAttributes(newElement.Attributes());
            element.Name = newElement.Name;
        }
    }

    protected virtual void HandleListItemElement(XElement element)
    {
        element.Name = ListItemElement.Name;
    }

    protected virtual void HandleParagraphElement(XElement element)
    {
        element.Name = ParagraphElement.Name;
    }

    protected virtual void HandleInlineCodeElement(XElement element)
    {
        element.Name = InlineCodeElement.Name;
    }

    protected virtual void HandleCodeBlockElement(XElement element)
    {
        if (element.Attribute("skip") is not null)
        {
            return;
        }

        var newElement = CodeBlockElement;

        Annotate(newElement);
        var deepestChild = GetEmptyDescendant(newElement);
        var elementNodes = element.Nodes().ToList();
        deepestChild.Add(elementNodes);

        element.Name = newElement.Name;
        element.ReplaceAttributes(newElement.Attributes());
        element.ReplaceNodes(newElement.Nodes());
    }

    protected virtual void HandleExampleElement(XElement element)
    {
        element.Name = ExampleElement.Name;
    }

    protected virtual void HandleSeeElement(XElement element)
    {
        if (element.Attribute(XmlDocIdentifiers.Href) is XAttribute hrefAttr)
        {
            HandleAnySeeHrefElement(element, hrefAttr.Value);
        }
        else if (element.Attribute(XmlDocIdentifiers.Cref) is XAttribute crefAttr)
        {
            HandleAnySeeCrefElement(element, crefAttr.Value);
        }
        else if (element.Attribute(XmlDocIdentifiers.Langword) is XAttribute langwordAttr)
        {
            HandleAnySeeLangwordElement(element, langwordAttr.Value);
        }
    }

    protected virtual void HandleAnySeeHrefElement(XElement element, string hrefValue)
    {
        element.Name = SeeHrefElement.Name;

        if (!element.Nodes().Any())
        {
            element.Add(hrefValue);
        }
    }

    protected virtual void HandleAnySeeLangwordElement(XElement element, string langword)
    {
        element.RemoveAttributes();
        element.Name = SeeLangwordElement.Name;

        element.Add(new XText(langword));
    }

    protected virtual void HandleAnySeeCrefElement(XElement element, string crefValue)
    {
        element.RemoveAttributes();

        string[] splitMemberName = crefValue.Split(':');
        (string objectIdentifier, string fullObjectName) = (splitMemberName[0], splitMemberName[1]);

        string typeId;
        string? memberId = null;

        if (objectIdentifier == MemberTypeId.Type) // type
        {
            typeId = fullObjectName;
        }
        else if (objectIdentifier == "!") // reference not found
        {
            HandleNotFoundCrefElement(element, fullObjectName);
            return;
        }
        else // member
        {
            (typeId, string memberName, string paramsString) = MemberSignatureParser.Parse(fullObjectName);
            memberId = memberName + paramsString;
        }

        // type found
        if (typeRegistry.GetDeclaredType(typeId) is not null)
        {
            element.Name = SeeCrefElement.Name;

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

    protected virtual void HandleNotFoundCrefElement(XElement element, string crefValue)
    {
        element.Name = SeeCrefNotFoundElement.Name;

        // no child nodes -> add cref 
        if (!element.Nodes().Any())
        {
            element.Add(new XText(crefValue));
        }
    }

    protected virtual void HandleParamRefElement(XElement element)
    {
        HandleAnyParamRefElement(element, ParamRefElement);
    }

    protected virtual void HandleTypeParamRefElement(XElement element)
    {
        HandleAnyParamRefElement(element, TypeParamRefElement);
    }

    protected virtual void HandleAnyParamRefElement(XElement element, XElement htmlElement)
    {
        string? name = element.Attribute(XmlDocIdentifiers.Name)?.Value;

        if (name is null)
        {
            return;
        }

        element.ReplaceAttributes(htmlElement.Attributes());
        element.Name = htmlElement.Name;

        element.Add(new XText(name));
    }
}

internal class HtmlCommentParser : DefaultCommentParser
{
    public HtmlCommentParser()
    {
    }

    public HtmlCommentParser(ITypeRegistry typeRegistry) : base(typeRegistry)
    {
    }

    protected override XElement ParamRefElement =>
        new(
            "code",
            new XAttribute("class", "text-light bg-dark")); // TODO: just for demo, remove afterwards
}
