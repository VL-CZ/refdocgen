using RefDocGen.Intermed;
using System.Reflection;

namespace RefDocGen.Extensions;

internal static class PropertyInfoExtensions
{
    public static AccessibilityModifier GetAccessibilityModifier(this PropertyInfo property)
    {
        return AccessibilityModifier.Public;
        // TODO
    }
}
