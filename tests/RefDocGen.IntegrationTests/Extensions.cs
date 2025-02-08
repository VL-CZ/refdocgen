using AngleSharp.Dom;

namespace RefDocGen.IntegrationTests;

internal static class Extensions
{
    internal static IElement? GetByDataIdOrDefault(this IElement element, string dataId)
    {
        return element.QuerySelector($"[data-id={dataId}]");
    }

    internal static IElement? GetByDataIdOrDefault(this IElement element, DataId dataId)
    {
        return GetByDataIdOrDefault(element, dataId.GetString());
    }

    internal static IElement GetByDataId(this IElement element, DataId dataId)
    {
        return element.GetByDataIdOrDefault(dataId)
            ?? throw new ArgumentException("Not found");
    }

    internal static IHtmlCollection<IElement> GetByDataIds(this IElement element, DataId dataId)
    {
        return element.QuerySelectorAll($"[data-id={dataId.GetString()}]");
    }

    internal static IElement GetMember(this IDocument document, string memberId)
    {
        return document.GetElementById(memberId)
            ?? throw new ArgumentException("Not found");
    }

    internal static IElement GetTypeSignature(this IDocument document)
    {
        return GetByDataId(document.DocumentElement, DataId.DeclaredTypeSignature);
    }

    internal static IElement GetTypeDataSection(this IDocument document)
    {
        return GetByDataId(document.DocumentElement, DataId.TypeDataSection);
    }
}
