using RefDocGen.Extensions;
using RefDocGen.Intermed;
using System.Reflection;

namespace RefDocGen;

public class AssemblyAnalyzer
{
    private readonly string assemblyPath;

    public AssemblyAnalyzer(string assemblyPath)
    {
        this.assemblyPath = assemblyPath;
    }

    public ClassIntermed[] GetIntermedTypes()
    {
        var types = GetDeclaredTypes();
        return types.Select(ConstructFromType).ToArray();
    }

    private Type[] GetDeclaredTypes()
    {
        var assembly = Assembly.LoadFrom(assemblyPath);
        return assembly.GetTypes();
    }

    private ClassIntermed ConstructFromType(Type type)
    {
        var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        var fields = type.GetFields(bindingFlags).Where(f => !f.IsCompilerGenerated());
        var properties = type.GetProperties(bindingFlags).Where(f => !f.IsCompilerGenerated());
        var methods = type.GetMethods(bindingFlags).Where(f => !f.IsCompilerGenerated());

        var fieldModels = fields.Select(f => new FieldIntermed(f)).ToArray();
        var propertyModels = properties.Select(p => new PropertyIntermed(p)).ToArray();
        var methodModels = methods.Select(m => new MethodIntermed(m)).ToArray();

        return new ClassIntermed(type.FullName, AccessModifier.Public, fieldModels, propertyModels, methodModels);
    }
}
