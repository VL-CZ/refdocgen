using RefDocGen.Tools;

namespace RefDocGen.Intermed;

public record ClassIntermed(string Name, AccessModifier AccessibilityModifier, FieldIntermed[] Fields, PropertyIntermed[] Properties, MethodIntermed[] Methods)
{
}

public record FieldIntermed(string Name, string Type, AccessModifier AccessibilityModifier, bool IsStatic, bool IsReadonly, bool IsConstant)
{
}

//public record PropertyAccessorIntermed(AccessibilityModifier AccessibilityModifier);

public record PropertyIntermed(string Name, string Type, AccessModifier AccessibilityModifier, AccessModifier? Getter, AccessModifier? Setter, bool IsStatic, bool IsVirtual, bool IsAbstract)
{

}

public record MethodIntermed(string Name, MethodParameter[] Parameters, string ReturnType, AccessModifier AccessibilityModifier, bool IsStatic, bool IsOverridable, bool OverridesAnotherMethod, bool IsAbstract, bool IsFinal, bool IsAsync)
{
    public bool IsSealed => OverridesAnotherMethod && IsFinal;

    public bool HasVirtualKeyword => IsOverridable && !IsAbstract && !OverridesAnotherMethod;
}

public record MethodParameter(string Name, string Type);

public enum AccessModifier { Private, Protected, Internal, Public, ProtectedInternal, PrivateProtected, File }

internal static class AccessModifierExtensions
{
    internal static string GetString(this AccessModifier accessModifier)
    {
        return accessModifier switch
        {
            AccessModifier.PrivateProtected => "private protected",
            AccessModifier.ProtectedInternal => "internal protected",
            _ => accessModifier.ToString().ToLowerInvariant()
        };
    }
}
