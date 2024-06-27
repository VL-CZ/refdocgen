using RefDocGen.Intermed;
using System.Reflection;

namespace RefDocGen.Extensions;

internal static class FieldInfoExtensions
{
    public static AccessibilityModifier GetAccessibilityModifier(this FieldInfo field)
    {
        if (field.IsPrivate)
        {
            return AccessibilityModifier.Private;
        }
        else if (field.IsFamily)
        {
            return AccessibilityModifier.Protected;
        }
        else if (field.IsAssembly)
        {
            return AccessibilityModifier.Internal;
        }
        else
        {
            return AccessibilityModifier.Public; // TODO: additional checks
        }
    }
}
