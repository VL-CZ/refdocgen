using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Abstract.Types;

namespace RefDocGen.TemplateGenerators.Tools.Keywords;

/// <summary>
/// Static class containing additional methods related to the 'abstract' keyword.
/// </summary>
internal static class AbstractKeyword
{
    /// <summary>
    /// Checks whether the 'abstract' keyword is present in the provided type definition.
    /// </summary>
    /// <param name="typeData">Type that we check for 'abstract' keyword.</param>
    /// <returns>Boolean representing if the 'abstract' keyword is present in the type definition.</returns>
    internal static bool IsPresentIn(IObjectTypeData typeData)
    {
        return typeData.IsAbstract && typeData.Kind == TypeKind.Class;
    }
}
