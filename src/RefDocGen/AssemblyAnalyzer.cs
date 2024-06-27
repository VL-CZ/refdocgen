using RefDocGen.Extensions;
using RefDocGen.Intermed;
using System.Linq;
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

        var fieldModels = fields.Select(f => new FieldIntermed(f.Name, f.FieldType.Name, f.GetAccessibilityModifier(), f.IsStatic)).ToArray();

        var propertyModels = properties.Select(p => new PropertyIntermed(p.Name, p.PropertyType.Name, p.GetMethod?.GetAccessibilityModifier(), p.SetMethod?.GetAccessibilityModifier())).ToArray();

        var methodModels = methods.Select(m =>
            new MethodIntermed(m.Name,
            m.GetParameters().Select(p => new MethodParameter(p.Name, p.ParameterType.Name)).ToArray(),
            m.ReturnType.Name, AccessibilityModifier.Public, m.IsStatic, m.IsVirtual, m.IsAbstract)
        ).ToArray();

        return new ClassIntermed(type.FullName, AccessibilityModifier.Public, fieldModels, propertyModels, methodModels);
    }
}
