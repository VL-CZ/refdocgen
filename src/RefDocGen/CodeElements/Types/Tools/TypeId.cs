using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.CodeElements.Types.Concrete.TypeName;
using RefDocGen.Tools;

namespace RefDocGen.CodeElements.Types.Tools;

/// <summary>
/// Class providing methods for getting IDs of the declared types.
/// </summary>
internal class TypeId
{
    /// <summary>
    /// Get the ID of the given <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type, whose ID is returned.</param>
    /// <returns>The ID of the given <paramref name="type"/></returns>
    internal static string Of(ITypeDeclaration type)
    {
        string id = type.FullName;

        if (type.DeclaringType is not null)
        {
            id = $"{type.DeclaringType.Id}.{type.ShortName}";
        }

        if (type.HasTypeParameters)
        {
            id = $"{id}`{type.TypeParameters.Count}";
        }

        return id;
    }

    /// <inheritdoc cref="Of(ITypeDeclaration)"/>
    /// <param name="type">The type, whose ID is returned.</param>
    /// <param name="isDeclarationId">Indicates whether the ID in type declaration format should be returned (default: false).</param>
    /// <returns>The ID of the given <paramref name="type"/></returns>
    internal static string Of(ITypeNameData type, bool isDeclarationId = false)
    {
        string id = type.FullName;

        if (type.DeclaringType is not null)
        {
            id = $"{Of(type.DeclaringType, isDeclarationId)}.{type.ShortName}";
        }

        if (type.HasTypeParameters)
        {
            if (isDeclarationId)
            {
                id += $"`{type.TypeParameters.Count}"; // add count of type parameters
            }
            else
            {
                id = id + '{' + string.Join(",", type.TypeParameters.Select(p => p.Id)) + '}'; // select all type parameter IDs
            }
        }

        return id;
    }

    /// <summary>
    /// Get the ID of the given generic type parameter.
    /// </summary>
    /// <param name="param">The generic type parameter whose ID is returned.</param>
    /// <returns>ID of the given generic type parameter.</returns>
    internal static string Of(GenericTypeParameterNameData param)
    {
        string paramName = param.ShortName;
        string idSuffix = "";

        if (param.IsArray) // Array -> We need to split the type name into 2 parts: type parameter name and brackets
        {
            if (paramName.TryGetIndex('[', out int i))
            {
                (paramName, idSuffix) = (paramName[..i], paramName[i..]);
            }
        }
        else if (param.IsPointer) // Pointer -> equivalent to Array type
        {
            if (paramName.TryGetIndex('*', out int i))
            {
                (paramName, idSuffix) = (paramName[..i], paramName[i..]);
            }
        }

        if (param.availableTypeParameters.TryGetValue(paramName, out var typeParameter))
        {
            // We need to get the index of the generic parameter, for further info see:
            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments#d42-id-string-format

            string idPrefix = typeParameter.DeclaredAt == CodeElementKind.Type
                ? "`" // declared in a type -> single backtick
                : "``"; // declared in a member -> double backtick

            return idPrefix + typeParameter.Index + idSuffix; // generic param found -> use its index
        }
        else
        {
            return paramName + idSuffix; // generic param not found -> use its name
        }
    }
}
