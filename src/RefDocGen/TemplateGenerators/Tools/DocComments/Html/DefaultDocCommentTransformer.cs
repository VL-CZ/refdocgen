using RefDocGen.CodeElements.Abstract;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Concrete;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Concrete.Types.Delegate;
using RefDocGen.CodeElements.Concrete.Types.Enum;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.TemplateGenerators.Tools.TypeName;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.TemplateGenerators.Tools.DocComments.Html;

/// <summary>
/// Class responsible for transforming the XML doc comments into HTML.
/// </summary>
internal class DefaultDocCommentTransformer : IDocCommentTransformer
{
    /// <summary>
    /// <c>div</c> element name.
    /// </summary>
    private const string div = "div";

    /// <summary>
    /// Configuration for transforming the XML elements into HTML.
    /// </summary>
    private readonly IDocCommentTransformerConfiguration configuration;

    /// <summary>
    /// An array of parent XML doc comment elements.
    /// </summary>
    private readonly string[] docCommentParentElements = [
        XmlDocIdentifiers.Summary,
        XmlDocIdentifiers.Remarks,
        XmlDocIdentifiers.Returns,
        XmlDocIdentifiers.Exception,
        XmlDocIdentifiers.Value
    ];

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultDocCommentTransformer"/> class.
    /// </summary>
    /// <param name="typeRegistry">
    /// <inheritdoc cref="TypeRegistry"/>
    /// </param>
    /// <param name="configuration">
    /// <inheritdoc cref="configuration"/>
    /// </param>
    internal DefaultDocCommentTransformer(IDocCommentTransformerConfiguration configuration, ITypeRegistry typeRegistry)
    {
        this.configuration = configuration;
        TypeRegistry = typeRegistry;
    }

    /// <inheritdoc cref="DefaultDocCommentTransformer(IDocCommentTransformerConfiguration, ITypeRegistry)"/>
    internal DefaultDocCommentTransformer(IDocCommentTransformerConfiguration configuration)
    {
        this.configuration = configuration;

        TypeRegistry = new TypeRegistry(
                new Dictionary<string, ObjectTypeData>(),
                new Dictionary<string, EnumTypeData>(),
                new Dictionary<string, DelegateTypeData>()
            );
    }

    /// <inheritdoc/>
    public ITypeRegistry TypeRegistry { get; set; }

    /// <inheritdoc/>
    public string ToHtmlString(XElement docComment)
    {
        var docCommentCopy = new XElement(docComment);
        TransformToHtml(docCommentCopy);

        return docCommentCopy.ToString();
    }

    /// <summary>
    /// Transforms the <paramref name="element"/> to its HTML representation.
    /// </summary>
    /// <param name="element">The element to transform to HTML.</param>
    /// <remarks>
    /// The transformation is done in-place, therefore the caller should pass a copy of the element as a parameter.
    /// </remarks>
    private void TransformToHtml(XElement element)
    {
        // firstly transform the children
        foreach (var child in element.Elements())
        {
            TransformToHtml(child);
        }

        // transform the element to HTML based on its name
        var transformedElement = element.Name.ToString() switch
        {
            XmlDocIdentifiers.Para => TransformParagraphElement(element),
            XmlDocIdentifiers.List => TransformListElement(element),
            XmlDocIdentifiers.Item => TransformListItemElement(element),
            XmlDocIdentifiers.InlineCode => TransformInlineCodeElement(element),
            XmlDocIdentifiers.CodeBlock => TransformCodeBlockElement(element),
            XmlDocIdentifiers.Example => TransformExampleElement(element),
            XmlDocIdentifiers.See => TransformSeeElement(element),
            XmlDocIdentifiers.SeeAlso => TransformSeeAlsoElement(element),
            XmlDocIdentifiers.ParamRef => TransformParamRefElement(element),
            XmlDocIdentifiers.TypeParamRef => TransformTypeParamRefElement(element),
            string name when docCommentParentElements.Contains(name) => new XElement(div, element.Nodes()),
            _ => null // the element isn't one of the doc comment elements -> keep it as it is
        };

        if (transformedElement is not null)
        {
            // replace the element by HTML representation
            element.ReplaceDataBy(transformedElement);
        }
    }

    /// <summary>
    /// Copies child nodes from <paramref name="source"/> to <paramref name="target"/>.
    /// <para>
    /// If the <paramref name="target"/> isn't an empty element, the nodes are copied to its innermost empty descendant.
    /// </para>
    /// <para>
    /// Note: <paramref name="target"/> must have exactly one empty descendant, otherwise an exception is thrown.
    /// </para>
    /// </summary>
    /// <param name="source">
    /// The source <see cref="XElement"/> whose child nodes will be copied.
    /// </param>
    /// <param name="target">
    /// The target <see cref="XElement"/> to which the child nodes will be copied.
    /// </param>
    /// <returns>
    /// A copy of <paramref name="target"/> element, containing the child nodes of <paramref name="source"/>.
    /// </returns>
    private XElement CopyChildNodes(XElement source, XElement target)
    {
        var result = new XElement(target);
        var emptyDescendant = result.GetSingleEmptyDescendantOrSelf();

        emptyDescendant.Add(source.Nodes());

        return result;
    }

    /// <summary>
    /// Copies child nodes and the selected attribute from <paramref name="source"/> to <paramref name="target"/>.
    /// If the source has no child nodes, the attribute text is copied there.
    /// <para>
    /// If the <paramref name="target"/> isn't an empty element, the nodes are copied to its innermost empty descendant.
    /// </para>
    /// <para>
    /// Note: <paramref name="target"/> must have exactly one empty descendant, otherwise an exception is thrown.
    /// </para>
    /// </summary>
    /// <param name="source">
    /// The source <see cref="XElement"/> whose child nodes will be copied.
    /// </param>
    /// <param name="target">
    /// The target <see cref="XElement"/> to which the child nodes will be copied.
    /// </param>
    /// <param name="attributeName">Name of the attribute that will be copied.</param>
    /// <returns>
    /// A copy of <paramref name="target"/> element, containing the child nodes of <paramref name="source"/>.
    /// </returns>
    private XElement CopyChildNodesAndAttribute(XElement source, XElement target, string attributeName)
    {
        var result = new XElement(target);

        var emptyDescendant = result.GetSingleEmptyDescendantOrSelf();
        emptyDescendant.Add(source.Nodes());

        if (source.Attribute(attributeName) is XAttribute attr)
        {
            if (!emptyDescendant.Nodes().Any()) // no child nodes -> add attribute value
            {
                emptyDescendant.Add(attr.Value);
            }

            emptyDescendant.SetAttributeValue(attributeName, attr.Value); // add the attribute
        }

        return result;
    }

    /// <summary>
    /// Adds text to <paramref name="target"/> node.
    /// <para>
    /// If the <paramref name="target"/> isn't an empty element, the nodes are copied to its innermost empty descendant.
    /// </para>
    /// <para>
    /// Note: <paramref name="target"/> must have exactly one empty descendant, otherwise an exception is thrown.
    /// </para>
    /// </summary>
    /// <param name="text">The text to add.</param>
    /// <param name="target">The target <see cref="XElement"/> to which the child nodes will be copied.</param>
    /// <returns>A copy of <paramref name="target"/> element, containing the <paramref name="text"/>.</returns>
    private XElement AddTextNodeTo(string text, XElement target)
    {
        var result = new XElement(target);

        var emptyDescendant = result.GetSingleEmptyDescendantOrSelf();
        emptyDescendant.Add(text);

        return result;
    }

    /// <summary>
    /// Transforms the <c>&lt;list&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;list&gt;</c> element to transform.</param>
    /// <returns>The HTML representation of the <c>&lt;list&gt;</c> element.</returns>
    protected virtual XElement TransformListElement(XElement element)
    {
        string listType = element.Attribute("type")?.Value ?? "bullet";

        var types = new Dictionary<string, XElement>()
        {
            ["bullet"] = configuration.BulletListElement,
            ["number"] = configuration.NumberListElement,
            ["table"] = configuration.BulletListElement, // TODO: add 
        };

        return types.TryGetValue(listType, out var newElement)
            ? CopyChildNodes(element, newElement)
            : element;
    }

    /// <summary>
    /// Transforms the <c>&lt;item&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;item&gt;</c> element to transform.</param>
    /// <returns>The HTML representation of the <c>&lt;item&gt;</c> element.</returns>
    protected virtual XElement TransformListItemElement(XElement element)
    {
        return CopyChildNodes(element, configuration.ListItemElement);
    }

    /// <summary>
    /// Transforms the <c>&lt;para&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;para&gt;</c> element to transform.</param>
    /// <returns>The HTML representation of the <c>&lt;para&gt;</c> element.</returns>
    protected virtual XElement TransformParagraphElement(XElement element)
    {
        return CopyChildNodes(element, configuration.ParagraphElement);
    }

    /// <summary>
    /// Transforms the <c>&lt;c&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;c&gt;</c> element to transform.</param>
    /// <returns>The HTML representation of the <c>&lt;c&gt;</c> element.</returns>
    protected virtual XElement TransformInlineCodeElement(XElement element)
    {
        return CopyChildNodes(element, configuration.InlineCodeElement);
    }

    /// <summary>
    /// Transforms the <c>&lt;code&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;code&gt;</c> element to transform.</param>
    /// <returns>The HTML representation of the <c>&lt;code&gt;</c> element.</returns>
    protected virtual XElement TransformCodeBlockElement(XElement element)
    {
        return CopyChildNodes(element, configuration.CodeBlockElement);
    }

    /// <summary>
    /// Transforms the <c>&lt;example&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;example&gt;</c> element to transform.</param>
    /// <returns>The HTML representation of the <c>&lt;example&gt;</c> element.</returns>
    protected virtual XElement TransformExampleElement(XElement element)
    {
        return CopyChildNodes(element, configuration.ExampleElement);
    }

    /// <summary>
    /// Transforms any <c>&lt;see&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;see&gt;</c> element to transform.</param>
    /// <returns>The HTML representation of the <c>&lt;see&gt;</c> element.</returns>
    protected virtual XElement TransformSeeElement(XElement element)
    {
        if (element.Attribute(XmlDocIdentifiers.Href) is XAttribute hrefAttr)
        {
            return TransformSeeHrefElement(element, hrefAttr.Value);
        }
        else if (element.Attribute(XmlDocIdentifiers.Cref) is XAttribute crefAttr)
        {
            return TransformSeeCrefElement(element, crefAttr.Value);
        }
        else if (element.Attribute(XmlDocIdentifiers.Langword) is XAttribute langwordAttr)
        {
            return TransformSeeLangwordElement(element);
        }
        else
        {
            return element;
        }
    }

    /// <summary>
    /// Transforms the <c>&lt;see cref="..."&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;see&gt;</c> element to transform.</param>
    /// <param name="crefValue">Value of the <c>cref</c> attribute.</param>
    /// <returns>The HTML representation of the <c>&lt;see cref="..."&gt;</c> element.
    /// <para>
    /// If the referenced object is not found, the HTML representation of the not found <c>cref</c> element is returned.
    /// </para>
    /// </returns>
    protected virtual XElement TransformSeeCrefElement(XElement element, string crefValue)
    {
        return TransformAnyCrefElement(element, configuration.SeeCrefElement, configuration.SeeCrefNotFoundElement, crefValue);
    }

    /// <summary>
    /// Transforms the <c>&lt;see href="..."&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;see&gt;</c> element to transform.</param>
    /// <param name="hrefValue">Value of the <c>href</c> attribute.</param>
    /// <returns>The HTML representation of the <c>&lt;see href="..."&gt;</c> element.</returns>
    protected virtual XElement TransformSeeHrefElement(XElement element, string hrefValue)
    {
        return CopyChildNodesAndAttribute(element, configuration.SeeHrefElement, XmlDocIdentifiers.Href);
    }

    /// <summary>
    /// Transforms the <c>&lt;see langword="..."&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;see&gt;</c> element to transform.</param>
    /// <returns>The HTML representation of the <c>&lt;see langword="..."&gt;</c> element.</returns>
    protected virtual XElement TransformSeeLangwordElement(XElement element)
    {
        return element.Attribute(XmlDocIdentifiers.Langword) is XAttribute langwordAttr
            ? AddTextNodeTo(langwordAttr.Value, configuration.SeeLangwordElement)
            : element;
    }

    /// <summary>
    /// Transforms any <c>&lt;seealso&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;seealso&gt;</c> element to transform.</param>
    /// <returns>The HTML representation of the <c>&lt;seealso&gt;</c> element.</returns>
    protected virtual XElement TransformSeeAlsoElement(XElement element)
    {
        if (element.Attribute(XmlDocIdentifiers.Href) is XAttribute hrefAttr)
        {
            return TransformSeeHrefElement(element, hrefAttr.Value);
        }
        else if (element.Attribute(XmlDocIdentifiers.Cref) is XAttribute crefAttr)
        {
            return TransformSeeCrefElement(element, crefAttr.Value);
        }
        else
        {
            return element;
        }
    }

    /// <summary>
    /// Transforms the <c>&lt;seealso href="..."&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;seealso&gt;</c> element to transform.</param>
    /// <param name="hrefValue">Value of the <c>href</c> attribute.</param>
    /// <returns>The HTML representation of the <c>&lt;seealso href="..."&gt;</c> element.</returns>
    protected virtual XElement TransformSeeAlsoHrefElement(XElement element, string hrefValue)
    {
        return CopyChildNodesAndAttribute(element, configuration.SeeAlsoHrefElement, XmlDocIdentifiers.Href);
    }

    /// <summary>
    /// Transforms the <c>&lt;seealso cref="..."&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;seealso&gt;</c> element to transform.</param>
    /// <param name="crefValue">Value of the <c>cref</c> attribute.</param>
    /// <returns>
    /// The HTML representation of the <c>&lt;seealso cref="..."&gt;</c> element.
    /// <para>
    /// If the referenced object is not found, the HTML representation of the not found <c>cref</c> element is returned.
    /// </para>
    /// </returns>
    protected virtual XElement TransformSeeAlsoCrefElement(XElement element, string crefValue)
    {
        return TransformAnyCrefElement(element, configuration.SeeAlsoCrefElement, configuration.SeeAlsoCrefNotFoundElement, crefValue);
    }

    /// <summary>
    /// Transforms the <c>&lt;paramref&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;paramref&gt;</c> element to transform.</param>
    /// <returns>The HTML representation of the <c>&lt;paramref&gt;</c> element.</returns>
    protected virtual XElement TransformParamRefElement(XElement element)
    {
        if (element.Attribute(XmlDocIdentifiers.Name) is XAttribute nameAttr)
        {
            return AddTextNodeTo(nameAttr.Value, configuration.ParamRefElement);
        }
        return element;
    }

    /// <summary>
    /// Transforms the <c>&lt;paramref&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;paramref&gt;</c> element to transform.</param>
    /// <returns>The HTML representation of the <c>&lt;paramref&gt;</c> element.</returns>
    protected virtual XElement TransformTypeParamRefElement(XElement element)
    {
        if (element.Attribute(XmlDocIdentifiers.Name) is XAttribute nameAttr)
        {
            return AddTextNodeTo(nameAttr.Value, configuration.TypeParamRefElement);
        }
        return element;
    }

    /// <summary>
    /// Transforms any element with <c>cref</c> attribute to its HTML representation.
    /// </summary>
    /// <param name="element">The element to transform.</param>
    /// <param name="htmlTemplateIfFound">The HTML template used for transformation if the cref target is found.</param>
    /// <param name="crefValue">Value of the <c>cref</c> attribute.</param>
    /// <param name="htmlTemplateIfNotFound">The HTML template used for transformation if the cref target is found.</param>
    /// <returns>
    /// The HTML representation of the provided element.
    /// <para>
    /// The resulting HTMl depends also on the fact whether the referenced object is found.
    /// </para>
    /// </returns>
    private XElement TransformAnyCrefElement(XElement element, XElement htmlTemplateIfFound, XElement htmlTemplateIfNotFound, string crefValue)
    {
        string[] splitMemberName = crefValue.Split(':');
        (string objectIdentifier, string fullObjectName) = (splitMemberName[0], splitMemberName[1]);

        string typeId;
        string? memberId = null;

        if (objectIdentifier == CodeElementId.Type) // type
        {
            typeId = fullObjectName;
        }
        else if (CodeElementId.IsMember(objectIdentifier)) // member
        {
            (typeId, string memberName, string paramsString) = MemberSignatureParser.Parse(fullObjectName);
            memberId = memberName + paramsString;
        }
        else // reference not found
        {
            return AddTextNodeTo(crefValue, htmlTemplateIfNotFound);
        }

        // type found
        if (TypeRegistry.GetDeclaredType(typeId) is ITypeDeclaration type)
        {
            var result = new XElement(htmlTemplateIfFound);
            var emptyDescendant = result.GetSingleEmptyDescendantOrSelf();

            string targetUrl = "./" + typeId + ".html";
            string targetName = CSharpTypeName.Of(type);

            // add member ID (if the reference target is a type member)
            if (memberId is not null)
            {
                targetUrl += $"#{memberId}";
                targetName += $".{memberId}";
            }

            emptyDescendant.Add(
                new XAttribute(XmlDocIdentifiers.Href, targetUrl)
            );

            // no child nodes -> add target name as a text
            if (!element.Nodes().Any())
            {
                emptyDescendant.Add(targetName);
            }
            else // add child nodes if present
            {
                emptyDescendant.Add(element.Nodes());
            }

            return result;
        }
        else // type not found
        {
            return AddTextNodeTo(fullObjectName, htmlTemplateIfNotFound);
        }
    }
}
