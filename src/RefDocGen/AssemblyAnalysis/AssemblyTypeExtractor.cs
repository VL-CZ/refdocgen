using System.Reflection;
using RefDocGen.CodeElements.Concrete.Types.Enum;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Concrete;
using RefDocGen.CodeElements.Concrete.Types.Delegate;
using RefDocGen.CodeElements.Concrete.Members.Enum;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Concrete.Types.Attribute;

namespace RefDocGen.AssemblyAnalysis;

/// <summary>
/// Class responsible for extracting type information from a selected assembly.
/// </summary>
internal class AssemblyTypeExtractor
{
    /// <summary>
    /// Name of the delegate 'invoke' method.
    /// </summary>
    private const string delegateMethodName = "Invoke";

    /// <summary>
    /// Path to the DLL assembly to analyze and extract types.
    /// </summary>
    private readonly string assemblyPath;

    /// <summary>
    /// Binding flags used for selecting the types and its members.
    /// </summary>
    private readonly BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyTypeExtractor"/> class with the specified assembly path.
    /// </summary>
    /// <param name="assemblyPath">The path to the DLL assembly file</param>
    internal AssemblyTypeExtractor(string assemblyPath)
    {
        this.assemblyPath = assemblyPath;
    }

    /// <summary>
    /// Get all the declared types in the assembly and return them as <see cref="ObjectTypeData"/> objects.
    /// </summary>
    /// <returns>An array of <see cref="ObjectTypeData"/> objects representing the types in the assembly.</returns>
    internal TypeRegistry GetDeclaredTypes()
    {
        var assembly = Assembly.LoadFrom(assemblyPath);

        var enums = assembly
            .GetTypes()
            .Where(t => !t.IsCompilerGenerated() && t.IsEnum);

        var delegates = assembly
            .GetTypes()
            .Where(t => !t.IsCompilerGenerated() && t.IsDelegate());

        var types = assembly
            .GetTypes()
            .Where(t => !t.IsCompilerGenerated())
            .Except(enums)
            .Except(delegates);

        // construct types
        var enumData = enums
            .Select(ConstructEnum)
            .ToDictionary(t => t.Id);

        var delegateData = delegates
            .Select(ConstructDelegate)
            .ToDictionary(t => t.Id);

        var objectTypeData = types
            .Select(ConstructObjectType)
            .ToDictionary(t => t.Id);

        return new TypeRegistry(objectTypeData, enumData, delegateData);
    }

    /// <summary>
    /// Construct a <see cref="ObjectTypeData"/> object from the given <see cref="Type"/>.
    /// </summary>
    /// <param name="type">The type to construct the data model from.</param>
    /// <returns><see cref="ObjectTypeData"/> object representing the type.</returns>
    private ObjectTypeData ConstructObjectType(Type type)
    {
        var constructors = type
            .GetConstructors(bindingFlags)
            .Where(c => !c.IsCompilerGenerated());

        var fields = type
            .GetFields(bindingFlags)
            .Where(f => !f.IsCompilerGenerated());

        var indexers = type
            .GetProperties(bindingFlags)
            .Where(p => !p.IsCompilerGenerated() && p.IsIndexer());

        var properties = type
            .GetProperties(bindingFlags)
            .Where(p => !p.IsCompilerGenerated())
            .Except(indexers);

        var operators = type
            .GetMethods(bindingFlags)
            .Where(m => !m.IsCompilerGenerated() && m.IsOperator());

        var methods = type
            .GetMethods(bindingFlags)
            .Where(m => !m.IsCompilerGenerated() && !m.IsSpecialName)
            .Except(operators);

        var events = type
            .GetEvents(bindingFlags)
            .Where(e => !e.IsCompilerGenerated());

        // build the object type
        var objectType = new ObjectTypeBuilder(type)
            .WithConstructors(constructors)
            .WithFields(fields)
            .WithProperties(properties)
            .WithIndexers(indexers)
            .WithMethods(methods)
            .WithOperators(operators)
            .WithEvents(events)
            .Build();

        return objectType;
    }

    /// <summary>
    /// Construct an <see cref="EnumTypeData"/> object from the given <see cref="Type"/>.
    /// </summary>
    /// <param name="type">The type to construct the enum data model from.</param>
    /// <returns><see cref="EnumTypeData"/> object representing the enum.</returns>
    private EnumTypeData ConstructEnum(Type type)
    {
        var emptyTypeParams = new Dictionary<string, TypeParameterData>();
        var attributeData = GetAttributeData(type, emptyTypeParams);

        var enumType = new EnumTypeData(type, attributeData);

        var enumValues = type
            .GetFields(bindingFlags)
            .Where(f => !f.IsCompilerGenerated()
                && !f.IsSpecialName) // exclude '_value' field.
            .Select(f => new EnumMemberData(f, enumType, GetAttributeData(f, emptyTypeParams)))
            .ToDictionary(v => v.Id);

        enumType.AddMembers(enumValues);

        return enumType;
    }

    /// <summary>
    /// Construct a <see cref="DelegateTypeData"/> object from the given <see cref="Type"/>.
    /// </summary>
    /// <param name="type">The type to construct the delegate data model from.</param>
    /// <returns><see cref="DelegateTypeData"/> object representing the enum.</returns>
    private DelegateTypeData ConstructDelegate(Type type)
    {
        var typeParameters = GetTypeParameters(type);
        var attributeData = GetAttributeData(type, typeParameters);

        var delegateType = new DelegateTypeData(type, typeParameters, attributeData);

        var invokeMethodInfo = type.GetMethod(delegateMethodName) ?? throw new ArgumentException("TODO");

        var parameters = invokeMethodInfo.GetParameters()
            .Select((p, index) => new ParameterData(p, typeParameters, []))
            .ToDictionary(p => p.Name);

        var invokeMethod = new MethodData(
            invokeMethodInfo,
            delegateType,
            parameters,
            new Dictionary<string, TypeParameterData>(),
            typeParameters,
            []);

        delegateType.AddInvokeMethod(invokeMethod);

        return delegateType;
    }

    private AttributeData[] GetAttributeData(MemberInfo member, IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters)
    {
        return member.GetCustomAttributesData()
            .Where(a => !a.IsCompilerGenerated())
            .Select(a => new AttributeData(a, availableTypeParameters))
            .ToArray();
    }

    private Dictionary<string, TypeParameterData> GetTypeParameters(Type type)
    {
        return type
            .GetGenericArguments()
            .Select((ga, i) => new TypeParameterData(ga, i, CodeElementKind.Type))
            .ToDictionary(t => t.Name);
    }
}
