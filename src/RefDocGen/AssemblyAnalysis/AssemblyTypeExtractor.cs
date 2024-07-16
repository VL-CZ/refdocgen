using RefDocGen.MemberData;
using System.Reflection;

namespace RefDocGen.AssemblyAnalysis;

/// <summary>
/// Class responsible for extracting type information from a selected assembly.
/// </summary>
public class AssemblyTypeExtractor
{
    /// <summary>
    /// Path to the DLL assembly to analyze and extract types.
    /// </summary>
    private readonly string assemblyPath;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyTypeExtractor"/> class with the specified assembly path.
    /// </summary>
    /// <param name="assemblyPath">The path to the DLL assembly file</param>
    public AssemblyTypeExtractor(string assemblyPath)
    {
        this.assemblyPath = assemblyPath;
    }

    /// <summary>
    /// Get all the declared classes in the assembly and return them as <see cref="ClassData"/> objects.
    /// </summary>
    /// <returns>An array of <see cref="ClassData"/> objects representing the classes in the assembly.</returns>
    public ClassData[] GetDeclaredClasses()
    {
        var types = GetDeclaredTypes();
        return types.Select(ConstructFromType).ToArray();
    }

    /// <summary>
    /// Get all declared types in the assembly.
    /// </summary>
    /// <returns>List of all declared types in the assembly.</returns>
    private Type[] GetDeclaredTypes()
    {
        var assembly = Assembly.LoadFrom(assemblyPath);
        return assembly.GetTypes();
    }

    /// <summary>
    /// Construct a <see cref="ClassData"/> object from a given <see cref="Type"/>.
    /// </summary>
    /// <param name="type">The type to construct the data model from.</param>
    /// <returns><see cref="ClassData"/> object representing the type.</returns>
    private ClassData ConstructFromType(Type type)
    {
        var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        var fields = type.GetFields(bindingFlags).Where(f => !f.IsCompilerGenerated());
        var properties = type.GetProperties(bindingFlags).Where(f => !f.IsCompilerGenerated());
        var methods = type.GetMethods(bindingFlags).Where(f => !f.IsCompilerGenerated());

        var fieldModels = fields.Select(f => new FieldData(f)).ToArray();
        var propertyModels = properties.Select(p => new PropertyData(p)).ToArray();
        var methodModels = methods.Select(m => new MethodData(m)).ToArray();

        return new ClassData(type.FullName, AccessModifier.Public, fieldModels, propertyModels, methodModels);
    }
}
