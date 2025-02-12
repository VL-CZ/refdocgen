using AngleSharp.Dom;

namespace RefDocGen.IntegrationTests.Tools;

/// <summary>
/// Class containing extension methods for the objects from <c>AngleSharp.Dom</c> namespace.
/// </summary>
internal static class DomExtensions
{
    /// <summary>
    /// Gets an element by its <c>data-id</c> value.
    /// </summary>
    /// <param name="element">The element to search in.</param>
    /// <param name="dataId">The provided <c>data-id</c> value.</param>
    /// <returns>An HTML element with the given <c>data-id</c> value, or <c>null</c> if no such element exists.</returns>
    /// <remarks>If multiple elements are found, the first one is returned.</remarks>
    internal static IElement? GetByDataIdOrDefault(this IElement element, DataId dataId)
    {
        return element.QuerySelector($"[data-id={dataId.GetString()}]");
    }

    /// <summary>
    /// Gets an element by its <c>data-id</c> value.
    /// </summary>
    /// <param name="element">The element to search in.</param>
    /// <param name="dataId">The provided <c>data-id</c> value.</param>
    /// <returns>An HTML element with the given <c>data-id</c> value, or throws an exception if no such element exists.</returns>
    /// <exception cref="ArgumentException">Thrown if there's no element found with the given <paramref name="dataId"/>.</exception>
    /// <remarks>If multiple elements are found, the first one is returned.</remarks>
    internal static IElement GetByDataId(this IElement element, DataId dataId)
    {
        return element.GetByDataIdOrDefault(dataId)
            ?? throw new ArgumentException($"No element with data-id='${dataId}' found");
    }

    /// <summary>
    /// Gets an element by its <c>data-id</c> value.
    /// </summary>
    /// <param name="element">The element to search in.</param>
    /// <param name="dataId">The provided <c>data-id</c> value.</param>
    /// <returns>An HTML element with the given <c>data-id</c> value, an exception is thrown if no such element exists.</returns>
    /// <exception cref="ArgumentException">Thrown if there's no element found with the given <paramref name="dataId"/>.</exception>
    internal static IHtmlCollection<IElement> GetByDataIds(this IElement element, DataId dataId)
    {
        return element.QuerySelectorAll($"[data-id={dataId.GetString()}]");
    }

    /// <summary>
    /// Gets an element representing the member data.
    /// </summary>
    /// <param name="document">The document to search in.</param>
    /// <param name="memberId">ID of the member to search.</param>
    /// <returns>An HTML element representing the member with the given ID, an exception is thrown if no such element exists.</returns>
    /// <exception cref="ArgumentException">Thrown if there's no member found with the given <paramref name="memberId"/>.</exception>
    internal static IElement GetMemberElement(this IDocument document, string memberId)
    {
        return document.GetElementById(memberId)
            ?? throw new ArgumentException($"Member '{memberId}' not found");
    }

    /// <summary>
    /// Gets a section of the page representing the type data.
    /// </summary>
    /// <param name="document">The document to search in.</param>
    /// <returns>An HTML element representing the type data.</returns>
    internal static IElement GetTypeDataSection(this IDocument document)
    {
        return document.DocumentElement.GetByDataId(DataId.TypeDataSection);
    }
}
