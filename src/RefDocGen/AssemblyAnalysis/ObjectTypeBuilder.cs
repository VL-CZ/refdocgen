using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Concrete.Types.Attribute;
using RefDocGen.Tools;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RefDocGen.AssemblyAnalysis;

internal class ObjectTypeBuilder
{
    private readonly ObjectTypeData type;
    private readonly Dictionary<string, TypeParameterData> typeParameters;

    private Dictionary<string, ConstructorData> constructors = [];
    private Dictionary<string, FieldData> fields = [];
    private Dictionary<string, PropertyData> properties = [];
    private Dictionary<string, IndexerData> indexers = [];
    private Dictionary<string, MethodData> methods = [];
    private Dictionary<string, OperatorData> operators = [];
    private Dictionary<string, EventData> events = [];

    internal ObjectTypeBuilder(Type type)
    {
        typeParameters = Helper.GetTypeParametersDictionary(type);
        var attributeData = Helper.GetAttributeData(type, typeParameters);

        // construct the object type
        this.type = new ObjectTypeData(type, typeParameters, attributeData);
    }

    internal ObjectTypeBuilder WithConstructors(IEnumerable<ConstructorInfo> constructors)
    {
        this.constructors = constructors
            .Select(c => ConstructorDataCreator.CreateFrom(c, type, typeParameters))
            .ToIdDictionary();

        return this;
    }

    internal ObjectTypeBuilder WithFields(IEnumerable<FieldInfo> fields)
    {
        this.fields = fields
            .Select(f => FieldDataCreator.CreateFrom(f, type, typeParameters))
            .ToIdDictionary();

        return this;
    }

    internal ObjectTypeBuilder WithProperties(IEnumerable<PropertyInfo> properties)
    {
        this.properties = properties
            .Select(p => PropertyDataCreator.CreateFrom(p, type, typeParameters))
            .ToIdDictionary();

        return this;
    }

    internal ObjectTypeBuilder WithIndexers(IEnumerable<PropertyInfo> indexers)
    {
        this.indexers = indexers
            .Select(i => IndexerDataCreator.CreateFrom(i, type, typeParameters))
            .ToIdDictionary();

        return this;
    }

    internal ObjectTypeBuilder WithMethods(IEnumerable<MethodInfo> methods)
    {
        this.methods = methods
            .Select(m => MethodDataCreator.CreateFrom(m, type, typeParameters))
            .ToIdDictionary();

        return this;
    }

    internal ObjectTypeBuilder WithOperators(IEnumerable<MethodInfo> operators)
    {
        this.operators = operators
            .Select(o => OperatorDataCreator.CreateFrom(o, type, typeParameters))
            .ToIdDictionary();

        return this;
    }

    internal ObjectTypeBuilder WithEvents(IEnumerable<EventInfo> events)
    {
        this.events = events
            .Select(e => EventDataCreator.CreateFrom(e, type, typeParameters))
            .ToIdDictionary();

        return this;
    }

    internal ObjectTypeData Build()
    {
        // add the members
        type.AddMembers(constructors, fields, properties, methods, operators, indexers, events);
        return type;
    }
}

/// <summary>
/// Class responsible for creating <see cref="ConstructorData"/> instances.
/// </summary>
internal static class ConstructorDataCreator
{
    /// <summary>
    /// Creates a <see cref="ConstructorData"/> instance from the corresponding <paramref name="constructor"/>.
    /// </summary>
    /// <param name="constructor"><see cref="ConstructorInfo"/> object representing the constructor.</param>
    /// <param name="containingType">Type containing the constructor.</param>
    /// <param name="availableTypeParameters">A dictionary of available type parameters; the keys represent type parameter names.</param>
    /// <returns>A <see cref="ConstructorData"/> instance representing the constructor.</returns>
    internal static ConstructorData CreateFrom(ConstructorInfo constructor, TypeDeclaration containingType, Dictionary<string, TypeParameterData> availableTypeParameters)
    {
        return new ConstructorData(
            constructor,
            containingType,
            Helper.GetParametersDictionary(constructor, availableTypeParameters),
            Helper.GetAttributeData(constructor, availableTypeParameters));
    }
}

/// <summary>
/// Class responsible for creating <see cref="FieldData"/> instances.
/// </summary>
internal static class FieldDataCreator
{
    /// <summary>
    /// Creates a <see cref="FieldData"/> instance from the corresponding <paramref name="field"/>.
    /// </summary>
    /// <param name="field"><see cref="FieldInfo"/> object representing the field.</param>
    /// <param name="containingType">Type containing the field.</param>
    /// <param name="availableTypeParameters">A dictionary of available type parameters; the keys represent type parameter names.</param>
    /// <returns>A <see cref="FieldData"/> instance representing the field.</returns>
    internal static FieldData CreateFrom(FieldInfo field, TypeDeclaration containingType, Dictionary<string, TypeParameterData> availableTypeParameters)
    {
        return new FieldData(
            field,
            containingType,
            availableTypeParameters,
            Helper.GetAttributeData(field, availableTypeParameters));
    }
}

/// <summary>
/// Class responsible for creating <see cref="PropertyData"/> instances.
/// </summary>
internal static class PropertyDataCreator
{
    /// <summary>
    /// Creates a <see cref="PropertyData"/> instance from the corresponding <paramref name="property"/>.
    /// </summary>
    /// <param name="property"><see cref="PropertyInfo"/> object representing the property.</param>
    /// <param name="containingType">Type containing the property.</param>
    /// <param name="availableTypeParameters">A dictionary of available type parameters; the keys represent type parameter names.</param>
    /// <returns>A <see cref="PropertyData"/> instance representing the property.</returns>
    internal static PropertyData CreateFrom(PropertyInfo property, TypeDeclaration containingType, Dictionary<string, TypeParameterData> availableTypeParameters)
    {
        var getterMethod = property.GetMethod is not null
            ? MethodDataCreator.CreateFrom(property.GetMethod, containingType, availableTypeParameters)
            : null;

        var setterMethod = property.SetMethod is not null
            ? MethodDataCreator.CreateFrom(property.SetMethod, containingType, availableTypeParameters)
            : null;

        return new PropertyData(
            property,
            getterMethod,
            setterMethod,
            containingType,
            availableTypeParameters,
            Helper.GetAttributeData(property, availableTypeParameters));
    }
}

/// <summary>
/// Class responsible for creating <see cref="IndexerData"/> instances.
/// </summary>
internal static class IndexerDataCreator
{
    /// <summary>
    /// Creates a <see cref="IndexerData"/> instance from the corresponding <paramref name="propertyInfo"/>.
    /// </summary>
    /// <param name="propertyInfo"><see cref="PropertyInfo"/> object representing the indexer.</param>
    /// <param name="containingType">Type containing the indexer.</param>
    /// <param name="availableTypeParameters">A dictionary of available type parameters; the keys represent type parameter names.</param>
    /// <returns>A <see cref="IndexerData"/> instance representing the indexer.</returns>
    internal static IndexerData CreateFrom(PropertyInfo propertyInfo, TypeDeclaration containingType, Dictionary<string, TypeParameterData> availableTypeParameters)
    {
        var getterMethod = propertyInfo.GetMethod is not null
            ? MethodDataCreator.CreateFrom(propertyInfo.GetMethod, containingType, availableTypeParameters)
            : null;

        var setterMethod = propertyInfo.SetMethod is not null
            ? MethodDataCreator.CreateFrom(propertyInfo.SetMethod, containingType, availableTypeParameters)
            : null;

        return new IndexerData(
            propertyInfo,
            getterMethod,
            setterMethod,
            containingType,
            Helper.GetParametersDictionary(propertyInfo, availableTypeParameters),
            availableTypeParameters,
            Helper.GetAttributeData(propertyInfo, availableTypeParameters));
    }
}

/// <summary>
/// Class responsible for creating <see cref="MethodData"/> instances.
/// </summary>
internal static class MethodDataCreator
{
    /// <summary>
    /// Creates a <see cref="MethodData"/> instance from the corresponding <paramref name="methodInfo"/>.
    /// </summary>
    /// <param name="methodInfo"><see cref="MethodInfo"/> object representing the method.</param>
    /// <param name="containingType">Type containing the method.</param>
    /// <param name="availableTypeParameters">A dictionary of available type parameters; the keys represent type parameter names.</param>
    /// <returns>A <see cref="MethodData"/> instance representing the method.</returns>
    internal static MethodData CreateFrom(MethodInfo methodInfo, TypeDeclaration containingType, Dictionary<string, TypeParameterData> availableTypeParameters)
    {
        var declaredTypeParameters = Helper.GetTypeParametersDictionary(methodInfo);
        var allTypeParameters = availableTypeParameters.Merge(declaredTypeParameters);

        return new MethodData(
            methodInfo,
            containingType,
            Helper.GetParametersDictionary(methodInfo, allTypeParameters),
            declaredTypeParameters,
            allTypeParameters,
            Helper.GetAttributeData(methodInfo, allTypeParameters));
    }
}

/// <summary>
/// Class responsible for creating <see cref="OperatorData"/> instances.
/// </summary>
internal static class OperatorDataCreator
{
    /// <summary>
    /// Creates a <see cref="OperatorData"/> instance from the corresponding <paramref name="methodInfo"/>.
    /// </summary>
    /// <param name="methodInfo"><see cref="MethodInfo"/> object representing the operator.</param>
    /// <param name="containingType">Type containing the operator.</param>
    /// <param name="availableTypeParameters">A dictionary of available type parameters; the keys represent type parameter names.</param>
    /// <returns>A <see cref="OperatorData"/> instance representing the operator.</returns>
    internal static OperatorData CreateFrom(MethodInfo methodInfo, TypeDeclaration containingType, Dictionary<string, TypeParameterData> availableTypeParameters)
    {
        return new OperatorData(
            methodInfo,
            containingType,
            Helper.GetParametersDictionary(methodInfo, availableTypeParameters),
            Helper.GetTypeParametersDictionary(methodInfo),
            availableTypeParameters,
            Helper.GetAttributeData(methodInfo, availableTypeParameters));
    }
}

/// <summary>
/// Class responsible for creating <see cref="EventData"/> instances.
/// </summary>
internal static class EventDataCreator
{
    /// <summary>
    /// Creates a <see cref="EventData"/> instance from the corresponding <paramref name="eventInfo"/>.
    /// </summary>
    /// <param name="eventInfo"><see cref="EventInfo"/> object representing the event.</param>
    /// <param name="containingType">Type containing the event.</param>
    /// <param name="availableTypeParameters">A dictionary of available type parameters; the keys represent type parameter names.</param>
    /// <returns>A <see cref="EventData"/> instance representing the event.</returns>
    internal static EventData CreateFrom(EventInfo eventInfo, TypeDeclaration containingType, Dictionary<string, TypeParameterData> availableTypeParameters)
    {
        MethodData? addMethod = null;
        MethodData? removeMethod = null;

        if (eventInfo.GetAddMethod(nonPublic: true) is MethodInfo addMethodInfo)
        {
            addMethod = MethodDataCreator.CreateFrom(addMethodInfo, containingType, availableTypeParameters);
        }

        if (eventInfo.GetRemoveMethod(nonPublic: true) is MethodInfo removeMethodInfo)
        {
            removeMethod = MethodDataCreator.CreateFrom(removeMethodInfo, containingType, availableTypeParameters);
        }

        return new EventData(
            eventInfo,
            addMethod,
            removeMethod,
            containingType,
            availableTypeParameters,
            Helper.GetAttributeData(eventInfo, availableTypeParameters));
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
