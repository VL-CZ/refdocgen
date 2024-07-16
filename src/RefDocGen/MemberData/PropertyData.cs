using RefDocGen.DocExtraction;
using RefDocGen.MemberData.Interfaces;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.MemberData;

public record PropertyData : ICallableMember
{
    public PropertyData(PropertyInfo propertyInfo)
    {
        PropertyInfo = propertyInfo;
        Getter = PropertyInfo.GetMethod is not null ? new MethodData(PropertyInfo.GetMethod) : null;
        Setter = PropertyInfo.SetMethod is not null ? new MethodData(PropertyInfo.SetMethod) : null;
    }

    public PropertyInfo PropertyInfo { get; }

    public MethodData? Getter { get; }

    public MethodData? Setter { get; }

    public string Name => PropertyInfo.Name;

    public string Type => PropertyInfo.PropertyType.Name;

    public IEnumerable<MethodData> Accessors
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

    public bool OverridesAnotherMember => Accessors.All(a => a.OverridesAnotherMember);

    public bool IsAbstract => Accessors.All(a => a.IsAbstract);

    public bool IsFinal => Accessors.All(a => a.IsFinal);

    public bool IsSealed => Accessors.All(a => a.IsSealed);

    public bool IsAsync => false;

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

    public XElement DocComment { get; init; } = DocCommentTools.Empty;

}
