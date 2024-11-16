using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Concrete.Types;

/// <summary>
/// Class representing data of a type, including its members.
/// <para>
/// Typically, only the types declared in the assemblies we analyze, are represented by this class.
/// </para>
/// </summary>
internal class TypeData : TypeNameData, ITypeData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeData"/> class.
    /// </summary>
    /// <param name="type"><see cref="System.Type"/> object representing the type.</param>
    /// <param name="constructors">Dictionary of constructors declared in the class; keys are the corresponding constructor IDs</param>
    /// <param name="fields">Dictionary of fields declared in the class; keys are the corresponding field IDs.</param>
    /// <param name="properties">Dictionary of properties declared in the class; keys are the corresponding property IDs.</param>
    /// <param name="methods">Dictionary of methods declared in the class; keys are the corresponding method IDs.</param>
    /// <param name="typeParameterDeclarations">Collection of type parameters declared in this type; the keys represent type parameter names.</param>
    public TypeData(
        Type type,
        IReadOnlyDictionary<string, ConstructorData> constructors,
        IReadOnlyDictionary<string, FieldData> fields,
        IReadOnlyDictionary<string, PropertyData> properties,
        IReadOnlyDictionary<string, MethodData> methods,
        IReadOnlyDictionary<string, TypeParameterDeclaration> typeParameterDeclarations)
        : base(type, typeParameterDeclarations)
    {
        Type = type;
        Constructors = constructors;
        Fields = fields;
        Properties = properties;
        Methods = methods;
        TypeParameterDeclarations = typeParameterDeclarations;
    }

    /// <summary>
    /// <see cref="System.Type"/> object representing the type.
    /// </summary>
    public Type Type { get; }

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
