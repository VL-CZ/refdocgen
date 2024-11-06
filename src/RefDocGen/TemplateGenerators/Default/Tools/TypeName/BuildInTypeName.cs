using RefDocGen.MemberData.Abstract;

namespace RefDocGen.TemplateGenerators.Default.Tools.TypeName;

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

    private static Type GetBaseElementType(Type type)
    {
        var elementType = type.GetElementType();

        return elementType is null ? type : GetBaseElementType(elementType);
    }

    internal static string Get(ITypeNameData type)
    {
        var t = GetBaseElementType(type.TypeObject);

        if (builtInTypes.TryGetValue(t, out string? builtInName))
        {
            if (type.IsArray)
            {
                int endOfTypeNameIndex = type.ShortName.IndexOf('[');
                return builtInName + type.ShortName[endOfTypeNameIndex..];
            }
            else
            {
                return builtInName;
            }
        }
        else
        {
            return type.ShortName;
        }
    }
}
