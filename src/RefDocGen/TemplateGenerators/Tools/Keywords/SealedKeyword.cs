using RefDocGen.MemberData;
using RefDocGen.MemberData.Abstract;

namespace RefDocGen.TemplateGenerators.Tools.Keywords;

/// <summary>
/// Static class containing additional methods related to the 'sealed' keyword.
/// </summary>
internal static class SealedKeyword
{
    /// <summary>
    /// Checks whether the 'sealed' keyword is present in the provided type definition.
    /// </summary>
    /// <param name="typeData">Type that we check for 'sealed' keyword.</param>
    /// <returns>Boolean representing if the 'sealed' keyword is present in the type definition.</returns>
    internal static bool IsPresentIn(ITypeData typeData)
    {
        return typeData.IsSealed && typeData.Kind == TypeKind.Class;
    }
}
