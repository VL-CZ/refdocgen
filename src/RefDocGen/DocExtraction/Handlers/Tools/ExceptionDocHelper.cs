using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Tools;

/// <summary>
/// Helper class used for documenting member exceptions.
/// </summary>
internal class ExceptionDocHelper
{
    /// <summary>
    /// Parses the provided XML exception doc comments to a collection of <see cref="IExceptionData"/>.
    /// </summary>
    /// <param name="elements">A collection of XML doc comments documenting the exceptions.</param>
    /// <returns>A collection of exception data corresponding to the provided XML doc comments.</returns>
    internal static IReadOnlyList<IExceptionData> Parse(IEnumerable<XElement> elements)
    {
        var exceptions = new List<IExceptionData>();

        foreach (var e in elements)
        {
            if (e.TryGetCrefAttribute(out var exceptionName))
            {
                string[] splitMemberName = exceptionName.Value.Split(':');
                (_, string fullExceptionName) = (splitMemberName[0], splitMemberName[1]);

                exceptions.Add(
                    new ExceptionData(fullExceptionName, e)
                    );
            }
        }

        return exceptions;
    }
}
