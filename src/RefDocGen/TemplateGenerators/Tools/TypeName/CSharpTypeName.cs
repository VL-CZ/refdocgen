using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.Tools;

namespace RefDocGen.TemplateGenerators.Tools.TypeName;

/// <summary>
/// Static class used for retrieving type names in C# format.
/// </summary>
internal static class CSharpTypeName
{
    /// <summary>
    /// Dictionary of the built-in C# type names.
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
    internal static string? GetBuiltInName(ITypeNameData type)
    {
        const char arrayOpenBracket = '[';
        var baseElementType = type.TypeObject.GetBaseElementType();

        if (builtInTypeNames.TryGetValue(baseElementType, out string? builtInName))
        {
            if (type.IsArray && type.ShortName.Contains(arrayOpenBracket)) // if it's an array, we need to append the brackets (according to the array depth)
            {
                int arrayBracketsStartIndex = type.ShortName.IndexOf(arrayOpenBracket);
                builtInName += type.ShortName[arrayBracketsStartIndex..]; // append the array brackets string
            }
        }

        return builtInName;
    }

    /// <summary>
    /// Get the name of the given type in C# format.
    /// </summary>
    /// <param name="type">The type, whose name is retrieved.</param>
    /// <returns>Name of the type formatted according to C# conventions.</returns>
    internal static string Of(ITypeNameData type)
    {
        string typeName = GetBuiltInName(type) ?? type.ShortName;

        if (type.HasTypeParameters)
        {
            string genericParamsString = string.Join(", ", type.TypeParameters.Select(Of));
            typeName += '<' + genericParamsString + '>'; // add generic params to the type name
        }

        if (type.IsPointer)
        {
            typeName += '*';
        }

        return typeName;
    }
}

