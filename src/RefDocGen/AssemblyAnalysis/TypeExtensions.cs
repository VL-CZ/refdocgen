using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Tools;

namespace RefDocGen.AssemblyAnalysis;

/// <summary>
/// Class containing extension methods for <see cref="Type"/> class.
/// </summary>
internal static class TypeExtensions
{
    /// <summary>
    /// Checks whether the type's visibility is at least equal to <paramref name="minVisibility"/>.
    /// </summary>
    /// <param name="type">The provided type.</param>
    /// <param name="minVisibility">Minimal visibility of the types to include</param>
    /// <returns><c>true</c> if the type's visibility is at least equal to <paramref name="minVisibility"/>, <c>false</c> otherwise.</returns>
    internal static bool IsVisible(this Type type, AccessModifier minVisibility)
    {
        return AccessModifierHelper.GetAccessModifier(type).IsAtMostAsRestrictiveAs(minVisibility);
    }
}
