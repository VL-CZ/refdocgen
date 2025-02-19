using System.Reflection;
using RefDocGen.CodeElements.Concrete.Types.Enum;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Concrete;
using RefDocGen.CodeElements.Concrete.Types.Delegate;
using RefDocGen.CodeElements.Concrete.Members.Enum;
using RefDocGen.AssemblyAnalysis.MemberCreators;
using RefDocGen.CodeElements;
using RefDocGen.Tools;

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
    /// Minimal visibility of the types and members to include.
    /// </summary>
    private readonly AccessModifier minVisibility;

    /// <summary>
    /// Indicates whether the methods inherited from <see cref="object"/> and <see cref="ValueType"/> types should be included in the types.
    /// </summary>
    private readonly bool excludeObjectMethods;

    /// <summary>
    /// Collection of all declared nested object types.
    /// </summary>
    private readonly List<ObjectTypeData> allNestedObjectTypes = [];

    /// <summary>
    /// Collection of all declared nested delegate types.
    /// </summary>
    private readonly List<DelegateTypeData> allNestedDelegates = [];

    /// <summary>
    /// Collection of all declared nested enum types.
    /// </summary>
    private readonly List<EnumTypeData> allNestedEnums = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyTypeExtractor"/> class with the specified assembly path.
    /// </summary>
    /// <param name="assemblyPath">The path to the DLL assembly file</param>
    /// <param name="minVisibility">Minimal visibility of the types and members to include.</param>
    /// <param name="inheritMembers">Indicates whether the types should contain inherited members as well.</param>
    internal AssemblyTypeExtractor(string assemblyPath, AccessModifier minVisibility, MemberInheritance inheritMembers)
    {
        this.assemblyPath = assemblyPath;
        this.minVisibility = minVisibility;

        var defaultBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        bindingFlags = inheritMembers == MemberInheritance.None
            ? defaultBindingFlags | BindingFlags.DeclaredOnly
            : defaultBindingFlags;

        excludeObjectMethods = inheritMembers == MemberInheritance.NonObject;
    }

    /// <summary>
    /// Get all the declared types in the assembly (including enums and delegates).
    /// </summary>
    /// <returns>An instance of <see cref="TypeRegistry"/> class, containing all of the declared types.</returns>
    internal TypeRegistry GetDeclaredTypes()
    {
        var assembly = Assembly.LoadFrom(assemblyPath);

        var visibleTypes = assembly
            .GetTypes()
            .Where(t => !t.IsCompilerGenerated()) // exclude the types generated by the compiler
            .Where(t => !t.IsNested) // exlude nested types - these are created in 'ConstructObjectType' method
            .Where(t => t.IsVisible(minVisibility)); // select only the types that are visible

        var enums = visibleTypes
            .Where(t => t.IsEnum);

        var delegates = visibleTypes
            .Where(t => t.IsDelegate());

        var objectTypes = visibleTypes
            .Except(enums)
            .Except(delegates);

        // construct types
        var enumsData = enums
            .Select(ConstructEnum)
            .ToIdDictionary();

        var delegatesData = delegates
            .Select(ConstructDelegate)
            .ToIdDictionary();

        var objectTypesData = objectTypes
            .Select(ConstructObjectType)
            .ToIdDictionary();

        return new TypeRegistry( // add the types (including nested ones) to the registry
            objectTypesData.Merge(allNestedObjectTypes.ToIdDictionary()),
            enumsData.Merge(allNestedEnums.ToIdDictionary()),
            delegatesData.Merge(allNestedDelegates.ToIdDictionary()));
    }

    /// <summary>
    /// Construct a <see cref="ObjectTypeData"/> object from the given <see cref="Type"/>.
    /// </summary>
    /// <param name="type">The type to construct the data model from.</param>
    /// <returns><see cref="ObjectTypeData"/> object representing the type.</returns>
    private ObjectTypeData ConstructObjectType(Type type)
    {
        // get members
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

        if (excludeObjectMethods) // exclude methods inherited from 'object' and 'ValueType' types (if desired)
        {
            methods = methods.Where(m => m.DeclaringType != typeof(object) && m.DeclaringType != typeof(ValueType));
        }

        // get nested types
        var allNestedTypes = type
            .GetNestedTypes(bindingFlags)
            .Where(f => !f.IsCompilerGenerated());

        var nestedDelegates = allNestedTypes
            .Where(t => t.IsDelegate())
            .Select(ConstructDelegate)
            .ToArray();

        var nestedEnums = allNestedTypes
            .Where(t => t.IsEnum)
            .Select(ConstructEnum)
            .ToArray();

        var nestedObjectTypes = allNestedTypes
            .Where(t => !t.IsEnum && !t.IsDelegate())
            .Select(ConstructObjectType)
            .ToArray();

        // build the object type
        var objectType = new ObjectTypeDataBuilder(type, minVisibility)
            .AddConstructors(constructors)
            .AddFields(fields)
            .AddProperties(properties)
            .AddIndexers(indexers)
            .AddMethods(methods)
            .AddOperators(operators)
            .AddEvents(events)
            .AddNestedObjectTypes(nestedObjectTypes)
            .AddNestedDelegates(nestedDelegates)
            .AddNestedEnums(nestedEnums)
            .Build();

        // remember the nested types
        allNestedObjectTypes.AddRange(nestedObjectTypes);
        allNestedDelegates.AddRange(nestedDelegates);
        allNestedEnums.AddRange(nestedEnums);

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
        var attributeData = MemberCreatorHelper.GetAttributeData(type, emptyTypeParams);

        var enumType = new EnumTypeData(type, attributeData);

        var enumValues = type
            .GetFields(bindingFlags)
            .Where(f => !f.IsCompilerGenerated()
                && !f.IsSpecialName) // exclude '_value' field.
            .Select(f => new EnumMemberData(f, enumType, MemberCreatorHelper.GetAttributeData(f, emptyTypeParams)))
            .ToIdDictionary();

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
        var typeParameters = MemberCreatorHelper.CreateTypeParametersDictionary(type);
        var attributeData = MemberCreatorHelper.GetAttributeData(type, typeParameters);

        var delegateType = new DelegateTypeData(type, typeParameters, attributeData);

        var invokeMethodInfo = type.GetMethod(delegateMethodName) ?? throw new ArgumentException("TODO");
        var invokeMethod = MethodDataCreator.CreateFrom(invokeMethodInfo, delegateType, typeParameters);

        delegateType.AddInvokeMethod(invokeMethod);

        return delegateType;
    }
}
