using AngleSharp.Dom;

namespace RefDocGen.IntegrationTests.Tools;

/// <summary>
/// Class containing extension methods for the <see cref="IDocument"/> interface.
/// </summary>
internal static class IDocumentExtensions
{
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
