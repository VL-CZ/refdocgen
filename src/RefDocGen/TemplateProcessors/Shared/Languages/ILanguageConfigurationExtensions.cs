using RefDocGen.CodeElements.Shared;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.TypeName;

namespace RefDocGen.TemplateProcessors.Shared.Languages;

/// <summary>
/// Contains extension methods for <see cref="ILanguageConfiguration"/>.
/// </summary>
internal static class ILanguageConfigurationExtensions
{
    /// <summary>
    /// Gets the name of the given <paramref name="type"/> in the format of <paramref name="lang"/> language.
    /// </summary>
    /// <param name="lang">The language, in which the type name should be returned.</param>
    /// <param name="type">The type, whose name is returned.</param>
    /// <returns>Name of the given <paramref name="type"/> in the format of <paramref name="lang"/> language.</returns>
    internal static string GetTypeName(this ILanguageConfiguration lang, ITypeDeclaration type)
    {
        return lang.GetTypeName(type.TypeObject.GetTypeNameData());
    }

    /// <summary>
    /// Gets the type name (including its generic parameters) of the given <paramref name="type"/>.
    /// </summary>
    /// <param name="lang">The language, in which the type name should be returned.</param>
    /// <param name="type">The type, whose name is returned.</param>
    /// <param name="includeTypeParameters">Specifies whether the type paramters should be included in the result.</param>
    /// <param name="useTypeFullName">Specifies whether the full name of the type should be used.</param>
    /// <returns>The name of the type.</returns>
    internal static string GetTypeName(this ILanguageConfiguration lang, ITypeNameData type, bool includeTypeParameters, bool useTypeFullName)
    {
        string typeName = lang.GetTypeName(type, includeTypeParameters);

        if (useTypeFullName && type.Namespace != "")
        {
            return $"{type.Namespace}.{typeName}";
        }
        else
        {
            return typeName;
        }
    }
}
