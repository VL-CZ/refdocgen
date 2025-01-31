using RefDocGen.AssemblyAnalysis.MemberCreators;
using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;
using System.Reflection;

namespace RefDocGen.AssemblyAnalysis;

/// <summary>
/// Class responsible for building an instance of <see cref="ObjectTypeData"/> class.
/// </summary>
internal class ObjectTypeDataBuilder
{
    /// <summary>
    /// The object type to be built.
    /// </summary>
    private readonly ObjectTypeData type;

    /// <inheritdoc cref="TypeDeclaration.TypeParameters"/>
    private readonly Dictionary<string, TypeParameterData> typeParameters;

    /// <summary>
    /// Minimal visibility of the members to include.
    /// </summary>
    private readonly AccessModifier minVisibility;

    /// <inheritdoc cref="ObjectTypeData.Constructors"/>
    private Dictionary<string, ConstructorData> constructors = [];

    /// <inheritdoc cref="ObjectTypeData.Fields"/>
    private Dictionary<string, FieldData> fields = [];

    /// <inheritdoc cref="ObjectTypeData.Properties"/>
    private Dictionary<string, PropertyData> properties = [];

    /// <inheritdoc cref="ObjectTypeData.Indexers"/>
    private Dictionary<string, IndexerData> indexers = [];

    /// <inheritdoc cref="ObjectTypeData.Methods"/>
    private Dictionary<string, MethodData> methods = [];

    /// <inheritdoc cref="ObjectTypeData.Operators"/>
    private Dictionary<string, OperatorData> operators = [];

    /// <inheritdoc cref="ObjectTypeData.Events"/>
    private Dictionary<string, EventData> events = [];

    /// <summary>
    /// Initializes a new instance of <see cref="ObjectTypeDataBuilder"/> class.
    /// </summary>
    /// <param name="type">The type of the object to be built.</param>
    /// <param name="minVisibility">Minimal visibility of the members to include.</param>
    internal ObjectTypeDataBuilder(Type type, AccessModifier minVisibility)
    {
        typeParameters = MemberCreatorHelper.CreateTypeParametersDictionary(type);
        var attributeData = MemberCreatorHelper.GetAttributeData(type, typeParameters);

        // construct the object type
        this.type = new ObjectTypeData(type, typeParameters, attributeData);
        this.minVisibility = minVisibility;
    }

    /// <summary>
    /// Adds constructors to the object being built.
    /// </summary>
    /// <param name="constructors">An enumerable of object constructors.</param>
    /// <returns>The current <see cref="ObjectTypeDataBuilder"/> instance.</returns>
    internal ObjectTypeDataBuilder AddConstructors(IEnumerable<ConstructorInfo> constructors)
    {
        this.constructors = constructors
            .Select(c => ConstructorDataCreator.CreateFrom(c, type, typeParameters))
            .Where(IsVisible)
            .ToIdDictionary();

        return this;
    }

    /// <summary>
    /// Adds fields to the object being built.
    /// </summary>
    /// <param name="fields">An enumerable of object fields.</param>
    /// <returns>The current <see cref="ObjectTypeDataBuilder"/> instance.</returns>
    internal ObjectTypeDataBuilder AddFields(IEnumerable<FieldInfo> fields)
    {
        this.fields = fields
            .Select(f => FieldDataCreator.CreateFrom(f, type, typeParameters))
            .Where(IsVisible)
            .ToIdDictionary();

        return this;
    }

    /// <summary>
    /// Adds properties to the object being built.
    /// </summary>
    /// <param name="properties">An enumerable of object properties.</param>
    /// <returns>The current <see cref="ObjectTypeDataBuilder"/> instance.</returns>
    internal ObjectTypeDataBuilder AddProperties(IEnumerable<PropertyInfo> properties)
    {
        this.properties = properties
            .Select(p => PropertyDataCreator.CreateFrom(p, type, typeParameters))
            .Where(IsVisible)
            .ToIdDictionary();

        return this;
    }

    /// <summary>
    /// Adds indexers to the object being built.
    /// </summary>
    /// <param name="indexers">An enumerable of object indexers.</param>
    /// <returns>The current <see cref="ObjectTypeDataBuilder"/> instance.</returns>
    internal ObjectTypeDataBuilder AddIndexers(IEnumerable<PropertyInfo> indexers)
    {
        this.indexers = indexers
            .Select(i => IndexerDataCreator.CreateFrom(i, type, typeParameters))
            .Where(IsVisible)
            .ToIdDictionary();

        return this;
    }

    /// <summary>
    /// Adds methods to the object being built.
    /// </summary>
    /// <param name="methods">An enumerable of object methods.</param>
    /// <returns>The current <see cref="ObjectTypeDataBuilder"/> instance.</returns>
    internal ObjectTypeDataBuilder AddMethods(IEnumerable<MethodInfo> methods)
    {
        this.methods = methods
            .Select(m => MethodDataCreator.CreateFrom(m, type, typeParameters))
            .Where(IsVisible)
            .ToIdDictionary();

        return this;
    }

    /// <summary>
    /// Adds operators to the object being built.
    /// </summary>
    /// <param name="operators">An enumerable of object operators.</param>
    /// <returns>The current <see cref="ObjectTypeDataBuilder"/> instance.</returns>
    internal ObjectTypeDataBuilder AddOperators(IEnumerable<MethodInfo> operators)
    {
        this.operators = operators
            .Select(o => OperatorDataCreator.CreateFrom(o, type, typeParameters))
            .Where(IsVisible)
            .ToIdDictionary();

        return this;
    }

    /// <summary>
    /// Adds events to the object being built.
    /// </summary>
    /// <param name="events">An enumerable of object events.</param>
    /// <returns>The current <see cref="ObjectTypeDataBuilder"/> instance.</returns>
    internal ObjectTypeDataBuilder AddEvents(IEnumerable<EventInfo> events)
    {
        this.events = events
            .Select(e => EventDataCreator.CreateFrom(e, type, typeParameters))
            .Where(IsVisible)
            .ToIdDictionary();

        return this;
    }

    /// <summary>
    /// Builds the object type using the provided members.
    /// </summary>
    /// <returns>An <see cref="ObjectTypeData"/> instance containing the provided members.</returns>
    internal ObjectTypeData Build()
    {
        // add the members
        type.AddMembers(constructors, fields, properties, methods, operators, indexers, events);
        return type;
    }

    /// <summary>
    /// Checks if the <paramref name="member"/> has at least <see cref="minVisibility"/>.
    /// </summary>
    /// <param name="member">The member to check.</param>
    /// <returns>
    /// <c>true</c> if the member has at least the visibility determined by <see cref="minVisibility"/>, <c>false</c> otherwise.
    /// </returns>
    private bool IsVisible(IMemberData member)
    {
        return member.AccessModifier.IsAtMostAsRestrictiveAs(minVisibility);
    }
}
