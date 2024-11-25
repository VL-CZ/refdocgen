using RefDocGen.CodeElements.Concrete.Types;
using System.Reflection;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Concrete.Types.Enum;
using RefDocGen.CodeElements.Concrete.Members.Enum;

namespace RefDocGen.AssemblyAnalysis;

/// <summary>
/// Class responsible for extracting type information from a selected assembly.
/// </summary>
internal class AssemblyTypeExtractor
{
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

        var types = assembly
            .GetTypes()
            .Where(t => !t.IsCompilerGenerated() && !t.IsEnum)
            .Select(ConstructFromType)
            .ToDictionary(t => t.Id);

        var enums = assembly
            .GetTypes()
            .Where(t => !t.IsCompilerGenerated() && t.IsEnum)
            .Select(ConstructFromEnum)
            .ToDictionary(t => t.Id);

        return new TypeRegistry(types, enums);
    }

    /// <summary>
    /// Construct an <see cref="EnumTypeData"/> object from the given <see cref="Type"/>.
    /// </summary>
    /// <param name="type">The type to construct the enum data model from.</param>
    /// <returns><see cref="EnumTypeData"/> object representing the enum.</returns>
    private EnumTypeData ConstructFromEnum(Type type)
    {
        var enumValues = type
            .GetFields(bindingFlags)
            .Where(f => !f.IsCompilerGenerated() && !f.IsSpecialName) // exclude '_value' field.
            .Select(f => new EnumMemberData(f))
            .ToDictionary(v => v.Id);

        return new EnumTypeData(type, enumValues);
    }

    /// <summary>
    /// Construct a <see cref="ObjectTypeData"/> object from the given <see cref="Type"/>.
    /// </summary>
    /// <param name="type">The type to construct the data model from.</param>
    /// <returns><see cref="ObjectTypeData"/> object representing the type.</returns>
    private ObjectTypeData ConstructFromType(Type type)
    {
        var constructors = type.GetConstructors(bindingFlags).Where(c => !c.IsCompilerGenerated());
        var fields = type.GetFields(bindingFlags).Where(f => !f.IsCompilerGenerated());
        var properties = type.GetProperties(bindingFlags).Where(p => !p.IsCompilerGenerated());
        var methods = type.GetMethods(bindingFlags).Where(m => !m.IsCompilerGenerated());
        var typeParameters = type.GetGenericArguments().Select((ga, i) => new TypeParameterDeclaration(ga.Name, i)).ToDictionary(t => t.Name);

        // construct *Data objects
        var ctorModels = constructors
            .Select(c => new ConstructorData(c, typeParameters))
            .ToDictionary(c => c.Id);

        var fieldModels = fields
            .Select(f => new FieldData(f, typeParameters))
            .ToDictionary(f => f.Id);

        var propertyModels = properties
            .Select(p => new PropertyData(p, typeParameters))
            .ToDictionary(p => p.Id);

        var methodModels = methods
            .Select(m => new MethodData(m, typeParameters))
            .ToDictionary(m => m.Id);

        return new ObjectTypeData(type, ctorModels, fieldModels, propertyModels, methodModels, typeParameters);
    }
}
