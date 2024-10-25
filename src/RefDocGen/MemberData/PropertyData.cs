using RefDocGen.MemberData.Abstract;
using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.MemberData;

/// <summary>
/// Represents data of a property.
/// </summary>
public record PropertyData : ICallableMemberData
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

    /// <summary>
    /// <see cref="System.Reflection.PropertyInfo"/> object representing the property.
    /// </summary>
    public PropertyInfo PropertyInfo { get; }

    /// <summary>
    /// Gets the getter method represented as a <see cref="MethodData"/> object.
    /// <para>If the getter is missing, <c>null</c> is returned</para>
    /// </summary>
    public MethodData? Getter { get; }

    /// <summary>
    /// Gets the setter method represented as a <see cref="MethodData"/> object.
    /// <para>If the setter is missing, <c>null</c> is returned</para>
    /// </summary>
    public MethodData? Setter { get; }

    /// <inheritdoc/>
    public string Id => Name;

    /// <inheritdoc/>
    public string Name => PropertyInfo.Name;

    /// <summary>
    /// Type of the property.
    /// </summary>
    public string Type => PropertyInfo.PropertyType.Name;

    /// <summary>
    /// Gets the declared accessors (getter and setter) of the property represented as <see cref="MethodData"/> objects.
    /// <para>If one of the accessors is missing, enumerable with a single <see cref="MethodData"/> object is returned.</para>
    /// </summary>
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

    /// <summary>
    /// Gets the access modifier of the getter.
    /// <para>If the getter is missing, <c>null</c> is returned</para>
    /// </summary>
    public AccessModifier? GetterAccessModifier => Getter?.AccessModifier;

    /// <summary>
    /// Gets the access modifier of the setter.
    /// <para>If the setter is missing, <c>null</c> is returned</para>
    /// </summary>
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

    /// <summary>
    /// Gets the XMl doc comment for this property.
    /// </summary>
    public XElement DocComment { get; init; } = XmlDocElementFactory.EmptySummary;

}
