using RefDocGen.MemberData.Abstract;

namespace RefDocGen.TemplateGenerators.Default.Tools.TypeName;

/// <summary>
/// 
/// </summary>
internal class CSharpTypeName
{
    /// <summary>
    /// Dictionary of built-in C# type names
    /// </summary>
    private static readonly Dictionary<Type, string> builtInTypeNames = new()
    {
        [typeof(bool)] = "bool",
        [typeof(byte)] = "byte",
        [typeof(sbyte)] = "sbyte",
        [typeof(char)] = "char",
        [typeof(decimal)] = "decimal",
        [typeof(double)] = "double",
        [typeof(float)] = "float",
        [typeof(int)] = "int",
        [typeof(uint)] = "uint",
        [typeof(nint)] = "nint",
        [typeof(nuint)] = "nuint",
        [typeof(long)] = "long",
        [typeof(ulong)] = "ulong",
        [typeof(short)] = "short",
        [typeof(ushort)] = "ushort",
        [typeof(object)] = "object",
        [typeof(string)] = "string",
        [typeof(void)] = "void"
    };

    /// <summary>
    /// Get the built-in name of the type, or return <c>null</c> if the type doesn't have any corresponding built-in name.
    /// </summary>
    /// <param name="type">The type, whose name is retrieved.</param>
    /// <returns>Built-in name of the type (e.g. <c>int</c>), or <c>null</c> if the type doesn't have any corresponding built-in name.</returns>
    internal static string? GetBuiltInTypeName(ITypeNameData type)
    {
        const char arrayOpenBracket = '[';
        string? resultName = null;

        var baseElementType = type.GetBaseElementType();

        if (builtInTypeNames.TryGetValue(baseElementType, out string? builtInName))
        {
            if (type.IsArray && type.ShortName.Contains(arrayOpenBracket))
            {
                int arrayBracketsStartIndex = type.ShortName.IndexOf(arrayOpenBracket);
                return builtInName + type.ShortName[arrayBracketsStartIndex..]; // append the array brackets string
            }
            else
            {
                resultName = builtInName;
            }
        }

        return resultName;
    }

    /// <summary>
    /// Get the name of the given type in C# format.
    /// </summary>
    /// <param name="type">The type, whose name is retrieved.</param>
    /// <returns>Name of the type formatted according to C# conventions.</returns>
    public static string Of(ITypeNameData type)
    {
        string typeName = GetBuiltInTypeName(type) ?? type.ShortName;

        if (type.HasGenericParameters)
        {
            const string genericParamsDelimiter = ", ";
            string genericParamsString = string.Join(genericParamsDelimiter, type.GenericParameters.Select(Of));

            typeName += '<' + genericParamsString + '>';
        }

        if (type.IsPointer)
        {
            typeName += '*';
        }

        return typeName;
    }
}

