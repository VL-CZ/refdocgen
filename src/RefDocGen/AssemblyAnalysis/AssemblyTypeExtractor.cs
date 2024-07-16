using RefDocGen.MemberData;
using System.Reflection;

namespace RefDocGen.AssemblyAnalysis;

public class AssemblyTypeExtractor
{
    private readonly string assemblyPath;

    public AssemblyTypeExtractor(string assemblyPath)
    {
        this.assemblyPath = assemblyPath;
    }

    public ClassData[] GetDeclaredClasses()
    {
        var types = GetDeclaredTypes();
        return types.Select(ConstructFromType).ToArray();
    }

    private Type[] GetDeclaredTypes()
    {
        var assembly = Assembly.LoadFrom(assemblyPath);
        return assembly.GetTypes();
    }

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
