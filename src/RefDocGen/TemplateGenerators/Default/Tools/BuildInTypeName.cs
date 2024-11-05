using RefDocGen.MemberData.Abstract;

namespace RefDocGen.TemplateGenerators.Default.Tools;

internal class BuiltInTypeName
{
    private static readonly Dictionary<Type, string> builtInTypes = new()
    {
        { typeof(bool), "bool" },
        { typeof(byte), "byte" },
        { typeof(sbyte), "sbyte" },
        { typeof(char), "char" },
        { typeof(decimal), "decimal" },
        { typeof(double), "double" },
        { typeof(float), "float" },
        { typeof(int), "int" },
        { typeof(uint), "uint" },
        { typeof(nint), "nint" },
        { typeof(nuint), "nuint" },
        { typeof(long), "long" },
        { typeof(ulong), "ulong" },
        { typeof(short), "short" },
        { typeof(ushort), "ushort" },
        { typeof(object), "object" },
        { typeof(string), "string" },
        { typeof(void), "void" },
    };

    internal static string Get(ITypeNameData type)
    {
        return builtInTypes.TryGetValue(type.TypeObject, out string? builtInName)
            ? builtInName
            : type.ShortName;
    }
}