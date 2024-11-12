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
internal record TypeData(Type Type, Dictionary<string, ConstructorData> Constructors,
    Dictionary<string, FieldData> Fields, Dictionary<string, PropertyData> Properties, Dictionary<string, MethodData> Methods) : TypeNameData(Type), ITypeData
{
    /// <inheritdoc/>
    public XElement DocComment { get; internal set; } = XmlDocElementFactory.EmptySummary;

    /// <inheritdoc/>
    public AccessModifier AccessModifier => AccessModifierExtensions.GetAccessModifier(Type.IsNestedPrivate, Type.IsNestedFamily,
        Type.IsNestedAssembly || Type.IsNotPublic, Type.IsPublic || Type.IsNestedPublic, Type.IsNestedFamANDAssem, Type.IsNestedFamORAssem);

    public bool IsAbstract => Type.IsAbstract;

    public bool IsInterface => Type.IsInterface;

    public bool IsValueType => Type.IsValueType;

    public bool IsSealed => Type.IsSealed;

    public bool IsStatic => IsAbstract && IsSealed;

    /// <inheritdoc/>
    IReadOnlyList<IConstructorData> ITypeData.Constructors => Constructors.Values.ToList();

    /// <inheritdoc/>
    IReadOnlyList<IFieldData> ITypeData.Fields => Fields.Values.ToList();

    /// <inheritdoc/>
    IReadOnlyList<IMethodData> ITypeData.Methods => Methods.Values.ToList();

    /// <inheritdoc/>
    IReadOnlyList<IPropertyData> ITypeData.Properties => Properties.Values.ToList();
}
