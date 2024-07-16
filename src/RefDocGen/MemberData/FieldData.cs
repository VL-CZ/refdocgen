using System.Reflection;

namespace RefDocGen.MemberData;

public record FieldData(FieldInfo FieldInfo)
{
    public string Name => FieldInfo.Name;

    public string Type => FieldInfo.FieldType.Name;

    public AccessModifier AccessModifier => AccessModifierExtensions.GetAccessModifier(FieldInfo.IsPrivate, FieldInfo.IsFamily,
        FieldInfo.IsAssembly, FieldInfo.IsPublic, FieldInfo.IsFamilyAndAssembly, FieldInfo.IsFamilyOrAssembly);

    public bool IsStatic => FieldInfo.IsStatic;

    public bool IsReadonly => FieldInfo.IsInitOnly;

    public bool IsConstant => FieldInfo.IsLiteral;
}
