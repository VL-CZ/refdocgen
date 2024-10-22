using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Tools;

/// <summary>
/// Contains helper methods for anything related to XML documentation comments.
/// </summary>
internal static class EmptyDocCommentNode
{
    /// <summary>
    /// Get empty 'summary' <see cref="XElement"/>.
    /// </summary>
    internal static XElement Summary => new("summary");

    /// <summary>
    /// Get empty 'returns' <see cref="XElement"/>.
    /// </summary>
    internal static XElement Returns => new("returns");

    /// <summary>
    /// Get empty 'param' <see cref="XElement"/> with the given 'name' attribute.
    /// </summary>
    /// <param name="name">Name attribute value.</param>
    /// <returns>'param' <see cref="XElement"/> with the given 'name' attribute.</returns>
    internal static XElement ParamWithName(string name)
    {
        return new XElement("param", new XAttribute("name", name));
    }
}
