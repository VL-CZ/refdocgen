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
    /// Indicates whether the members have already been added.
    /// </summary>
    private bool membersAdded;

    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectTypeData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    /// <param name="typeParameterDeclarations">Collection of type parameters declared in this type; the keys represent type parameter names.</param>
    public ObjectTypeData(Type type, IReadOnlyDictionary<string, TypeParameterData> typeParameterDeclarations)
        : base(type, typeParameterDeclarations)
    { }

    /// <summary>
    /// Dictionary of constructors declared in the type; keys are the corresponding constructor IDs.
    /// </summary>
    public IReadOnlyDictionary<string, ConstructorData> Constructors { get; private set; } = new Dictionary<string, ConstructorData>();

    /// <summary>
    /// Dictionary of fields declared in the type; keys are the corresponding field IDs.
    /// </summary>
    public IReadOnlyDictionary<string, FieldData> Fields { get; private set; } = new Dictionary<string, FieldData>();

    /// <summary>
    /// Dictionary of properties declared in the type; keys are the corresponding property IDs.
    /// </summary>
    public IReadOnlyDictionary<string, PropertyData> Properties { get; private set; } = new Dictionary<string, PropertyData>();

    /// <summary>
    /// Dictionary of methods declared in the type; keys are the corresponding method IDs.
    /// </summary>
    public IReadOnlyDictionary<string, MethodData> Methods { get; private set; } = new Dictionary<string, MethodData>();

    /// <summary>
    /// Dictionary of operators declared in the type; keys are the corresponding operator IDs.
    /// </summary>
    public IReadOnlyDictionary<string, OperatorData> Operators { get; private set; } = new Dictionary<string, OperatorData>();

    /// <summary>
    /// Dictionary of indexers declared in the type; keys are the corresponding operator IDs.
    /// </summary>
    public IReadOnlyDictionary<string, IndexerData> Indexers { get; private set; } = new Dictionary<string, IndexerData>();

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
    IEnumerable<IConstructorData> IObjectTypeData.Constructors => Constructors.Values;

    /// <inheritdoc/>
    IEnumerable<IFieldData> IObjectTypeData.Fields => Fields.Values;

    /// <inheritdoc/>
    IEnumerable<IMethodData> IObjectTypeData.Methods => Methods.Values;

    /// <inheritdoc/>
    IEnumerable<IPropertyData> IObjectTypeData.Properties => Properties.Values;

    /// <inheritdoc/>
    IEnumerable<IOperatorData> IObjectTypeData.Operators => Operators.Values;

    /// <inheritdoc/>
    IEnumerable<IIndexerData> IObjectTypeData.Indexers => Indexers.Values;

    /// <inheritdoc/>
    internal override IReadOnlyDictionary<string, MemberData> AllMembers { get; private protected set; } = new Dictionary<string, MemberData>();

    internal void AddMembers(
        IReadOnlyDictionary<string, ConstructorData> constructors,
        IReadOnlyDictionary<string, FieldData> fields,
        IReadOnlyDictionary<string, PropertyData> properties,
        IReadOnlyDictionary<string, MethodData> methods,
        IReadOnlyDictionary<string, OperatorData> operators,
        IReadOnlyDictionary<string, IndexerData> indexers)
    {
        if (membersAdded)
        {
            throw new InvalidOperationException($"The members have been already added to {Id} type.");
        }

        Constructors = constructors;
        Fields = fields;
        Properties = properties;
        Methods = methods;
        Operators = operators;
        Indexers = indexers;

        AllMembers = ((IEnumerable<MemberData>)Constructors.Values)
            .Concat(Fields.Values)
            .Concat(Methods.Values)
            .Concat(Properties.Values)
            .Concat(Operators.Values)
            .Concat(Indexers.Values)
            .ToDictionary(m => m.Id);

        membersAdded = true;
    }
}
