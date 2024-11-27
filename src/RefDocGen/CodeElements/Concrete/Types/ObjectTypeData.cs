using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types.Enum;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Concrete.Types;

/// <summary>
/// Class representing data of a value, reference or interface type, including its members.
/// <para>
/// Typically, only the types declared in the assemblies we analyze, are represented by this class.
/// </para>
/// <para>
/// Note: This class doesn't represent enum types - see <see cref="EnumTypeData"/>.
/// </para>
/// </summary>
internal class ObjectTypeData : TypeNameData, IObjectTypeData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectTypeData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    /// <param name="constructors">Dictionary of constructors declared in the class; keys are the corresponding constructor IDs</param>
    /// <param name="fields">Dictionary of fields declared in the class; keys are the corresponding field IDs.</param>
    /// <param name="properties">Dictionary of properties declared in the class; keys are the corresponding property IDs.</param>
    /// <param name="methods">Dictionary of methods declared in the class; keys are the corresponding method IDs.</param>
    /// <param name="typeParameterDeclarations">Collection of type parameters declared in this type; the keys represent type parameter names.</param>
    public ObjectTypeData(
        Type type,
        IReadOnlyDictionary<string, ConstructorData> constructors,
        IReadOnlyDictionary<string, FieldData> fields,
        IReadOnlyDictionary<string, PropertyData> properties,
        IReadOnlyDictionary<string, MethodData> methods,
        IReadOnlyDictionary<string, TypeParameterDeclaration> typeParameterDeclarations)
        : base(type, typeParameterDeclarations)
    {
        Constructors = constructors;
        Fields = fields;
        Properties = properties;
        Methods = methods;
        TypeParameterDeclarations = typeParameterDeclarations;

        BaseType = type.BaseType is not null
            ? new TypeNameData(type.BaseType)
            : null;

        Interfaces = type.GetInterfaces()
            .Select(i => new TypeNameData(i))
            .ToArray();
    }

    /// <summary>
    /// Dictionary of constructors declared in the class; keys are the corresponding constructor IDs.
    /// </summary>
    public IReadOnlyDictionary<string, ConstructorData> Constructors { get; }

    /// <summary>
    /// Dictionary of fields declared in the class; keys are the corresponding field IDs.
    /// </summary>
    public IReadOnlyDictionary<string, FieldData> Fields { get; }

    /// <summary>
    /// Dictionary of properties declared in the class; keys are the corresponding property IDs.
    /// </summary>
    public IReadOnlyDictionary<string, PropertyData> Properties { get; }

    /// <summary>
    /// Dictionary of methods declared in the class; keys are the corresponding method IDs.
    /// </summary>
    public IReadOnlyDictionary<string, MethodData> Methods { get; }

    /// <summary>
    /// Collection of type parameters declared in this type; the keys represent type parameter names.
    /// </summary>
    public IReadOnlyDictionary<string, TypeParameterDeclaration> TypeParameterDeclarations { get; }

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
    public XElement DocComment { get; internal set; } = XmlDocElements.EmptySummary;

    /// <inheritdoc/>
    public AccessModifier AccessModifier =>
        AccessModifierExtensions.GetAccessModifier(
            TypeObject.IsNestedPrivate,
            TypeObject.IsNestedFamily,
            TypeObject.IsNestedAssembly || TypeObject.IsNotPublic,
            TypeObject.IsPublic || TypeObject.IsNestedPublic,
            TypeObject.IsNestedFamANDAssem,
            TypeObject.IsNestedFamORAssem);

    /// <inheritdoc/>
    public bool IsAbstract => TypeObject.IsAbstract;

    /// <inheritdoc/>
    public bool IsSealed => TypeObject.IsSealed;

    /// <inheritdoc/>
    public TypeKind Kind => TypeObject.IsInterface
        ? TypeKind.Interface
        : TypeObject.IsValueType
            ? TypeKind.ValueType
            : TypeKind.Class;

    /// <inheritdoc/>
    public ITypeNameData? BaseType { get; }

    /// <inheritdoc/>
    public IReadOnlyList<ITypeNameData> Interfaces { get; }

    /// <inheritdoc/>
    IReadOnlyList<IConstructorData> IObjectTypeData.Constructors => Constructors.Values.ToList();

    /// <inheritdoc/>
    IReadOnlyList<IFieldData> IObjectTypeData.Fields => Fields.Values.ToList();

    /// <inheritdoc/>
    IReadOnlyList<IMethodData> IObjectTypeData.Methods => Methods.Values.ToList();

    /// <inheritdoc/>
    IReadOnlyList<IPropertyData> IObjectTypeData.Properties => Properties.Values.ToList();

    /// <inheritdoc/>
    IReadOnlyList<ITypeParameterDeclaration> IObjectTypeData.TypeParameterDeclarations =>
        TypeParameterDeclarations.Values.OrderBy(t => t.Index).ToList();
}