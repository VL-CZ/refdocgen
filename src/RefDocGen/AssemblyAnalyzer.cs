using RefDocGen.Extensions;
using RefDocGen.Intermed;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

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

        var fieldModels = fields.Select(ConstructField).ToArray();
        var propertyModels = properties.Select(ConstructProperty).ToArray();
        var methodModels = methods.Select(ConstructMethod).ToArray();

        return new ClassIntermed(type.FullName, AccessModifier.Public, fieldModels, propertyModels, methodModels);
    }

    private FieldIntermed ConstructField(FieldInfo field)
    {
        return new FieldIntermed(field.Name, field.FieldType.Name, field.GetAccessModifier(), field.IsStatic, field.IsInitOnly, field.IsLiteral);
    }

    private PropertyIntermed ConstructProperty(PropertyInfo property)
    {
        var accessors = property.GetAccessors();

        bool isStatic = accessors.All(a => a.IsStatic);
        bool isAbstract = accessors.All(a => a.IsAbstract);
        bool isVirtual = accessors.All(a => a.IsVirtual);

        return new PropertyIntermed(property.Name, property.PropertyType.Name, property.GetAccessModifier(),
            property.GetMethod?.GetAccessModifier(), property.SetMethod?.GetAccessModifier(),
            isStatic, isVirtual, isAbstract);
    }

    private MethodIntermed ConstructMethod(MethodInfo method)
    {
        var parameters = method.GetParameters().Select(ConstructMethodParameter).ToArray();
        bool isOverridable = method.IsVirtual && !method.IsFinal;
        bool overridesAnotherMethod = !method.Equals(method.GetBaseDefinition());
        bool isAsync = method.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null;

        return new MethodIntermed(method.Name, parameters, method.ReturnType.Name, method.GetAccessModifier(), method.IsStatic, isOverridable, overridesAnotherMethod, method.IsAbstract, method.IsFinal, isAsync);
    }

    private MethodParameter ConstructMethodParameter(ParameterInfo p)
    {
        return new MethodParameter(p.Name, p.ParameterType.Name);
    }
}
