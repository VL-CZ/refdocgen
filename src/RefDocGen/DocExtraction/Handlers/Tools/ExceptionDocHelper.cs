using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Tools;

internal class ExceptionDocHelper
{
    internal static IReadOnlyList<IExceptionData> Add(IEnumerable<XElement> elements)
    {
        var exceptions = new List<IExceptionData>();

        foreach (var e in elements)
        {
            if (e.TryGetCrefAttribute(out var exceptionName))
            {
                exceptions.Add(
                    new ExceptionData(exceptionName.Value, e)
                    );
            }
        }

        return exceptions;
    }
}
