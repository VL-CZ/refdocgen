using RefDocGen.Intermed;
using System.Reflection;

namespace RefDocGen.Extensions;

internal static class MethodInfoExtensions
{
    public static AccessibilityModifier GetAccessibilityModifier(this MethodInfo method)
    {
        if (method.IsPrivate)
        {
            return AccessibilityModifier.Private;
        }
        else if (method.IsFamily)
        {
            return AccessibilityModifier.Protected;
        }
        else if (method.IsAssembly)
        {
            return AccessibilityModifier.Internal;
        }
        else
        {
            return AccessibilityModifier.Public; // TODO: additional checks
        }
    }
}
