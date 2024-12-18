using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types.Enum;

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
internal class ObjectTypeData : TypeDeclaration, IObjectTypeData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectTypeData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    /// <param name="constructors">Dictionary of constructors declared in the class; keys are the corresponding constructor IDs</param>
    /// <param name="fields">Dictionary of fields declared in the class; keys are the corresponding field IDs.</param>
    /// <param name="properties">Dictionary of properties declared in the class; keys are the corresponding property IDs.</param>
    /// <param name="methods">Dictionary of methods declared in the class; keys are the corresponding method IDs.</param>
    /// <param name="operators">Dictionary of operators declared in the class; keys are the corresponding operator IDs.</param>
    /// <param name="indexers">Dictionary of indexers declared in the class; keys are the corresponding operator IDs.</param>
    /// <param name="typeParameterDeclarations">Collection of type parameters declared in this type; the keys represent type parameter names.</param>
    public ObjectTypeData(
        Type type,
        IReadOnlyDictionary<string, ConstructorData> constructors,
        IReadOnlyDictionary<string, FieldData> fields,
        IReadOnlyDictionary<string, PropertyData> properties,
        IReadOnlyDictionary<string, MethodData> methods,
        IReadOnlyDictionary<string, OperatorData> operators,
        IReadOnlyDictionary<string, IndexerData> indexers,
        IReadOnlyDictionary<string, TypeParameterData> typeParameterDeclarations)
        : base(type, typeParameterDeclarations)
    {
        Constructors = constructors;
        Fields = fields;
        Properties = properties;
        Methods = methods;
        Operators = operators;
        Indexers = indexers;
    }

    /// <summary>
    /// Dictionary of constructors declared in the type; keys are the corresponding constructor IDs.
    /// </summary>
    public IReadOnlyDictionary<string, ConstructorData> Constructors { get; }

    /// <summary>
    /// Dictionary of fields declared in the type; keys are the corresponding field IDs.
    /// </summary>
    public IReadOnlyDictionary<string, FieldData> Fields { get; }

    /// <summary>
    /// Dictionary of properties declared in the type; keys are the corresponding property IDs.
    /// </summary>
    public IReadOnlyDictionary<string, PropertyData> Properties { get; }

    /// <summary>
    /// Dictionary of methods declared in the type; keys are the corresponding method IDs.
    /// </summary>
    public IReadOnlyDictionary<string, MethodData> Methods { get; }

    /// <summary>
    /// Collection of operators declared in the type; keys are the corresponding operator IDs.
    /// </summary>
    public IReadOnlyDictionary<string, OperatorData> Operators { get; }

    /// <summary>
    /// Collection of indexers declared in the type; keys are the corresponding operator IDs.
    /// </summary>
    public IReadOnlyDictionary<string, IndexerData> Indexers { get; }

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
    IReadOnlyList<IConstructorData> IObjectTypeData.Constructors => Constructors.Values.ToList();

    /// <inheritdoc/>
    IReadOnlyList<IFieldData> IObjectTypeData.Fields => Fields.Values.ToList();

    /// <inheritdoc/>
    IReadOnlyList<IMethodData> IObjectTypeData.Methods => Methods.Values.ToList();

    /// <inheritdoc/>
    IReadOnlyList<IPropertyData> IObjectTypeData.Properties => Properties.Values.ToList();

    /// <inheritdoc/>
    IReadOnlyList<IOperatorData> IObjectTypeData.Operators => Operators.Values.ToList();

    /// <inheritdoc/>
    IReadOnlyList<IIndexerData> IObjectTypeData.Indexers => Indexers.Values.ToList();

    // TODO: performance
    internal IReadOnlyDictionary<string, MemberData> AllMembers => ((IEnumerable<MemberData>)Constructors.Values)
        .Concat(Fields.Values)
        .Concat(Methods.Values)
        .Concat(Properties.Values)
        .Concat(Operators.Values)
        .Concat(Indexers.Values)
        .ToDictionary(m => m.Id);
}
