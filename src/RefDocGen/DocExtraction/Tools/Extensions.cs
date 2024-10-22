using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Tools;

internal static class XElementExtensions
{
    internal static bool TryGetElement(this XElement element, string name, [MaybeNullWhen(false)] out XElement childElement)
    {
        childElement = element.Element(name);
        return childElement is not null;
    }

    internal static bool TryGetAttribute(this XElement element, string name, [MaybeNullWhen(false)] out XAttribute attribute)
    {
        attribute = element.Attribute(name);
        return attribute is not null;
    }

    internal static bool TryGetSummaryElement(this XElement element, [MaybeNullWhen(false)] out XElement summaryNode)
    {
        return element.TryGetElement(MagicStrings.Summary, out summaryNode);
    }

    internal static bool TryGetReturnsElement(this XElement element, [MaybeNullWhen(false)] out XElement returnsNode)
    {
        return element.TryGetElement(MagicStrings.Returns, out returnsNode);
    }

    internal static bool TryGetNameAttribute(this XElement element, [MaybeNullWhen(false)] out XAttribute attribute)
    {
        return element.TryGetAttribute(MagicStrings.Name, out attribute);
    }
}
