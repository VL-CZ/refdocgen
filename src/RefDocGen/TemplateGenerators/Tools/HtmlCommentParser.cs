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
    private XElement GetEmptyDescendantOrSelf(XElement element)
    {
        return element.IsEmpty
            ? element
            : element.Descendants().Single(n => n.IsEmpty);
    }


    private XElement MarkAsGenerated(XElement element)
    {
        var attribute = new XAttribute("refdocgen-generated", true);

        element.Add(attribute);

        foreach (var e in element.Elements())
        {
            e.ReplaceWith(MarkAsGenerated(e));
        }
        return element;
    }

    private XElement Transform(XElement from, XElement template)
    {
        var newElement = MarkAsGenerated(template);
        var deepestChild = GetEmptyDescendantOrSelf(newElement);
        deepestChild.Add(from.Nodes());

        return newElement;
    }

    private XElement Transform(XElement from, XElement template, string attribute)
    {
        var newElement = MarkAsGenerated(template);
        var deepestChild = GetEmptyDescendantOrSelf(newElement);

        if (from.Attribute(attribute) is XAttribute attr && !from.Nodes().Any())
        {
            deepestChild.Add(attr.Value);
        }

        return newElement;
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

    private readonly string[] toRemove = ["summary", "remarks", "returns", "exception", "value"];

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
        XElement? transformedElement = null;

        if (element.Descendants().Where(d => d.Name == "list").Any())
        {
            int x = 0;
        }

        if (element.Attribute("refdocgen-generated") is null)
        {
            if (element.Name == "para")
            {
                transformedElement = HandleParagraphElement(element);
            }
            else if (element.Name == "list")
            {
                transformedElement = HandleListElement(element);
            }
            else if (element.Name == "item")
            {
                transformedElement = HandleListItemElement(element);
            }
            else if (element.Name == "c")
            {
                transformedElement = HandleInlineCodeElement(element);
            }
            else if (element.Name == "code")
            {
                transformedElement = HandleCodeBlockElement(element);
            }
            else if (element.Name == "example")
            {
                transformedElement = HandleExampleElement(element);
            }
            else if (element.Name == "see")
            {
                transformedElement = HandleSeeElement(element);
            }
            else if (element.Name == "seealso")
            {
                transformedElement = HandleSeeElement(element); // TODO: add
            }
            else if (element.Name == "paramref")
            {
                transformedElement = HandleParamRefElement(element);
            }
            else if (element.Name == "typeparamref")
            {
                transformedElement = HandleTypeParamRefElement(element);
            }
            else if (toRemove.Contains(element.Name.LocalName))
            {
                element.Name = "div";
            }
        }

        if (transformedElement is not null)
        {
            element.ReplaceWith(transformedElement);
        }

        transformedElement ??= element;

        foreach (var child in transformedElement.Elements().ToList())
        {
            ParseElement(child);
        }
    }

    protected virtual XElement HandleListElement(XElement element)
    {
        string listType = element.Attribute("type")?.Value ?? "bullet";

        var types = new Dictionary<string, XElement>()
        {
            ["bullet"] = BulletListElement,
            ["number"] = NumberListElement,
            ["table"] = BulletListElement, // TODO: add 
        };

        return types.TryGetValue(listType, out var newElement)
            ? Transform(element, newElement)
            : element;
    }

    protected virtual XElement HandleListItemElement(XElement element)
    {
        return Transform(element, ListItemElement);
    }

    protected virtual XElement HandleParagraphElement(XElement element)
    {
        return Transform(element, ParagraphElement);
    }

    protected virtual XElement HandleInlineCodeElement(XElement element)
    {
        return Transform(element, InlineCodeElement);
    }

    protected virtual XElement HandleCodeBlockElement(XElement element)
    {
        return Transform(element, CodeBlockElement);
    }

    protected virtual XElement HandleExampleElement(XElement element)
    {
        return Transform(element, ExampleElement);
    }

    protected virtual XElement HandleSeeElement(XElement element)
    {
        if (element.Attribute(XmlDocIdentifiers.Href) is XAttribute hrefAttr)
        {
            return HandleAnySeeHrefElement(element, hrefAttr.Value);
        }
        else if (element.Attribute(XmlDocIdentifiers.Cref) is XAttribute crefAttr)
        {
            return HandleAnySeeCrefElement(element, crefAttr.Value);
        }
        else if (element.Attribute(XmlDocIdentifiers.Langword) is XAttribute langwordAttr)
        {
            return HandleAnySeeLangwordElement(element, langwordAttr.Value);
        }
        else
        {
            return element;
        }
    }

    protected virtual XElement HandleAnySeeHrefElement(XElement element, string hrefValue)
    {
        var newElement = MarkAsGenerated(SeeHrefElement);
        var deepestChild = GetEmptyDescendantOrSelf(newElement);

        if (element.Nodes().Any())
        {
            deepestChild.Add(element.Nodes());
        }
        else
        {
            deepestChild.Add(hrefValue);
        }

        deepestChild.Add(
            new XAttribute(XmlDocIdentifiers.Href, hrefValue)
            );

        return newElement;
    }

    protected virtual XElement HandleAnySeeLangwordElement(XElement element, string langword)
    {
        return Transform(element, SeeLangwordElement, langword);
    }

    protected virtual XElement HandleAnySeeCrefElement(XElement element, string crefValue)
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
            return HandleNotFoundCrefElement(element, fullObjectName);
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

        return element;
    }

    protected virtual XElement HandleNotFoundCrefElement(XElement element, string crefValue)
    {
        return Transform(element, SeeCrefNotFoundElement, XmlDocIdentifiers.Cref);
    }

    protected virtual XElement HandleParamRefElement(XElement element)
    {
        return HandleAnyParamRefElement(element, ParamRefElement);
    }

    protected virtual XElement HandleTypeParamRefElement(XElement element)
    {
        return HandleAnyParamRefElement(element, TypeParamRefElement);
    }

    protected virtual XElement HandleAnyParamRefElement(XElement element, XElement htmlElement)
    {
        return Transform(element, htmlElement, XmlDocIdentifiers.Name);
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
