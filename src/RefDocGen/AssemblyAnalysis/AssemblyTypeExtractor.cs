using System.Reflection;
using RefDocGen.CodeElements.Concrete.Types.Enum;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Concrete;
using RefDocGen.CodeElements.Concrete.Types.Delegate;
using RefDocGen.CodeElements.Concrete.Members.Enum;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements;

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

        // construct *Data objects
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
        var constructors = type.GetConstructors(bindingFlags).Where(c => !c.IsCompilerGenerated());
        var fields = type.GetFields(bindingFlags).Where(f => !f.IsCompilerGenerated());

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

        var typeParameters = type
            .GetGenericArguments()
            .Select((ga, i) => new TypeParameterData(ga, i, CodeElementKind.Type))
            .ToDictionary(t => t.Name);

        // construct the object type
        var objectType = new ObjectTypeData(type, typeParameters);

        // construct *Data objects
        var ctorModels = constructors
            .Select(c => new ConstructorData(c, objectType, typeParameters))
            .ToDictionary(c => c.Id);

        var fieldModels = fields
            .Select(f => new FieldData(f, objectType, typeParameters))
            .ToDictionary(f => f.Id);

        var propertyModels = properties
            .Select(p => new PropertyData(p, objectType, typeParameters))
            .ToDictionary(p => p.Id);

        var methodModels = methods
            .Select(m => new MethodData(m, objectType, typeParameters))
            .ToDictionary(m => m.Id);

        var operatorModels = operators
            .Select(m => new OperatorData(m, objectType, typeParameters))
            .ToDictionary(m => m.Id);

        var indexerModels = indexers
            .Select(m => new IndexerData(m, objectType, typeParameters))
            .ToDictionary(m => m.Id);

        // add the members
        objectType.AddMembers(ctorModels, fieldModels, propertyModels, methodModels, operatorModels, indexerModels);

        return objectType;
    }

    /// <summary>
    /// Construct an <see cref="EnumTypeData"/> object from the given <see cref="Type"/>.
    /// </summary>
    /// <param name="type">The type to construct the enum data model from.</param>
    /// <returns><see cref="EnumTypeData"/> object representing the enum.</returns>
    private EnumTypeData ConstructEnum(Type type)
    {
        var enumType = new EnumTypeData(type);

        var enumValues = type
            .GetFields(bindingFlags)
            .Where(f => !f.IsCompilerGenerated()
                && !f.IsSpecialName) // exclude '_value' field.
            .Select(f => new EnumMemberData(f, enumType))
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
        var typeParameters = type
            .GetGenericArguments()
            .Select((ga, i) => new TypeParameterData(ga, i, CodeElementKind.Type))
            .ToDictionary(t => t.Name);

        var invokeMethod = type.GetMethod(delegateMethodName) ?? throw new ArgumentException("TODO");

        return new DelegateTypeData(type, invokeMethod, typeParameters);
    }
}
