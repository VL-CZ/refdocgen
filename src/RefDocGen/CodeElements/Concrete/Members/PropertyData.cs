using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types.Exception;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Tools;
using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Concrete.Members;

/// <summary>
/// Class representing data of a property.
/// </summary>
internal class PropertyData : MemberData, IPropertyData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyData"/> class.
    /// </summary>
    /// <param name="propertyInfo"><see cref="System.Reflection.PropertyInfo"/> object representing the property.</param>
    /// <param name="declaredTypeParameters">Collection of type parameters declared in the containing type; the keys represent type parameter names.</param>
    internal PropertyData(PropertyInfo propertyInfo, IReadOnlyDictionary<string, TypeParameterData> declaredTypeParameters) : base(propertyInfo)
    {
        PropertyInfo = propertyInfo;
        Type = propertyInfo.PropertyType.GetTypeNameData(declaredTypeParameters);

        Getter = PropertyInfo.GetMethod is not null
            ? new MethodData(PropertyInfo.GetMethod, declaredTypeParameters)
            : null;

        Setter = PropertyInfo.SetMethod is not null
            ? new MethodData(PropertyInfo.SetMethod, declaredTypeParameters)
            : null;
    }

    /// <inheritdoc/>
    public PropertyInfo PropertyInfo { get; }

    /// <inheritdoc/>
    public IMethodData? Getter { get; }

    /// <inheritdoc/>
    public IMethodData? Setter { get; }

    /// <inheritdoc/>
    public ITypeNameData Type { get; }

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
    public override bool IsStatic => Accessors.All(a => a.IsStatic);

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
    public override AccessModifier AccessModifier
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
    public XElement ValueDocComment { get; internal set; } = XmlDocElements.EmptySummary;

    /// <inheritdoc/>
    public IEnumerable<IExceptionDocumentation> Exceptions { get; internal set; } = [];
}

