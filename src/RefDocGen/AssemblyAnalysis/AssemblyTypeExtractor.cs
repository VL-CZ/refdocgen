using Microsoft.Extensions.Logging;
using RefDocGen.AssemblyAnalysis.Extensions;
using RefDocGen.AssemblyAnalysis.MemberCreators;
using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Members.Concrete.Enum;
using RefDocGen.CodeElements.Shared;
using RefDocGen.CodeElements.TypeRegistry;
using RefDocGen.CodeElements.Types.Concrete;
using RefDocGen.CodeElements.Types.Concrete.Delegate;
using RefDocGen.CodeElements.Types.Concrete.Enum;
using RefDocGen.Tools.Exceptions;
using System.Reflection;

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
    /// Names of assemblies to be excluded from the reference documentation.
    /// </summary>
    private readonly IEnumerable<string> assembliesToExclude;

    /// <summary>
    /// Namespaces to be excluded from the reference documentation.
    /// </summary>
    private readonly IEnumerable<string> namespacesToExclude;

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
    /// A logger instance.
    /// </summary>
    private readonly ILogger logger;

    /// <summary>
    /// Absolute paths to the DLL assemblies to analyze and extract types.
    /// </summary>
    private readonly IEnumerable<string> assemblyPaths;

    /// <summary>
    /// Absolute paths to the assemblies that are to be documented.
    /// </summary>
    /// <remarks>
    /// The assemblies marked as excluded are not contained in this collection.
    /// </remarks>
    internal IEnumerable<string> AnalyzedAssemblies { get; private set; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyTypeExtractor"/> class with the specified assembly path.
    /// </summary>
    /// <param name="assemblyPaths">Paths to the assembly files that should be documented.</param>
    /// <param name="configuration">Configuration describing what data should be extracted.</param>
    /// <param name="logger">A logger instance.</param>
    internal AssemblyTypeExtractor(IEnumerable<string> assemblyPaths, AssemblyDataConfiguration configuration, ILogger logger)
    {
        this.assemblyPaths = assemblyPaths.Select(Path.GetFullPath); // use absolute paths
        this.logger = logger;

        minVisibility = configuration.MinVisibility;
        assembliesToExclude = configuration.AssembliesToExclude;
        namespacesToExclude = configuration.NamespacesToExclude;

        var defaultBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        bindingFlags = configuration.MemberInheritanceMode == MemberInheritanceMode.None
            ? defaultBindingFlags | BindingFlags.DeclaredOnly
            : defaultBindingFlags;

        excludeObjectMethods = configuration.MemberInheritanceMode == MemberInheritanceMode.NonObject;
    }

    /// <summary>
    /// Get all the declared types in the assembly (including enums and delegates).
    /// </summary>
    /// <returns>An instance of <see cref="TypeRegistry"/> class, containing all of the declared types.</returns>
    internal TypeRegistry GetDeclaredTypes()
    {
        List<Type> types = [];
        List<string> includedAssemblies = [];

        foreach (string assemblyPath in assemblyPaths)
        {
            Assembly? assembly = null;
            try
            {
                string assemblyFolder = Path.GetDirectoryName(assemblyPath) ?? "";
                var alc = new AssemblyDependencyLoadContext(assemblyFolder);

                assembly = alc.LoadFromAssemblyPath(assemblyPath);
            }
            catch (Exception e) when (e is FileNotFoundException or ArgumentNullException or ArgumentException)
            {
                throw new AssemblyNotFoundException(assemblyPath, e); // Assembly not found
            }

            // load types
            if (!assembliesToExclude.Contains(assembly.GetName().Name))
            {
                types.AddRange(assembly.GetTypes());
                includedAssemblies.Add(assemblyPath);

                logger.LogInformation("Assembly {Name} loaded", assemblyPath);
            }
            else
            {
                logger.LogInformation("Assembly {Name} excluded", assemblyPath);
            }
        }

        AnalyzedAssemblies = includedAssemblies; // set the analyzed assemblies

        var visibleTypes = types
            .Where(t => !IsInExcludedNamespace(t))
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
            .Select(ConstructEnum);

        var delegatesData = delegates
            .Select(ConstructDelegate);

        var objectTypesData = objectTypes
            .Select(ConstructObjectType);

        return new TypeRegistry( // add the types (including nested ones) to the registry
            objectTypesData.Concat(allNestedObjectTypes),
            enumsData.Concat(allNestedEnums),
            delegatesData.Concat(allNestedDelegates));
    }

    /// <summary>
    /// Construct a <see cref="ObjectTypeData"/> object from the given <see cref="Type"/>.
    /// </summary>
    /// <param name="type">The type to construct the data model from.</param>
    /// <returns><see cref="ObjectTypeData"/> object representing the type.</returns>
    private ObjectTypeData ConstructObjectType(Type type)
    {
        var (ifaceProperties, ifaceMethods, ifaceEvents) = GetInheritedInterfaceMembers(type); // if the type is an interface, the inherited members must be resolve manually

        // get members
        var constructors = type
            .GetConstructors(bindingFlags)
            .Where(c => !c.IsCompilerGenerated());

        var fields = type
            .GetFields(bindingFlags)
            .Where(f => !f.IsCompilerGenerated());

        var indexers = type
            .GetProperties(bindingFlags)
            .Concat(ifaceProperties)
            .Where(p => !p.IsCompilerGenerated() && p.IsIndexer());

        var properties = type
            .GetProperties(bindingFlags)
            .Concat(ifaceProperties)
            .Where(p => !p.IsCompilerGenerated())
            .Except(indexers);

        var operators = type
            .GetMethods(bindingFlags)
            .Concat(ifaceMethods)
            .Where(m => !m.IsCompilerGenerated() && m.IsOperator());

        var methods = type
            .GetMethods(bindingFlags)
            .Concat(ifaceMethods)
            .Where(m => !m.IsCompilerGenerated() && !m.IsSpecialName)
            .Except(operators);

        var events = type
            .GetEvents(bindingFlags)
            .Concat(ifaceEvents)
            .Where(e => !e.IsCompilerGenerated());

        if (excludeObjectMethods) // exclude methods inherited from 'object' and 'ValueType' types (if requested)
        {
            methods = methods.Where(m => m.DeclaringType != typeof(object) && m.DeclaringType != typeof(ValueType));
        }

        // get nested types
        var allNestedTypes = type
            .GetNestedTypes(bindingFlags)
            .Where(f => !f.IsCompilerGenerated())
            .Where(t => t.IsVisible(minVisibility));

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
    /// Gets all inherited interface methods for an interface type.
    /// </summary>
    /// <param name="type">The selected type.</param>
    /// <returns>
    /// All properties, methods and events inherited from other interfaces, if the <paramref name="type"/> is an interface.
    /// <br/>
    /// If the <paramref name="type"/> is not an interface all enumerables are empty.
    /// </returns>
    private (IEnumerable<PropertyInfo> Properties, IEnumerable<MethodInfo> Methods, IEnumerable<EventInfo> Events)
            GetInheritedInterfaceMembers(Type type)
    {
        if (!type.IsInterface || bindingFlags.HasFlag(BindingFlags.DeclaredOnly)) // the type is not an interface
                                                                                  // OR the inherited members are to not meant to be included
        {
            return ([], [], []);
        }

        var interfaceMethods = type.GetInterfaces()
                .SelectMany(i => i.GetMethods(bindingFlags));

        var interfaceProperties = type.GetInterfaces()
                .SelectMany(i => i.GetProperties(bindingFlags));

        var interfaceEvents = type.GetInterfaces()
                .SelectMany(i => i.GetEvents(bindingFlags));

        return (interfaceProperties, interfaceMethods, interfaceEvents);
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

        var invokeMethodInfo = type.GetMethod(delegateMethodName) ?? throw new ArgumentException("The 'Invoke' method of a delegate wasn't found.");
        var invokeMethod = MethodDataCreator.CreateFrom(invokeMethodInfo, delegateType, typeParameters);

        delegateType.AddInvokeMethod(invokeMethod);

        return delegateType;
    }

    /// <summary>
    /// Checks if the given types is in an excluded namespace.
    /// </summary>
    /// <param name="type">The provided type.</param>
    /// <returns><c>true</c> if the given types is in an excluded namespace, <c>false</c> otherwise.</returns>
    private bool IsInExcludedNamespace(Type type)
    {
        return namespacesToExclude.Any(ns => type.Namespace == ns ||
            (type.Namespace?.StartsWith($"{ns}.", StringComparison.Ordinal) ?? false));
    }
}
