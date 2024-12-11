using RefDocGen.CodeElements.Abstract.Members;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Concrete.Members;

internal class ExceptionData : IExceptionData
{
    public ExceptionData(string name, XElement docComment)
    {
        Name = name;
        DocComment = docComment;
    }

    public string Name { get; }

    public XElement DocComment { get; }
}
