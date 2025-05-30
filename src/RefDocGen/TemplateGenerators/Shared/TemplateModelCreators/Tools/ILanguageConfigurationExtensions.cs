using RefDocGen.CodeElements.Shared;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.TemplateGenerators.Shared.Languages;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModelCreators.Tools;

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
}
