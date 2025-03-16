using AngleSharp.Dom;
using System.Text.RegularExpressions;

namespace RefDocGen.IntegrationTests.Tools;

/// <summary>
/// Class containing extension methods for the <see cref="IElement"/> interface.
/// </summary>
internal static class IElementExtensions
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
    /// Parses the content of the HTML element and returns it as a string.
    /// </summary>
    /// <param name="element">The element, whose HTML content is being parsed.</param>
    /// <returns>Parsed textual content of the HTML element.</returns>
    /// <remarks>
    /// <list type="bullet">
    /// <item>Firstly, only the textual content is selected - i.e. no child tags are present in the output</item>
    /// <item>Then, all continouos whitespace substrings are replaced by a single space character.</item>
    /// </list>
    /// </remarks>
    internal static string GetParsedContent(this IElement element)
    {
        return Regex.Replace(element.TextContent, @"\s+", " ").Trim();
    }

    /// <summary>
    /// Normalizes the inner HTML of the element and returns it as a string.
    /// </summary>
    /// <param name="element">The element, whose inner HTML is to be normalized.</param>
    /// <returns>Normalized textual content of the HTML element - i.e.  withall continouos whitespace substrings being replaced by a single space character.</returns>
    internal static string GetNormalizedInnerHtml(this IElement element)
    {
        return Regex.Replace(element.InnerHtml, @"\s+", " ").Trim();
    }

    /// <summary>
    /// Gets parsed textual content of the element with the given <paramref name="dataId"/>.
    /// </summary>
    /// <param name="element">The provided HTML to search for the <paramref name="dataId"/> element.</param>
    /// <param name="dataId">The data-id to search for.</param>
    /// <returns>Parsed textual content of the HTML element with the given <paramref name="dataId"/>.</returns>
    /// <seealso cref="GetParsedContent(IElement)"/>
    internal static string GetParsedContent(this IElement element, DataId dataId)
    {
        var targetElement = element.GetByDataId(dataId);
        return targetElement.GetParsedContent();
    }
}
