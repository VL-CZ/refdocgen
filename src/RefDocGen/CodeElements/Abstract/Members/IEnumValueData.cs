using System.Xml.Linq;

namespace RefDocGen.CodeElements.Abstract.Members;

public interface IEnumValueData
{
    string Id { get; }

    string Name { get; }

    XElement DocComment { get; }
}
