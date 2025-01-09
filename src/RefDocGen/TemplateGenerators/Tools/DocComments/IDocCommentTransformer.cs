using RefDocGen.CodeElements.Abstract;
using System.Xml.Linq;

namespace RefDocGen.TemplateGenerators.Tools.DocComments;

internal interface IDocCommentTransformer
{
    /// <summary>
    /// Converts the <paramref name="docComment"/> <see cref="XElement"/> to its HTML string representation.
    /// </summary>
    /// <param name="docComment">The element to be converted to its HTML string.</param>
    /// <returns>HTML string representation of the <paramref name="docComment"/>.</returns>
    string ToHtmlString(XElement docComment);

    ITypeRegistry TypeRegistry { get; set; }
}
