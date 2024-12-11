using System.Xml.Linq;

namespace RefDocGen.CodeElements.Abstract.Members;

public interface IExceptionData
{
    string Name { get; }

    XElement DocComment { get; }
}
