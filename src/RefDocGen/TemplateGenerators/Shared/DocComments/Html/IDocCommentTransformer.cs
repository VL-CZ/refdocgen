using RefDocGen.CodeElements.TypeRegistry;
using System.Xml.Linq;

namespace RefDocGen.TemplateGenerators.Shared.DocComments.Html;

/// <summary>
/// Provides methods for transforming the XML doc comments into HTML.
/// </summary>
internal interface IDocCommentTransformer
{
    /// <summary>
    /// Converts the <paramref name="docComment"/> <see cref="XElement"/> to its HTML string representation.
    /// </summary>
    /// <param name="docComment">The element to be converted to its HTML string.</param>
    /// <returns>Raw HTML string representation of the <paramref name="docComment"/>. <c>null</c> if the <paramref name="docComment"/> is empty.</returns>
    string? ToHtmlString(XElement docComment);

    /// <summary>
    /// A registry of the declared types.
    /// </summary>
    /// <remarks>
    /// Note: The type registry is needed for resolving <c>cref</c> attributes.
    /// </remarks>
    ITypeRegistry TypeRegistry { get; set; }
}
