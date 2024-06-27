namespace RefDocGen.Intermed;

public record ClassIntermed(string Name, AccessibilityModifier AccessibilityModifier, FieldIntermed[] Fields, MethodIntermed[] Methods)
{
}

public record FieldIntermed(string Name, string Type, AccessibilityModifier AccessibilityModifier, bool IsStatic)
{
}

public record MethodIntermed(string Name, MethodParameter[] Parameters, string ReturnType, AccessibilityModifier AccessibilityModifier, bool IsStatic, bool IsVirtual, bool IsAbstract)
{
}

public record MethodParameter(string Name, string Type);

public enum AccessibilityModifier { Private, Protected, Internal, Public } // TODO: add others
