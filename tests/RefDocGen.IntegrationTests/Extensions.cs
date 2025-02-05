using AngleSharp.Dom;

namespace RefDocGen.IntegrationTests;

internal static class Extensions
{
    internal static IElement? GetByDataIdOrDefault(this IElement element, string dataId)
    {
        return element.QuerySelector($"[data-id={dataId}]");
    }

    internal static IElement GetByDataId(this IElement element, string dataId)
    {
        return element.GetByDataIdOrDefault(dataId)
            ?? throw new ArgumentException("Not found");
    }

    internal static IHtmlCollection<IElement> GetDataIds(this IElement element, string attributeValue)
    {
        return element.QuerySelectorAll($"[data-id={attributeValue}]");
    }

    internal static IElement GetMember(this IDocument document, string memberId)
    {
        return document.GetElementById(memberId)
            ?? throw new ArgumentException("Not found");
    }

    internal static IElement GetTypeName(this IDocument document)
    {
        return document.DocumentElement.GetByDataId("type-name-title");
    }

    internal static IElement GetTypeDocsSection(this IDocument document)
    {
        return document.DocumentElement.GetByDataId("type-docs");
    }
}
