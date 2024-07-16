using System.Xml.Linq;

namespace RefDocGen.MemberData.Interfaces;

public interface IDocumentedMember
{
    XElement DocComment { get; }
}
