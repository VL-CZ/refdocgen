using RefDocGen.Tools.Xml;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Tools;

/// <summary>
/// Class containing extension methods for <see cref="XElement"/> class.
/// </summary>
internal static class XElementExtensions
{
    /// <summary>
    /// Tries to get the first child element with the specified name.
    /// </summary>
    /// <param name="element">The current <see cref="XElement"/> to search within.</param>
    /// <param name="name">The name of the child element to find.</param>
    /// <param name="childElement">The found <see cref="XElement"/>, or <see langword="null"/> if not found.</param>
    /// <returns><see langword="true"/> if the child element is found; otherwise, <see langword="false"/>.</returns>
    internal static bool TryGetElement(this XElement element, string name, [MaybeNullWhen(false)] out XElement childElement)
    {
        childElement = element.Element(name);
        return childElement is not null;
    }

    /// <summary>
    /// Tries to get an attribute with the specified name.
    /// </summary>
    /// <param name="element">The current <see cref="XElement"/> to search within.</param>
    /// <param name="name">The name of the attribute to find.</param>
    /// <param name="attribute">The found <see cref="XAttribute"/>, or <see langword="null"/> if not found.</param>
    /// <returns><see langword="true"/> if the attribute is found; otherwise, <see langword="false"/>.</returns>
    internal static bool TryGetAttribute(this XElement element, string name, [MaybeNullWhen(false)] out XAttribute attribute)
    {
        attribute = element.Attribute(name);
        return attribute is not null;
    }

    /// <summary>
    /// Tries to get the 'summary' child element.
    /// </summary>
    /// <param name="element">The current <see cref="XElement"/> to search within.</param>
    /// <param name="summaryNode">The found 'summary' <see cref="XElement"/>, or <see langword="null"/> if not found.</param>
    /// <returns><see langword="true"/> if the 'summary' element is found; otherwise, <see langword="false"/>.</returns>
    internal static bool TryGetSummaryElement(this XElement element, [MaybeNullWhen(false)] out XElement summaryNode)
    {
        return element.TryGetElement(XmlDocIdentifiers.Summary, out summaryNode);
    }

    /// <summary>
    /// Tries to get the 'value' child element.
    /// </summary>
    /// <param name="element">The current <see cref="XElement"/> to search within.</param>
    /// <param name="valueNode">The found 'value' <see cref="XElement"/>, or <see langword="null"/> if not found.</param>
    /// <returns><see langword="true"/> if the 'value' element is found; otherwise, <see langword="false"/>.</returns>
    internal static bool TryGetValueElement(this XElement element, [MaybeNullWhen(false)] out XElement valueNode)
    {
        return element.TryGetElement(XmlDocIdentifiers.Value, out valueNode);
    }

    /// <summary>
    /// Tries to get the 'remarks' child element.
    /// </summary>
    /// <param name="element">The current <see cref="XElement"/> to search within.</param>
    /// <param name="remarksNode">The found 'remarks' <see cref="XElement"/>, or <see langword="null"/> if not found.</param>
    /// <returns><see langword="true"/> if the 'remarks' element is found; otherwise, <see langword="false"/>.</returns>
    internal static bool TryGetRemarksElement(this XElement element, [MaybeNullWhen(false)] out XElement remarksNode)
    {
        return element.TryGetElement(XmlDocIdentifiers.Remarks, out remarksNode);
    }

    /// <summary>
    /// Tries to get the 'returns' child element.
    /// </summary>
    /// <param name="element">The current <see cref="XElement"/> to search within.</param>
    /// <param name="returnsNode">The found 'returns' <see cref="XElement"/>, or <see langword="null"/> if not found.</param>
    /// <returns><see langword="true"/> if the 'returns' element is found; otherwise, <see langword="false"/>.</returns>
    internal static bool TryGetReturnsElement(this XElement element, [MaybeNullWhen(false)] out XElement returnsNode)
    {
        return element.TryGetElement(XmlDocIdentifiers.Returns, out returnsNode);
    }

    /// <summary>
    /// Tries to get the 'name' attribute.
    /// </summary>
    /// <param name="element">The current <see cref="XElement"/> to search within.</param>
    /// <param name="attribute">The found 'name' <see cref="XAttribute"/>, or <see langword="null"/> if not found.</param>
    /// <returns><see langword="true"/> if the 'name' attribute is found; otherwise, <see langword="false"/>.</returns>
    internal static bool TryGetNameAttribute(this XElement element, [MaybeNullWhen(false)] out XAttribute attribute)
    {
        return element.TryGetAttribute(XmlDocIdentifiers.Name, out attribute);
    }

    /// <summary>
    /// Tries to get the 'cref' attribute.
    /// </summary>
    /// <param name="element">The current <see cref="XElement"/> to search within.</param>
    /// <param name="attribute">The found 'cref' <see cref="XAttribute"/>, or <see langword="null"/> if not found.</param>
    /// <returns><see langword="true"/> if the 'cref' attribute is found; otherwise, <see langword="false"/>.</returns>
    internal static bool TryGetCrefAttribute(this XElement element, [MaybeNullWhen(false)] out XAttribute attribute)
    {
        return element.TryGetAttribute(XmlDocIdentifiers.Cref, out attribute);
    }

    /// <summary>
    /// Selects descendant 'inheritdoc' elements of the given <paramref name="element"/>.
    /// </summary>
    /// <param name="element">Element, whose descendant inheritdocs we search.</param>
    /// <param name="inheritDocType">Type of the 'inheritdoc' elements to search for.</param>
    /// <returns>
    /// All descendant 'inheritdoc' elements of the given <paramref name="element"/>.
    /// <para>
    /// An empty enumerable is returned if there are no such elements.
    /// </para>
    /// </returns>
    internal static IEnumerable<XElement> GetInheritDocs(this XElement? element, InheritDocKind inheritDocType = InheritDocKind.Any)
    {
        if (element is null)
        {
            return [];
        }

        var allInheritDocs = element.Descendants(XmlDocIdentifiers.InheritDoc);

        if (inheritDocType == InheritDocKind.NonCref)
        {
            return allInheritDocs.Where(e => e.Attribute(XmlDocIdentifiers.Cref) is null);
        }
        else if (inheritDocType == InheritDocKind.Cref)
        {
            return allInheritDocs.Where(e => e.Attribute(XmlDocIdentifiers.Cref) is not null);
        }
        else
        {
            return allInheritDocs;
        }
    }
}

/// <summary>
/// Represents kind of the inheritdoc doc comment.
/// </summary>
internal enum InheritDocKind
{
    /// <summary>
    /// Represents any inheritdoc element.
    /// </summary>
    Any,

    /// <summary>
    /// Represents any inheritdoc element without 'cref' attribute.
    /// </summary>
    NonCref,

    /// <summary>
    /// Represents any inheritdoc element with 'cref' attribute.
    /// </summary>
    Cref
}
