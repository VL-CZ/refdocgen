using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Tools;

internal class ExceptionDocHelper
{
    internal static IReadOnlyList<IExceptionData> GetFrom(IEnumerable<XElement> elements)
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
