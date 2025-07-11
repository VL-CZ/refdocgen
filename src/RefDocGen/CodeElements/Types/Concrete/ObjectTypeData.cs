using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Members.Concrete;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Attribute;
using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.CodeElements.Types.Abstract.Enum;
using RefDocGen.CodeElements.Types.Concrete.Enum;

namespace RefDocGen.CodeElements.Types.Concrete;

/// <summary>
/// Class representing data of a value, reference or interface type, including its members.
/// <para>
/// Typically, only the types contained in the assemblies we analyze, are represented by this class.
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
    /// <param name="attributes">Collection of attributes applied to the type.</param>
    public ObjectTypeData(Type type, IReadOnlyDictionary<string, TypeParameterData> typeParameterDeclarations, IReadOnlyList<IAttributeData> attributes)
        : base(type, typeParameterDeclarations, attributes)
    { }

    /// <summary>
    /// Dictionary of constructors contained in the type; keys are the corresponding constructor IDs.
    /// </summary>
    public IReadOnlyDictionary<string, ConstructorData> Constructors { get; private set; } = new Dictionary<string, ConstructorData>();

    /// <summary>
    /// Dictionary of fields contained in the type; keys are the corresponding field IDs.
    /// </summary>
    public IReadOnlyDictionary<string, FieldData> Fields { get; private set; } = new Dictionary<string, FieldData>();

    /// <summary>
    /// Dictionary of properties contained in the type; keys are the corresponding property IDs.
    /// </summary>
    public IReadOnlyDictionary<string, PropertyData> Properties { get; private set; } = new Dictionary<string, PropertyData>();

    /// <summary>
    /// Dictionary of methods contained in the type; keys are the corresponding method IDs.
    /// </summary>
    public IReadOnlyDictionary<string, MethodData> Methods { get; private set; } = new Dictionary<string, MethodData>();

    /// <summary>
    /// Dictionary of operators contained in the type; keys are the corresponding operator IDs.
    /// </summary>
    public IReadOnlyDictionary<string, OperatorData> Operators { get; private set; } = new Dictionary<string, OperatorData>();

    /// <summary>
    /// Dictionary of indexers contained in the type; keys are the corresponding operator IDs.
    /// </summary>
    public IReadOnlyDictionary<string, IndexerData> Indexers { get; private set; } = new Dictionary<string, IndexerData>();

    /// <summary>
    /// Dictionary of events contained in the type; keys are the corresponding operator IDs.
    /// </summary>
    public IReadOnlyDictionary<string, EventData> Events { get; private set; } = new Dictionary<string, EventData>();

    /// <inheritdoc/>
    public bool IsAbstract => TypeObject.IsAbstract;

    /// <inheritdoc/>
    public bool IsSealed => TypeObject.IsSealed;

    /// <inheritdoc/>
    public ObjectTypeKind Kind => TypeObject.IsInterface
        ? ObjectTypeKind.Interface
        : TypeObject.IsValueType
            ? ObjectTypeKind.ValueType
            : ObjectTypeKind.Class;

    /// <inheritdoc/>
    public bool IsByRefLike => TypeObject.IsByRefLike;

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
    IEnumerable<IEventData> IObjectTypeData.Events => Events.Values;

    /// <inheritdoc/>
    internal override IReadOnlyDictionary<string, MemberData> AllMembers { get; private protected set; } = new Dictionary<string, MemberData>();

    /// <inheritdoc/>
    public IEnumerable<IObjectTypeData> NestedObjectTypes { get; private set; } = [];

    /// <inheritdoc/>
    public IEnumerable<IDelegateTypeData> NestedDelegates { get; private set; } = [];

    /// <inheritdoc/>
    public IEnumerable<IEnumTypeData> NestedEnums { get; private set; } = [];

    /// <inheritdoc/>
    public override bool IsInterface => Kind == ObjectTypeKind.Interface;

    /// <summary>
    /// Adds the members to the type.
    /// </summary>
    /// <param name="constructors">
    /// <inheritdoc cref="Constructors"/>
    /// </param>
    /// <param name="fields">
    /// <inheritdoc cref="Fields"/>
    /// </param>
    /// <param name="properties">
    /// <inheritdoc cref="Properties"/>
    /// </param>
    /// <param name="methods">
    /// <inheritdoc cref="Methods"/>
    /// </param>
    /// <param name="operators">
    /// <inheritdoc cref="Operators"/>
    /// </param>
    /// <param name="indexers">
    /// <inheritdoc cref="Indexers"/>
    /// </param>
    /// <param name="events">
    /// <inheritdoc cref="Events"/>
    /// </param>
    /// <param name="nestedObjectTypes">
    /// <inheritdoc cref="NestedObjectTypes"/>
    /// </param>
    /// <param name="nestedDelegates">
    /// <inheritdoc cref="NestedDelegates"/>
    /// </param>
    /// <param name="nestedEnums">
    /// <inheritdoc cref="NestedEnums"/>
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the members have already been added.
    /// </exception>
    internal void AddMembers(
        IReadOnlyDictionary<string, ConstructorData> constructors,
        IReadOnlyDictionary<string, FieldData> fields,
        IReadOnlyDictionary<string, PropertyData> properties,
        IReadOnlyDictionary<string, MethodData> methods,
        IReadOnlyDictionary<string, OperatorData> operators,
        IReadOnlyDictionary<string, IndexerData> indexers,
        IReadOnlyDictionary<string, EventData> events,
        IEnumerable<IObjectTypeData> nestedObjectTypes,
        IEnumerable<IDelegateTypeData> nestedDelegates,
        IEnumerable<IEnumTypeData> nestedEnums)
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
        Events = events;
        NestedObjectTypes = nestedObjectTypes;
        NestedDelegates = nestedDelegates;
        NestedEnums = nestedEnums;

        AllMembers = ((IEnumerable<MemberData>)Constructors.Values)
            .Concat(Fields.Values)
            .Concat(Methods.Values)
            .Concat(Properties.Values)
            .Concat(Operators.Values)
            .Concat(Indexers.Values)
            .Concat(Events.Values)
            .ToDictionary(m => m.Id);

        NestedTypes = ((IEnumerable<ITypeDeclaration>)NestedObjectTypes)
            .Concat(NestedDelegates)
            .Concat(NestedEnums);

        membersAdded = true;
    }
}
