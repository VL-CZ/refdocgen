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

        var fields = type.GetFields(bindingFlags);
        var methods = type.GetMethods(bindingFlags);

        var fieldModels = fields.Select(f => new FieldIntermed(f.Name, f.FieldType.Name, GetAccessibilityModifier(f), f.IsStatic)).ToArray();
        var methodModels = methods.Select(m =>
            new MethodIntermed(m.Name,
            m.GetParameters().Select(p => new MethodParameter(p.Name, p.ParameterType.Name)).ToArray(),
            m.ReturnType.Name, AccessibilityModifier.Public, m.IsStatic, m.IsVirtual, m.IsAbstract)
        ).ToArray();

        return new ClassIntermed(type.FullName, AccessibilityModifier.Public, fieldModels, methodModels);
    }

    private AccessibilityModifier GetAccessibilityModifier(FieldInfo field)
    {
        if (field.IsPrivate)
        {
            return AccessibilityModifier.Private;
        }
        else if (field.IsFamily)
        {
            return AccessibilityModifier.Protected;
        }
        else if (field.IsAssembly)
        {
            return AccessibilityModifier.Internal;
        }
        else
        {
            return AccessibilityModifier.Public; // TODO: additional checks
        }
    }
}
