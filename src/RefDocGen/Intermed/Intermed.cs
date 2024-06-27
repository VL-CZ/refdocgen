namespace RefDocGen.Intermed;

public record ClassIntermed(string Name, AccessibilityModifier AccessibilityModifier, FieldIntermed[] Fields, PropertyIntermed[] Properties, MethodIntermed[] Methods)
{
}

public record FieldIntermed(string Name, string Type, AccessibilityModifier AccessibilityModifier, bool IsStatic)
{
}

//public record PropertyAccessorIntermed(AccessibilityModifier AccessibilityModifier);

public record PropertyIntermed(string Name, string Type, AccessibilityModifier? Getter, AccessibilityModifier? Setter /*, bool IsStatic, bool IsVirtual, bool IsAbstract */)
{
}

public record MethodIntermed(string Name, MethodParameter[] Parameters, string ReturnType, AccessibilityModifier AccessibilityModifier, bool IsStatic, bool IsVirtual, bool IsAbstract)
{
}

public record MethodParameter(string Name, string Type);

public enum AccessibilityModifier { Private, Protected, Internal, Public } // TODO: add others
