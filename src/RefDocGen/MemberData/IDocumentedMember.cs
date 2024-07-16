using System.Xml.Linq;

namespace RefDocGen.MemberData;

public interface IDocumentedMember
{
    XElement DocComment { get; }
}
