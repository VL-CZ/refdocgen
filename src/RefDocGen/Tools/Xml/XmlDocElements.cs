using System.Xml.Linq;

namespace RefDocGen.Tools.Xml;

/// <summary>
/// Provides the selected XML doc comment nodes.
/// </summary>
internal static class XmlDocElements
{
    /// <summary>
    /// Create new empty 'summary' <see cref="XElement"/>.
    /// </summary>
    internal static XElement EmptySummary => new(XmlDocIdentifiers.Summary);

    /// <summary>
    /// Create new empty 'returns' <see cref="XElement"/>.
    /// </summary>
    internal static XElement EmptyReturns => new(XmlDocIdentifiers.Returns);

    /// <summary>
    /// Create new 'param' <see cref="XElement"/> with the given 'name' attribute and no children.
    /// </summary>
    /// <param name="name">Name attribute value.</param>
    /// <returns>'param' <see cref="XElement"/> with the given 'name' attribute.</returns>
    internal static XElement EmptyParamWithName(string name)
    {
        return new XElement(
                XmlDocIdentifiers.Param,
                new XAttribute(XmlDocIdentifiers.Name, name)
            );
    }
}
