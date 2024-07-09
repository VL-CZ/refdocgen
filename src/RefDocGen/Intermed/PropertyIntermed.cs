using System.Reflection;

namespace RefDocGen.Intermed;

// public record MethodBase(string Name, bool IsStatic, bool IsOverridable, bool OverridesAnotherMethod, bool IsAbstract, bool IsFinal);

public class PropertyIntermed
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

    public bool HasGetter => Getter is not null;
}
