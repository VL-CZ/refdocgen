using System.Reflection;

namespace RefDocGen.Intermed;

public record FieldIntermed(FieldInfo FieldInfo)
{
    public string Name => FieldInfo.Name;

    public string Type => FieldInfo.FieldType.Name;

    public AccessModifier AccessibilityModifier => AccessModifierExtensions.GetAccessModifier(FieldInfo.IsPrivate, FieldInfo.IsFamily,
        FieldInfo.IsAssembly, FieldInfo.IsPublic, FieldInfo.IsFamilyAndAssembly, FieldInfo.IsFamilyOrAssembly);

    public bool IsStatic => FieldInfo.IsStatic;

    public bool IsReadonly => FieldInfo.IsInitOnly;

    public bool IsConstant => FieldInfo.IsLiteral;
}
