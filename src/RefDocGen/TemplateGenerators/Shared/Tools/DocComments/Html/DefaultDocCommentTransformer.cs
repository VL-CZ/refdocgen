using RefDocGen.CodeElements.Abstract;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.TemplateGenerators.Shared.Tools.Names;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.TemplateGenerators.Shared.Tools.DocComments.Html;

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
    /// Stack of visited &lt;list&gt; elements.
    /// </summary>
    private readonly Stack<ListType> visitedLists = new();

    /// <summary>
    /// Configuration for transforming the XML elements into HTML.
    /// </summary>
    private readonly IDocCommentHtmlConfiguration htmlConfiguration;

    /// <inheritdoc cref="TypeUrlResolver"/>
    private TypeUrlResolver? typeUrlResolver;

    /// <inheritdoc cref="TypeRegistry"/>
    private ITypeRegistry? typeRegistry;

    /// <summary>
    /// An array of parent XML doc comment elements.
    /// </summary>
    private readonly string[] docCommentParentElements = [
        XmlDocIdentifiers.Summary,
        XmlDocIdentifiers.Remarks,
        XmlDocIdentifiers.Param,
        XmlDocIdentifiers.TypeParam,
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
    /// <param name="htmlConfiguration">
    /// <inheritdoc cref="htmlConfiguration"/>
    /// </param>
    internal DefaultDocCommentTransformer(IDocCommentHtmlConfiguration htmlConfiguration, ITypeRegistry typeRegistry)
    {
        this.htmlConfiguration = htmlConfiguration;
        this.typeRegistry = typeRegistry;
        TypeUrlResolver = new(typeRegistry);
    }

    /// <inheritdoc cref="DefaultDocCommentTransformer(IDocCommentHtmlConfiguration, ITypeRegistry)"/>
    internal DefaultDocCommentTransformer(IDocCommentHtmlConfiguration htmlConfiguration)
    {
        this.htmlConfiguration = htmlConfiguration;
    }

    /// <inheritdoc/>
    public ITypeRegistry TypeRegistry
    {
        get => typeRegistry ?? throw new InvalidOperationException("ERROR: Type registry not set."); // TODO: update
        set
        {
            typeRegistry = value;
            TypeUrlResolver = new(typeRegistry);
        }
    }

    /// <summary>
    /// Resolver of the individual type's documentation pages.
    /// </summary>
    private TypeUrlResolver TypeUrlResolver
    {
        get => typeUrlResolver ?? throw new InvalidOperationException("ERROR: Type registry not set."); // TODO: update
        set => typeUrlResolver = value;
    }

    /// <inheritdoc/>
    public string? ToHtmlString(XElement docComment)
    {
        var docCommentCopy = new XElement(docComment);
        TransformToHtml(docCommentCopy);

        if (!docCommentCopy.Nodes().Any()) // no content -> return null
        {
            return null;
        }

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
        bool isList = element.Name.ToString() == XmlDocIdentifiers.List;

        if (isList)
        {
            MarkListAsVisited(element);
        }

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
            XmlDocIdentifiers.Term => TransformTermElement(element),
            XmlDocIdentifiers.Description => TransformDescriptionElement(element),
            XmlDocIdentifiers.ListHeader => TransformListHeaderElement(element),
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

        if (isList) // pop the visited list element
        {
            _ = visitedLists.Pop();
        }
    }

    /// <summary>
    /// Marks the list element as visited.
    /// </summary>
    /// <param name="listElement">The &lt;list&gt; element.</param>
    private void MarkListAsVisited(XElement listElement)
    {
        bool isTable = listElement.Attribute("type")?.Value == "table";
        var listType = isTable ? ListType.Table : ListType.NonTable;

        visitedLists.Push(listType);
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
            ["bullet"] = htmlConfiguration.BulletListElement,
            ["number"] = htmlConfiguration.NumberListElement,
            ["table"] = htmlConfiguration.TableListElement,
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
        if (visitedLists.Peek() == ListType.NonTable)
        {
            return CopyChildNodes(element, htmlConfiguration.ListItemElement);
        }
        else
        {
            return CopyChildNodes(element, htmlConfiguration.TableItemElement);
        }
    }

    /// <summary>
    /// Transforms the <c>&lt;term&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;term&gt;</c> element to transform.</param>
    /// <returns>The HTML representation of the <c>&lt;term&gt;</c> element.</returns>
    protected virtual XElement TransformTermElement(XElement element)
    {
        if (visitedLists.Peek() == ListType.NonTable)
        {
            return CopyChildNodes(element, htmlConfiguration.ListTermElement);
        }
        else
        {
            return CopyChildNodes(element, htmlConfiguration.TableTermElement);
        }
    }

    /// <summary>
    /// Transforms the <c>&lt;description&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;description&gt;</c> element to transform.</param>
    /// <returns>The HTML representation of the <c>&lt;description&gt;</c> element.</returns>
    protected virtual XElement TransformDescriptionElement(XElement element)
    {
        if (visitedLists.Peek() == ListType.NonTable)
        {
            return CopyChildNodes(element, htmlConfiguration.ListDescriptionElement);
        }
        else
        {
            return CopyChildNodes(element, htmlConfiguration.TableDescriptionElement);
        }
    }

    /// <summary>
    /// Transforms the <c>&lt;listheader&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;listheader&gt;</c> element to transform.</param>
    /// <returns>The HTML representation of the <c>&lt;listheader&gt;</c> element.</returns>
    protected virtual XElement TransformListHeaderElement(XElement element)
    {
        return CopyChildNodes(element, htmlConfiguration.ListHeaderElement);
    }

    /// <summary>
    /// Transforms the <c>&lt;para&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;para&gt;</c> element to transform.</param>
    /// <returns>The HTML representation of the <c>&lt;para&gt;</c> element.</returns>
    protected virtual XElement TransformParagraphElement(XElement element)
    {
        return CopyChildNodes(element, htmlConfiguration.ParagraphElement);
    }

    /// <summary>
    /// Transforms the <c>&lt;c&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;c&gt;</c> element to transform.</param>
    /// <returns>The HTML representation of the <c>&lt;c&gt;</c> element.</returns>
    protected virtual XElement TransformInlineCodeElement(XElement element)
    {
        return CopyChildNodes(element, htmlConfiguration.InlineCodeElement);
    }

    /// <summary>
    /// Transforms the <c>&lt;code&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;code&gt;</c> element to transform.</param>
    /// <returns>The HTML representation of the <c>&lt;code&gt;</c> element.</returns>
    protected virtual XElement TransformCodeBlockElement(XElement element)
    {
        return CopyChildNodes(element, htmlConfiguration.CodeBlockElement);
    }

    /// <summary>
    /// Transforms the <c>&lt;example&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;example&gt;</c> element to transform.</param>
    /// <returns>The HTML representation of the <c>&lt;example&gt;</c> element.</returns>
    protected virtual XElement TransformExampleElement(XElement element)
    {
        return CopyChildNodes(element, htmlConfiguration.ExampleElement);
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
        else if (element.Attribute(XmlDocIdentifiers.Langword) is not null)
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
        return TransformAnyCrefElement(element, htmlConfiguration.SeeCrefElement, htmlConfiguration.SeeCrefNotFoundElement, crefValue);
    }

    /// <summary>
    /// Transforms the <c>&lt;see href="..."&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;see&gt;</c> element to transform.</param>
    /// <param name="hrefValue">Value of the <c>href</c> attribute.</param>
    /// <returns>The HTML representation of the <c>&lt;see href="..."&gt;</c> element.</returns>
    protected virtual XElement TransformSeeHrefElement(XElement element, string hrefValue)
    {
        return CopyChildNodesAndAttribute(element, htmlConfiguration.SeeHrefElement, XmlDocIdentifiers.Href);
    }

    /// <summary>
    /// Transforms the <c>&lt;see langword="..."&gt;</c> element to its HTML representation.
    /// </summary>
    /// <param name="element">The <c>&lt;see&gt;</c> element to transform.</param>
    /// <returns>The HTML representation of the <c>&lt;see langword="..."&gt;</c> element.</returns>
    protected virtual XElement TransformSeeLangwordElement(XElement element)
    {
        return element.Attribute(XmlDocIdentifiers.Langword) is XAttribute langwordAttr
            ? AddTextNodeTo(langwordAttr.Value, htmlConfiguration.SeeLangwordElement)
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
        return CopyChildNodesAndAttribute(element, htmlConfiguration.SeeAlsoHrefElement, XmlDocIdentifiers.Href);
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
        return TransformAnyCrefElement(element, htmlConfiguration.SeeAlsoCrefElement, htmlConfiguration.SeeAlsoCrefNotFoundElement, crefValue);
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
            return AddTextNodeTo(nameAttr.Value, htmlConfiguration.ParamRefElement);
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
            return AddTextNodeTo(nameAttr.Value, htmlConfiguration.TypeParamRefElement);
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

        // type documentation found
        if (TypeUrlResolver.GetUrlOf(typeId, memberId) is string targetUrl)
        {
            var result = new XElement(htmlTemplateIfFound);
            var emptyDescendant = result.GetSingleEmptyDescendantOrSelf();

            string targetName = typeId;

            if (TypeRegistry.GetDeclaredType(typeId) is ITypeDeclaration type) // the type is found in the registry -> get its name
            {
                targetName = CSharpTypeName.Of(type);
            }

            // add member ID (if the reference target is a type member)
            if (memberId is not null)
            {
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
        else // type documentation not found
        {
            return AddTextNodeTo(fullObjectName, htmlTemplateIfNotFound);
        }
    }
}
