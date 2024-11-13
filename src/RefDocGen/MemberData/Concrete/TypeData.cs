using RefDocGen.MemberData.Abstract;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.MemberData.Concrete;

/// <summary>
/// Represents data of a class.
/// </summary>
/// <param name="Constructors">Dictionary of constructors declared in the class; keys are the corresponding constructor IDs</param>
/// <param name="Fields">Dictionary of fields declared in the class; keys are the corresponding field IDs.</param>
/// <param name="Properties">Dictionary of properties declared in the class; keys are the corresponding property IDs.</param>
/// <param name="Methods">Dictionary of methods declared in the class; keys are the corresponding method IDs.</param>
internal record TypeData(
    Type Type,
    IReadOnlyDictionary<string, ConstructorData> Constructors,
    IReadOnlyDictionary<string, FieldData> Fields,
    IReadOnlyDictionary<string, PropertyData> Properties,
    IReadOnlyDictionary<string, MethodData> Methods) : TypeNameData(Type), ITypeData
{
    /// <inheritdoc/>
    public override string Id
    {
        get
        {
            string name = FullName;

            if (HasGenericParameters)
            {
                name = name + '`' + GenericParameters.Count;
            }

            return name.Replace('&', '@');
        }
    }

    /// <inheritdoc/>
    public XElement DocComment { get; internal set; } = XmlDocElementFactory.EmptySummary;

    /// <inheritdoc/>
    public AccessModifier AccessModifier => AccessModifierExtensions.GetAccessModifier(Type.IsNestedPrivate, Type.IsNestedFamily,
        Type.IsNestedAssembly || Type.IsNotPublic, Type.IsPublic || Type.IsNestedPublic, Type.IsNestedFamANDAssem, Type.IsNestedFamORAssem);

    /// <inheritdoc/>
    public bool IsAbstract => Type.IsAbstract;

    /// <inheritdoc/>
    public bool IsSealed => Type.IsSealed;

    /// <inheritdoc/>
    public bool IsStatic => IsAbstract && IsSealed;

    /// <inheritdoc/>
    public TypeKind Kind => Type.IsInterface ? TypeKind.Interface : Type.IsValueType ? TypeKind.ValueType : TypeKind.Class;

    /// <inheritdoc/>
    IReadOnlyList<IConstructorData> ITypeData.Constructors => Constructors.Values.ToList();

    /// <inheritdoc/>
    IReadOnlyList<IFieldData> ITypeData.Fields => Fields.Values.ToList();

    /// <inheritdoc/>
    IReadOnlyList<IMethodData> ITypeData.Methods => Methods.Values.ToList();

    /// <inheritdoc/>
    IReadOnlyList<IPropertyData> ITypeData.Properties => Properties.Values.ToList();
}
