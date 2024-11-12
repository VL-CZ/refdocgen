using RefDocGen.MemberData.Concrete;
using System.Reflection;

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
    /// Initializes a new instance of the <see cref="AssemblyTypeExtractor"/> class with the specified assembly path.
    /// </summary>
    /// <param name="assemblyPath">The path to the DLL assembly file</param>
    internal AssemblyTypeExtractor(string assemblyPath)
    {
        this.assemblyPath = assemblyPath;
    }

    /// <summary>
    /// Get all the declared classes in the assembly and return them as <see cref="TypeData"/> objects.
    /// </summary>
    /// <returns>An array of <see cref="TypeData"/> objects representing the classes in the assembly.</returns>
    internal TypeData[] GetDeclaredClasses()
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
        return assembly.GetTypes().Where(t => !t.IsCompilerGenerated()).ToArray();
    }

    /// <summary>
    /// Construct a <see cref="TypeData"/> object from a given <see cref="Type"/>.
    /// </summary>
    /// <param name="type">The type to construct the data model from.</param>
    /// <returns><see cref="TypeData"/> object representing the type.</returns>
    private TypeData ConstructFromType(Type type)
    {
        var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        var constructors = type.GetConstructors(bindingFlags).Where(f => !f.IsCompilerGenerated());
        var fields = type.GetFields(bindingFlags).Where(f => !f.IsCompilerGenerated());
        var properties = type.GetProperties(bindingFlags).Where(f => !f.IsCompilerGenerated());
        var methods = type.GetMethods(bindingFlags).Where(f => !f.IsCompilerGenerated());

        // construct *Data objects
        var ctorModels = constructors.Select(c => new ConstructorData(c)).ToDictionary(c => c.Id);
        var fieldModels = fields.Select(f => new FieldData(f)).ToDictionary(f => f.Id);
        var propertyModels = properties.Select(p => new PropertyData(p)).ToDictionary(p => p.Id);
        var methodModels = methods.Select(m => new MethodData(m)).ToDictionary(m => m.Id);

        return new TypeData(type, ctorModels, fieldModels, propertyModels, methodModels);
    }
}
