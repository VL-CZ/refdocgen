using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Concrete.Types;

/// <summary>
/// Represents data of a type.
/// </summary>
/// <param name="Type"><see cref="System.Type"/> object representing the type.</param>
/// <param name="Constructors">Dictionary of constructors declared in the class; keys are the corresponding constructor IDs</param>
/// <param name="Fields">Dictionary of fields declared in the class; keys are the corresponding field IDs.</param>
/// <param name="Properties">Dictionary of properties declared in the class; keys are the corresponding property IDs.</param>
/// <param name="Methods">Dictionary of methods declared in the class; keys are the corresponding method IDs.</param>
/// <param name="TypeParameterDeclarations">Collection of type parameters declared in this type; the keys represent type parameter names.</param>
internal record TypeData(
    Type Type,
    IReadOnlyDictionary<string, ConstructorData> Constructors,
    IReadOnlyDictionary<string, FieldData> Fields,
    IReadOnlyDictionary<string, PropertyData> Properties,
    IReadOnlyDictionary<string, MethodData> Methods,
    IReadOnlyDictionary<string, TypeParameterDeclaration> TypeParameterDeclarations) : TypeNameData(Type, TypeParameterDeclarations), ITypeData
{
    /// <inheritdoc/>
    public override string Id
    {
        get
        {
            string name = FullName;

            if (HasTypeParameters)
            {
                name = name + '`' + TypeParameters.Count;
            }

            return name;
        }
    }

    /// <inheritdoc/>
    public XElement DocComment { get; internal set; } = XmlDocElementFactory.EmptySummary;

    /// <inheritdoc/>
    public AccessModifier AccessModifier =>
        AccessModifierExtensions.GetAccessModifier(
            Type.IsNestedPrivate,
            Type.IsNestedFamily,
            Type.IsNestedAssembly || Type.IsNotPublic,
            Type.IsPublic || Type.IsNestedPublic,
            Type.IsNestedFamANDAssem,
            Type.IsNestedFamORAssem);

    /// <inheritdoc/>
    public bool IsAbstract => Type.IsAbstract;

    /// <inheritdoc/>
    public bool IsSealed => Type.IsSealed;

    /// <inheritdoc/>
    public TypeKind Kind => Type.IsInterface
        ? TypeKind.Interface
        : Type.IsValueType
            ? TypeKind.ValueType
            : TypeKind.Class;

    /// <inheritdoc/>
    IReadOnlyList<IConstructorData> ITypeData.Constructors => Constructors.Values.ToList();

    /// <inheritdoc/>
    IReadOnlyList<IFieldData> ITypeData.Fields => Fields.Values.ToList();

    /// <inheritdoc/>
    IReadOnlyList<IMethodData> ITypeData.Methods => Methods.Values.ToList();

    /// <inheritdoc/>
    IReadOnlyList<IPropertyData> ITypeData.Properties => Properties.Values.ToList();
}
