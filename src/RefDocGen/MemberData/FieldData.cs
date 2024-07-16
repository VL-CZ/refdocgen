using RefDocGen.DocExtraction;
using RefDocGen.MemberData.Interfaces;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.MemberData;

public record FieldData(FieldInfo FieldInfo) : IMemberData
{
    public string Name => FieldInfo.Name;

    public string Type => FieldInfo.FieldType.Name;

    public AccessModifier AccessModifier => AccessModifierExtensions.GetAccessModifier(FieldInfo.IsPrivate, FieldInfo.IsFamily,
        FieldInfo.IsAssembly, FieldInfo.IsPublic, FieldInfo.IsFamilyAndAssembly, FieldInfo.IsFamilyOrAssembly);

    public bool IsStatic => FieldInfo.IsStatic;

    public bool IsReadonly => FieldInfo.IsInitOnly;

    public bool IsConstant => FieldInfo.IsLiteral;

    public XElement DocComment { get; init; } = DocCommentTools.Empty;
}
