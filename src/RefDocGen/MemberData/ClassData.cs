using System.Xml.Linq;

namespace RefDocGen.MemberData;

public record ClassData(string Name, AccessModifier AccessModifier, FieldData[] Fields, PropertyData[] Properties, MethodData[] Methods, XElement DocComment)
{
}
