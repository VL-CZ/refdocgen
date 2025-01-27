using RefDocGen.AssemblyAnalysis.MemberCreators;
using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Concrete.Types.Attribute;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RefDocGen.AssemblyAnalysis;

internal class ObjectTypeBuilder
{
    /// <summary>
    /// The object type to be built.
    /// </summary>
    private readonly ObjectTypeData type;

    /// <inheritdoc cref="TypeDeclaration.TypeParameters"/>
    private readonly Dictionary<string, TypeParameterData> typeParameters;

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
    /// Initializes a new instance of <see cref="ObjectTypeBuilder"/> class.
    /// </summary>
    /// <param name="type">The type of the object to be built.</param>
    internal ObjectTypeBuilder(Type type)
    {
        typeParameters = Helper.GetTypeParametersDictionary(type);
        var attributeData = Helper.GetAttributeData(type, typeParameters);

        // construct the object type
        this.type = new ObjectTypeData(type, typeParameters, attributeData);
    }

    /// <summary>
    /// Adds constructors to the object being built.
    /// </summary>
    /// <param name="constructors">An enumerable of object constructors.</param>
    /// <returns>The current <see cref="ObjectTypeBuilder"/> instance.</returns>
    internal ObjectTypeBuilder AddConstructors(IEnumerable<ConstructorInfo> constructors)
    {
        this.constructors = constructors
            .Select(c => ConstructorDataCreator.CreateFrom(c, type, typeParameters))
            .ToIdDictionary();

        return this;
    }

    internal ObjectTypeBuilder AddFields(IEnumerable<FieldInfo> fields)
    {
        this.fields = fields
            .Select(f => FieldDataCreator.CreateFrom(f, type, typeParameters))
            .ToIdDictionary();

        return this;
    }

    internal ObjectTypeBuilder AddProperties(IEnumerable<PropertyInfo> properties)
    {
        this.properties = properties
            .Select(p => PropertyDataCreator.CreateFrom(p, type, typeParameters))
            .ToIdDictionary();

        return this;
    }

    internal ObjectTypeBuilder AddIndexers(IEnumerable<PropertyInfo> indexers)
    {
        this.indexers = indexers
            .Select(i => IndexerDataCreator.CreateFrom(i, type, typeParameters))
            .ToIdDictionary();

        return this;
    }

    internal ObjectTypeBuilder AddMethods(IEnumerable<MethodInfo> methods)
    {
        this.methods = methods
            .Select(m => MethodDataCreator.CreateFrom(m, type, typeParameters))
            .ToIdDictionary();

        return this;
    }

    internal ObjectTypeBuilder AddOperators(IEnumerable<MethodInfo> operators)
    {
        this.operators = operators
            .Select(o => OperatorDataCreator.CreateFrom(o, type, typeParameters))
            .ToIdDictionary();

        return this;
    }

    internal ObjectTypeBuilder AddEvents(IEnumerable<EventInfo> events)
    {
        this.events = events
            .Select(e => EventDataCreator.CreateFrom(e, type, typeParameters))
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
}

internal static class Helper
{
    internal static Dictionary<string, ParameterData> GetParametersDictionary(ConstructorInfo constructor, IReadOnlyDictionary<string, TypeParameterData> typeParameters)
    {
        return GetParametersDictionary(constructor.GetParameters(), typeParameters);
    }

    internal static Dictionary<string, ParameterData> GetParametersDictionary(MethodInfo method, IReadOnlyDictionary<string, TypeParameterData> typeParameters)
    {
        bool isExtensionMethod = method.IsDefined(typeof(ExtensionAttribute), true);
        return GetParametersDictionary(method.GetParameters(), typeParameters, isExtensionMethod);
    }

    internal static Dictionary<string, ParameterData> GetParametersDictionary(PropertyInfo indexer, IReadOnlyDictionary<string, TypeParameterData> typeParameters)
    {
        return GetParametersDictionary(indexer.GetIndexParameters(), typeParameters);
    }

    internal static Dictionary<string, TypeParameterData> GetTypeParametersDictionary(Type type)
    {
        return GetTypeParametersDictionary(type.GetGenericArguments(), CodeElementKind.Type);
    }

    internal static Dictionary<string, TypeParameterData> GetTypeParametersDictionary(MethodBase method)
    {
        return GetTypeParametersDictionary(method.GetGenericArguments(), CodeElementKind.Member);
    }

    internal static AttributeData[] GetAttributeData(ParameterInfo member, IReadOnlyDictionary<string, TypeParameterData> typeParameters)
    {
        return GetAttributeData(member.GetCustomAttributesData(), typeParameters);
    }

    internal static AttributeData[] GetAttributeData(MemberInfo member, IReadOnlyDictionary<string, TypeParameterData> typeParameters)
    {
        return GetAttributeData(member.GetCustomAttributesData(), typeParameters);
    }

    private static AttributeData[] GetAttributeData(IEnumerable<CustomAttributeData> attributes, IReadOnlyDictionary<string, TypeParameterData> typeParameters)
    {
        return attributes
            .Where(a => !a.IsCompilerGenerated())
            .Select(a => new AttributeData(a, typeParameters))
            .ToArray();
    }

    private static Dictionary<string, TypeParameterData> GetTypeParametersDictionary(Type[] typeParameters, CodeElementKind codeElementKind)
    {
        return typeParameters
            .Select((ga, i) => new TypeParameterData(ga, i, codeElementKind))
            .ToDictionary(t => t.Name);
    }

    private static Dictionary<string, ParameterData> GetParametersDictionary(
        ParameterInfo[] parameters,
        IReadOnlyDictionary<string, TypeParameterData> typeParameters,
        bool hasExtensionParameter = false)
    {
        return parameters
            .Select((p, index) => new ParameterData(
                p,
                typeParameters,
                GetAttributeData(p, typeParameters),
                hasExtensionParameter && index == 0))
            .ToDictionary(p => p.Name);
    }
}
