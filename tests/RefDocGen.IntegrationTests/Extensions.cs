using AngleSharp.Dom;

namespace RefDocGen.IntegrationTests;

internal static class Extensions
{
    internal static IElement? GetByDataIdOrDefault(this IElement element, string dataId)
    {
        return element.QuerySelector($"[data-id={dataId}]");
    }

    internal static IElement GetByDataId(this IElement element, XmlDocElement dataId)
    {
        return element.GetByDataIdOrDefault(dataId.ToXmlString())
            ?? throw new ArgumentException("Not found");
    }

    internal static IHtmlCollection<IElement> GetByDataIds(this IElement element, XmlDocElement dataId)
    {
        return element.QuerySelectorAll($"[data-id={dataId.ToXmlString()}]");
    }

    internal static IElement GetMember(this IDocument document, string memberId)
    {
        return document.GetElementById(memberId)
            ?? throw new ArgumentException("Not found");
    }

    internal static IElement GetTypeName(this IDocument document)
    {
        return document.DocumentElement.GetByDataIdOrDefault("type-name-title");
    }

    internal static IElement GetTypeDataSection(this IDocument document)
    {
        return document.DocumentElement.GetByDataIdOrDefault("declared-type-data");
    }
}
