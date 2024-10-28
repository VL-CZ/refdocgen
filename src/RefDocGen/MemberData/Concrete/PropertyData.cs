using RefDocGen.MemberData.Abstract;
using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.MemberData.Concrete;

/// <summary>
/// Class representing data of a property.
/// </summary>
internal class PropertyData : IPropertyData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyData"/> class.
    /// </summary>
    /// <param name="propertyInfo"><see cref="System.Reflection.PropertyInfo"/> object representing the property.</param>
    public PropertyData(PropertyInfo propertyInfo)
    {
        PropertyInfo = propertyInfo;
        Getter = PropertyInfo.GetMethod is not null ? new MethodData(PropertyInfo.GetMethod) : null;
        Setter = PropertyInfo.SetMethod is not null ? new MethodData(PropertyInfo.SetMethod) : null;
    }

    /// <inheritdoc/>
    public PropertyInfo PropertyInfo { get; }

    /// <inheritdoc/>
    public IMethodData? Getter { get; }

    /// <inheritdoc/>
    public IMethodData? Setter { get; }

    /// <inheritdoc/>
    public string Id => Name;

    /// <inheritdoc/>
    public string Name => PropertyInfo.Name;

    /// <inheritdoc/>
    public string Type => PropertyInfo.PropertyType.Name;

    /// <inheritdoc/>
    public IEnumerable<IMethodData> Accessors
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

    /// <inheritdoc/>
    public bool IsStatic => Accessors.All(a => a.IsStatic);

    /// <inheritdoc/>
    public bool IsOverridable => Accessors.All(a => a.IsOverridable);

    /// <inheritdoc/>
    public bool OverridesAnotherMember => Accessors.All(a => a.OverridesAnotherMember);

    /// <inheritdoc/>
    public bool IsAbstract => Accessors.All(a => a.IsAbstract);

    /// <inheritdoc/>
    public bool IsFinal => Accessors.All(a => a.IsFinal);

    /// <inheritdoc/>
    public bool IsSealed => Accessors.All(a => a.IsSealed);

    /// <inheritdoc/>
    public bool IsVirtual => Accessors.All(a => a.IsVirtual);

    /// <inheritdoc/>
    public bool IsAsync => false;

    /// <inheritdoc/>
    public AccessModifier? GetterAccessModifier => Getter?.AccessModifier;

    /// <inheritdoc/>
    public AccessModifier? SetterAccessModifier => Setter?.AccessModifier;

    /// <inheritdoc/>
    public AccessModifier AccessModifier
    {
        get
        {
            var modifiers = Accessors.Select(a => a.AccessModifier);
            return AccessModifierExtensions.GetTheLeastRestrictive(modifiers);
        }
    }

    /// <inheritdoc/>
    public bool IsConstant => false;

    /// <inheritdoc/>
    public bool IsReadonly => Setter is null;

    /// <inheritdoc/>
    public XElement DocComment { get; internal set; } = XmlDocElementFactory.EmptySummary;
}

