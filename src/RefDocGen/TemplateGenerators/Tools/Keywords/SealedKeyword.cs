using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Abstract.Types;

namespace RefDocGen.TemplateGenerators.Tools.Keywords;

/// <summary>
/// Static class containing additional methods related to the 'sealed' keyword.
/// </summary>
internal static class SealedKeyword
{
    /// <summary>
    /// Checks whether the 'sealed' keyword is present in the provided type definition.
    /// </summary>
    /// <param name="type">Type that we check for 'sealed' keyword.</param>
    /// <returns>Boolean representing if the 'sealed' keyword is present in the type definition.</returns>
    internal static bool IsPresentIn(IObjectTypeData type)
    {
        return type.IsSealed && type.Kind == TypeKind.Class && !type.IsAbstract;
    }
}
