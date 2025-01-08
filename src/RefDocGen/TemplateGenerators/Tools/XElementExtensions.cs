using System.Xml.Linq;

namespace RefDocGen.TemplateGenerators.Tools;

/// <summary>
/// Provides extension methods for <see cref="XElement"/> class.
/// </summary>
internal static class XElementExtensions
{

    internal static XElement GetEmptyDescendantOrSelf(this XElement element)
    {
        return element.DescendantsAndSelf().SingleOrDefault(n => n.IsEmpty)
            ?? throw new ArgumentException(""); // TODO: add exception message
    }
}
