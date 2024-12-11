using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using RefDocGen.Tools.Xml;

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
}
