namespace RefDocGen.Intermed;

public record ClassIntermed(string Name, AccessModifier AccessibilityModifier, FieldIntermed[] Fields, PropertyIntermed[] Properties, MethodIntermed[] Methods)
{
}

public record FieldIntermed(string Name, string Type, AccessModifier AccessibilityModifier, bool IsStatic, bool IsReadonly, bool IsConstant)
{
}

//public record PropertyAccessorIntermed(AccessibilityModifier AccessibilityModifier);

public record MethodBase(string Name, bool IsStatic, bool IsOverridable, bool OverridesAnotherMethod, bool IsAbstract, bool IsFinal);

public record PropertyIntermed(string Name, string Type, MethodIntermed? Getter, MethodIntermed? Setter)
{
    public IEnumerable<MethodIntermed> Accessors
    {
        get
        {
            var accessors = new List<MethodIntermed>();

            if (Getter is not null)
            {
                accessors.Add(Getter);
            }
            if (Setter is not null)
            {
                accessors.Add(Setter);
            }

            return accessors;
        }
    }

    public bool IsStatic => Accessors.All(a => a.IsStatic);

    public bool IsOverridable => Accessors.All(a => a.IsOverridable);

    public bool OverridesAnotherProperty => Accessors.All(a => a.OverridesAnotherMethod);

    public bool IsAbstract => Accessors.All(a => a.IsAbstract);

    public bool IsFinal => Accessors.All(a => a.IsFinal);

    public bool IsSealed => Accessors.All(a => a.IsSealed);

    public bool HasVirtualKeyword => Accessors.All(a => a.HasVirtualKeyword);

    public AccessModifier? GetterAccessModifier => Getter?.AccessModifier;

    public AccessModifier? SetterAccessModifier => Setter?.AccessModifier;

    public AccessModifier AccessModifier => AccessModifierExtensions.GetTheLeastRestrictive(Accessors.Select(a => a.AccessModifier).ToArray());
}

public record MethodIntermed(string Name, MethodParameter[] Parameters, string ReturnType, AccessModifier AccessModifier, bool IsStatic, bool IsOverridable, bool OverridesAnotherMethod, bool IsAbstract, bool IsFinal, bool IsAsync)
{
    public bool IsSealed => OverridesAnotherMethod && IsFinal;

    public bool HasVirtualKeyword => IsOverridable && !IsAbstract && !OverridesAnotherMethod;
}

public record MethodParameter(string Name, string Type);

public enum AccessModifier { Private, PrivateProtected, Protected, Internal, ProtectedInternal, Public } // sorted from the LEAST restrictive

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

    internal static AccessModifier GetTheLeastRestrictive(params AccessModifier[] accessModifiers)
    {
        int minIntegerValue = accessModifiers.Max(a => (int)a);
        return (AccessModifier)minIntegerValue;
    }
}
