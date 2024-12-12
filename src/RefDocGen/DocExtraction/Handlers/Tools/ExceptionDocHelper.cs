using RefDocGen.CodeElements.Abstract.Types.Exception;
using RefDocGen.CodeElements.Concrete.Types.Exception;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Tools;

/// <summary>
/// Helper class used for documenting member exceptions.
/// </summary>
internal class ExceptionDocHelper
{
    /// <summary>
    /// Parses the provided XML exception doc comments to a collection of <see cref="IExceptionDocumentation"/>.
    /// </summary>
    /// <param name="elements">A collection of XML doc comments documenting the exceptions.</param>
    /// <returns>A collection of exception data corresponding to the provided XML doc comments.</returns>
    internal static IReadOnlyList<IExceptionDocumentation> Parse(IEnumerable<XElement> elements)
    {
        var exceptions = new List<IExceptionDocumentation>();

        foreach (var e in elements)
        {
            if (e.TryGetCrefAttribute(out var exceptionName))
            {
                string[] splitMemberName = exceptionName.Value.Split(':');
                (_, string fullExceptionName) = (splitMemberName[0], splitMemberName[1]);

                exceptions.Add(
                    new ExceptionDocumentation(fullExceptionName, e)
                    );
            }
        }

        return exceptions;
    }
}
