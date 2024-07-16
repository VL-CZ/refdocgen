using RefDocGen.DocExtraction;
using System.Xml.Linq;

namespace RefDocGen.MemberData;

public record ClassData(string Name, AccessModifier AccessModifier, FieldData[] Fields, PropertyData[] Properties, MethodData[] Methods)
{
    public XElement DocComment { get; init; } = DocCommentTools.Empty;
}
