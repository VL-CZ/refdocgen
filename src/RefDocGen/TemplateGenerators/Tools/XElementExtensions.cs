using System.Xml.Linq;

namespace RefDocGen.TemplateGenerators.Tools;

/// <summary>
/// Provides extension methods for <see cref="XElement"/> class.
/// </summary>
internal static class XElementExtensions
{

    internal static XElement GetEmptyDescendantOrSelf(this XElement element)
    {
        var x = element;
        return element.DescendantsAndSelf().SingleOrDefault(n => !n.Nodes().Any())
            ?? throw new ArgumentException($"{element} problem."); // TODO: add exception message
    }
}
