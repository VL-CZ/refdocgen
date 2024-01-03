using RefDocGen.TemplateModels;
using System.Reflection;

namespace RefDocGen;

public class AssemblyAnalyzer
{
    private readonly string assemblyPath;

    public AssemblyAnalyzer(string assemblyPath)
    {
        this.assemblyPath = assemblyPath;
    }

    public ClassTemplateModel[] GetTemplateModels()
    {
        var types = GetDeclaredTypes();

        return types.Select(ConstructFromType).ToArray();
    }

    private Type[] GetDeclaredTypes()
    {
        var assembly = Assembly.LoadFrom(assemblyPath);
        return assembly.GetTypes();
    }

    private ClassTemplateModel ConstructFromType(Type type)
    {
        var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        var fields = type.GetFields(bindingFlags);
        var methods = type.GetMethods(bindingFlags);

        var fieldModels = fields.Select(f => new FieldTemplateModel(f.Name, f.FieldType.Name, "")).ToArray();
        var methodModels = methods.Select(m => new MethodTemplateModel(m.Name, m.ReturnType.Name, "")).ToArray();

        return new ClassTemplateModel(type.FullName, string.Empty, fieldModels, methodModels);
    }
}
