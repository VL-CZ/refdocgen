using RefDocGen.CodeElements.Shared;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.TemplateGenerators.Shared.Languages;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModelCreators.Tools;

/// <summary>
/// Contains extension methods for <see cref="ITypeDeclaration"/>.
/// </summary>
internal static class ITypeDeclarationExtensions
{
    /// <summary>
    /// Converts the <paramref name="lang"/> into the corresopnding <see cref="ITypeNameData"/> instance.
    /// </summary>
    /// <param name="lang">The instance to be converted.</param>
    /// <returns>A corresponding <see cref="ITypeDeclaration"/> instance.</returns>
    internal static string GetTypeName(this ILanguageConfiguration lang, ITypeDeclaration type)
    {
        var typeName = lang.GetTypeName(type.TypeObject.GetTypeNameData());

        if (type.DeclaringType is not null)
        {
            typeName = GetTypeName(lang, type.DeclaringType) + "." + typeName;
        }

        return typeName;
    }
}
