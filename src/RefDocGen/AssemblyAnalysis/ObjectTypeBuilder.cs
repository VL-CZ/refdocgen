using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Concrete.Types.Attribute;
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
        typeParameters = GetTypeParameters(type);
        var attributeData = GetAttributeData(type);

        // construct the object type
        this.type = new ObjectTypeData(type, typeParameters, attributeData);
    }

    internal ObjectTypeBuilder WithConstructors(IEnumerable<ConstructorInfo> constructors)
    {
        this.constructors = constructors
            .Select(ConstructConstructor)
            .ToDictionary(c => c.Id);

        return this;
    }

    internal ObjectTypeBuilder WithFields(IEnumerable<FieldInfo> fields)
    {
        this.fields = fields
            .Select(ConstructField)
            .ToDictionary(c => c.Id);

        return this;
    }

    internal ObjectTypeBuilder WithProperties(IEnumerable<PropertyInfo> properties)
    {
        this.properties = properties
            .Select(ConstructProperty)
            .ToDictionary(p => p.Id);

        return this;
    }

    internal ObjectTypeBuilder WithIndexers(IEnumerable<PropertyInfo> indexers)
    {
        this.indexers = indexers
            .Select(ConstructIndexer)
            .ToDictionary(p => p.Id);

        return this;
    }

    internal ObjectTypeBuilder WithMethods(IEnumerable<MethodInfo> methods)
    {
        this.methods = methods
            .Select(ConstructMethod)
            .ToDictionary(m => m.Id);

        return this;
    }

    internal ObjectTypeBuilder WithOperators(IEnumerable<MethodInfo> operators)
    {
        this.operators = operators
            .Select(ConstructOperator)
            .ToDictionary(m => m.Id);

        return this;
    }

    internal ObjectTypeBuilder WithEvents(IEnumerable<EventInfo> events)
    {
        this.events = events
            .Select(ConstructEvent)
            .ToDictionary(e => e.Id);

        return this;
    }

    internal ObjectTypeData Build()
    {
        // add the members
        type.AddMembers(constructors, fields, properties, methods, operators, indexers, events);
        return type;
    }

    private AttributeData[] GetAttributeData(MemberInfo member)
    {
        return member.GetCustomAttributesData()
            .Where(a => !a.IsCompilerGenerated())
            .Select(a => new AttributeData(a, typeParameters))
            .ToArray();
    }

    private AttributeData[] GetAttributeData(ParameterInfo member)
    {
        return member.GetCustomAttributesData()
            .Where(a => !a.IsCompilerGenerated())
            .Select(a => new AttributeData(a, typeParameters))
            .ToArray();
    }

    private Dictionary<string, TypeParameterData> GetTypeParameters(Type type)
    {
        return GetDictionary(type.GetGenericArguments(), CodeElementKind.Type);
    }

    private Dictionary<string, TypeParameterData> GetTypeParameters(MethodBase method)
    {
        return GetDictionary(method.GetGenericArguments(), CodeElementKind.Member);
    }

    private ConstructorData ConstructConstructor(ConstructorInfo constructorInfo)
    {
        return new ConstructorData(
            constructorInfo,
            type,
            GetDictionary(constructorInfo.GetParameters()),
            GetAttributeData(constructorInfo));
    }

    private FieldData ConstructField(FieldInfo fieldInfo)
    {
        return new FieldData(
            fieldInfo,
            type,
            typeParameters,
            GetAttributeData(fieldInfo));
    }

    private PropertyData ConstructProperty(PropertyInfo propertyInfo)
    {
        var getterMethod = propertyInfo.GetMethod is not null
            ? ConstructMethod(propertyInfo.GetMethod)
            : null;

        var setterMethod = propertyInfo.SetMethod is not null
            ? ConstructMethod(propertyInfo.SetMethod)
            : null;

        return new PropertyData(
            propertyInfo,
            getterMethod,
            setterMethod,
            type,
            typeParameters,
            GetAttributeData(propertyInfo));
    }

    private IndexerData ConstructIndexer(PropertyInfo propertyInfo)
    {
        var getterMethod = propertyInfo.GetMethod is not null
            ? ConstructMethod(propertyInfo.GetMethod)
            : null;

        var setterMethod = propertyInfo.SetMethod is not null
            ? ConstructMethod(propertyInfo.SetMethod)
            : null;

        return new IndexerData(
            propertyInfo,
            getterMethod,
            setterMethod,
            type,
            GetDictionary(propertyInfo.GetIndexParameters()),
            typeParameters,
            GetAttributeData(propertyInfo));
    }

    private MethodData ConstructMethod(MethodInfo methodInfo)
    {
        bool isExtensionMethod = methodInfo.IsDefined(typeof(ExtensionAttribute), true);

        return new MethodData(
            methodInfo,
            type,
            GetDictionary(methodInfo.GetParameters(), isExtensionMethod),
            GetTypeParameters(methodInfo),
            typeParameters,
            GetAttributeData(methodInfo));
    }

    private OperatorData ConstructOperator(MethodInfo methodInfo)
    {
        return new OperatorData(
            methodInfo,
            type,
            GetDictionary(methodInfo.GetParameters()),
            GetTypeParameters(methodInfo),
            typeParameters,
            GetAttributeData(methodInfo));
    }

    private EventData ConstructEvent(EventInfo eventInfo)
    {
        (MethodData? addMethod, MethodData? removeMethod) = (null, null);

        // construct the event methods
        if (eventInfo.GetAddMethod(nonPublic: true) is MethodInfo addMethodInfo)
        {
            addMethod = ConstructMethod(addMethodInfo);
        }

        if (eventInfo.GetRemoveMethod(nonPublic: true) is MethodInfo removeMethodInfo)
        {
            removeMethod = ConstructMethod(removeMethodInfo);
        }

        return new EventData(
            eventInfo,
            addMethod,
            removeMethod,
            type,
            typeParameters,
            GetAttributeData(eventInfo));
    }

    private Dictionary<string, ParameterData> GetDictionary(ParameterInfo[] parameters, bool hasExtensionParameter = false)
    {
        return parameters
            .Select((p, index) => new ParameterData(
                p,
                typeParameters,
                GetAttributeData(p),
                hasExtensionParameter && index == 0))
            .ToDictionary(p => p.Name);
    }

    private Dictionary<string, TypeParameterData> GetDictionary(Type[] typeParameters, CodeElementKind codeElementKind)
    {
        return typeParameters
            .Select((ga, i) => new TypeParameterData(ga, i, codeElementKind))
            .ToDictionary(t => t.Name);
    }
}
