using AngleSharp.Dom;

namespace RefDocGen.IntegrationTests;

internal static class Extensions
{
    internal static IElement GetDataId(this IElement element, string attributeValue)
    {
        return element.QuerySelector($"[data-id={attributeValue}]")
            ?? throw new ArgumentException("Not found");
    }

    internal static IHtmlCollection<IElement> GetDataIds(this IElement element, string attributeValue)
    {
        return element.QuerySelectorAll($"[data-id={attributeValue}]");
    }

    internal static IElement GetMember(this IDocument element, string memberId)
    {
        return element.GetElementById(memberId)
            ?? throw new ArgumentException("Not found");
    }
}
