using RefDocGen.Extensions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RefDocGen.Intermed;

public record ClassIntermed(string Name, AccessModifier AccessibilityModifier, FieldIntermed[] Fields, PropertyIntermed[] Properties, MethodIntermed[] Methods)
{
}

public class FieldIntermed
{
    public FieldIntermed(FieldInfo fieldInfo)
    {
        FieldInfo = fieldInfo;
    }

    public FieldInfo FieldInfo { get; }

    public string Name => FieldInfo.Name;

    public string Type => FieldInfo.FieldType.Name;

    public AccessModifier AccessibilityModifier => FieldInfo.GetAccessModifier();

    public bool IsStatic => FieldInfo.IsStatic;

    public bool IsReadonly => FieldInfo.IsInitOnly;

    public bool IsConstant => FieldInfo.IsLiteral;
}

//public record PropertyAccessorIntermed(AccessibilityModifier AccessibilityModifier);

// public record MethodBase(string Name, bool IsStatic, bool IsOverridable, bool OverridesAnotherMethod, bool IsAbstract, bool IsFinal);

public record PropertyIntermed
{
    public PropertyIntermed(PropertyInfo propertyInfo)
    {
        PropertyInfo = propertyInfo;
        Getter = PropertyInfo.GetMethod is not null ? new MethodIntermed(PropertyInfo.GetMethod) : null;
        Setter = PropertyInfo.SetMethod is not null ? new MethodIntermed(PropertyInfo.SetMethod) : null;
    }

    public PropertyInfo PropertyInfo { get; }

    public MethodIntermed? Getter { get; }

    public MethodIntermed? Setter { get; }

    public string Name => PropertyInfo.Name;

    public string Type => PropertyInfo.PropertyType.Name;

    public IEnumerable<MethodIntermed> Accessors
    {
        get
        {
            if (Getter is not null)
            {
                yield return Getter;
            }
            if (Setter is not null)
            {
                yield return Setter;
            }
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

    public AccessModifier AccessModifier
    {
        get
        {
            var modifiers = Accessors.Select(a => a.AccessModifier);
            return AccessModifierExtensions.GetTheLeastRestrictive(modifiers);
        }
    }
}

public class MethodIntermed
{
    public MethodIntermed(MethodInfo methodInfo)
    {
        MethodInfo = methodInfo;
    }

    public MethodInfo MethodInfo { get; }

    public string Name => MethodInfo.Name;

    public string ReturnType => MethodInfo.ReturnType.Name;

    public AccessModifier AccessModifier => MethodInfo.GetAccessModifier();

    public bool IsStatic => MethodInfo.IsStatic;

    public bool IsOverridable => MethodInfo.IsVirtual && !MethodInfo.IsFinal;

    public bool OverridesAnotherMethod => !MethodInfo.Equals(MethodInfo.GetBaseDefinition());

    public bool IsAbstract => MethodInfo.IsAbstract;

    public bool IsFinal => MethodInfo.IsFinal;

    public bool IsAsync => MethodInfo.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null;

    public bool IsSealed => OverridesAnotherMethod && IsFinal;

    public bool HasVirtualKeyword => IsOverridable && !IsAbstract && !OverridesAnotherMethod;

    public IEnumerable<MethodParameter> GetParameters()
    {
        return MethodInfo.GetParameters().Select(p => new MethodParameter(p));
    }
}

public class MethodParameter
{
    public MethodParameter(ParameterInfo parameterInfo)
    {
        ParameterInfo = parameterInfo;
    }

    public ParameterInfo ParameterInfo { get; }

    public string Name => ParameterInfo.Name;

    public string Type => ParameterInfo.ParameterType.Name;
}


public enum AccessModifier { Private, PrivateProtected, Protected, Internal, ProtectedInternal, Public } // sorted from the MOST restrictive

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

    internal static AccessModifier GetTheLeastRestrictive(IEnumerable<AccessModifier> accessModifiers)
    {
        int minIntegerValue = accessModifiers.Max(a => (int)a);
        return (AccessModifier)minIntegerValue;
    }
}
