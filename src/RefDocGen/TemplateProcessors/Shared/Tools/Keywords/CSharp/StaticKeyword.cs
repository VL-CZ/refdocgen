using RefDocGen.CodeElements.Types.Abstract;

namespace RefDocGen.TemplateProcessors.Shared.Tools.Keywords.CSharp;

/// <summary>
/// Static class containing additional methods related to the 'static' keyword.
/// </summary>
internal static class StaticKeyword
{
    /// <summary>
    /// Checks whether the 'static' keyword is present in the provided type definition.
    /// </summary>
    /// <param name="type">Type that we check for 'static' keyword.</param>
    /// <returns>Boolean representing if the 'static' keyword is present in the type definition.</returns>
    internal static bool IsPresentIn(IObjectTypeData type)
    {
        return type.IsAbstract && type.IsSealed;
    }
}
