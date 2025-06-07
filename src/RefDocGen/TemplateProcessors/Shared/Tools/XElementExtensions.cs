using System.Xml.Linq;

namespace RefDocGen.TemplateProcessors.Shared.Tools;

/// <summary>
/// Provides extension methods for <see cref="XElement"/> class.
/// </summary>
internal static class XElementExtensions
{
    /// <summary>
    /// Gets descendant-or-self node that is empty (i.e. has no children).
    /// </summary>
    /// <param name="element">Element, where to search for the nodes.</param>
    /// <returns>Descendant-or-self node that is empty.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if there's no such node or more that 2 of them.
    /// </exception>
    internal static XElement GetSingleEmptyDescendantOrSelf(this XElement element)
    {
        return element.DescendantsAndSelf().SingleOrDefault(n => !n.Nodes().Any())
            ?? throw new ArgumentException(
                $"""
                The element {element.Name} has no single empty descendant, see:
                {element}
                """
                );
    }

    /// <summary>
    /// Replaces the name, attributes and child nodes of <paramref name="target"/> element with the data from <paramref name="source"/>.
    /// </summary>
    /// <param name="target">The element whose name, attributes and child nodes are copied.</param>
    /// <param name="source">The element whose name, attributes and child nodes are to be replaced.</param>
    internal static void ReplaceDataBy(this XElement target, XElement source)
    {
        if (target == source)
        {
            return; // the elements are the same -> do nothing
        }

        target.Name = source.Name;
        target.ReplaceAll(source.Attributes(), source.Nodes());
    }

    /// <summary>
    /// Adds a 'class' attribute to an XML element.
    /// </summary>
    /// <param name="element">The XML element.</param>
    /// <param name="className">Value of the 'class' attribute to add.</param>
    /// <returns></returns>
    internal static XElement WithClass(this XElement element, string className)
    {
        element.SetAttributeValue("class", className);
        return element;
    }
}
