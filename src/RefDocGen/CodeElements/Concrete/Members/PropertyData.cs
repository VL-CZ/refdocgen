using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types.Attribute;
using RefDocGen.CodeElements.Abstract.Types.Exception;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Tools;
using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Runtime.CompilerServices;
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
    /// <param name="availableTypeParameters">Collection of type parameters declared in the containing type; the keys represent type parameter names.</param>
    /// <param name="containingType">Type that contains the member.</param>
    /// <param name="attributes">Collection of attributes applied to the property.</param>
    /// <param name="getterMethod">
    /// <inheritdoc cref="Getter"/>
    /// </param>
    /// <param name="setterMethod">
    /// <inheritdoc cref="Setter"/>
    /// </param>
    internal PropertyData(
        PropertyInfo propertyInfo,
        MethodData? getterMethod,
        MethodData? setterMethod,
        TypeDeclaration containingType,
        IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters,
        IReadOnlyList<IAttributeData> attributes) : base(propertyInfo, containingType, attributes)
    {
        PropertyInfo = propertyInfo;
        Type = propertyInfo.PropertyType.GetTypeNameData(availableTypeParameters);
        ExplicitInterfaceType = Tools.ExplicitInterfaceType.Of(this);

        (Getter, Setter) = (getterMethod, setterMethod);

        if (Setter is not null) // check if the setter is 'init' only
        {
            var modifiers = Setter.MethodInfo.ReturnParameter.GetRequiredCustomModifiers();
            IsSetterInitOnly = modifiers.Any(t => t.FullName == initializerAttributeType);
        }
    }

    /// <summary>
    /// Fully qualified name of the attribute that marks 'init' setters.
    /// </summary>
    /// <seealso href="https://alistairevans.co.uk/2020/11/01/detecting-init-only-properties-with-reflection-in-c-9/"/>
    private const string initializerAttributeType = "System.Runtime.CompilerServices.IsExternalInit";

    /// <inheritdoc/>
    public override string Id => MemberId.Of(this);

    /// <inheritdoc/>
    public override string Name => MemberName.Of(this);

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
            return AccessModifierHelper.GetTheLeastRestrictive(modifiers);
        }
    }

    /// <inheritdoc/>
    public bool IsConstant => false;

    /// <inheritdoc/>
    public bool IsReadonly => Setter is null;

    /// <inheritdoc/>
    public XElement ValueDocComment { get; internal set; } = XmlDocElements.EmptySummary;

    /// <inheritdoc/>
    public IEnumerable<IExceptionDocumentation> DocumentedExceptions { get; internal set; } = [];

    /// <inheritdoc/>
    public bool IsExplicitImplementation => ExplicitInterfaceType is not null;

    /// <inheritdoc/>
    public virtual ITypeNameData? ExplicitInterfaceType { get; }

    /// <inheritdoc/>
    public object? ConstantValue => DBNull.Value;

    /// <inheritdoc/>
    public bool IsRequired => PropertyInfo.IsDefined(typeof(RequiredMemberAttribute), false);

    /// <inheritdoc/>
    public bool IsSetterInitOnly { get; }
}
