using RefDocGen.Intermed;
using System.Reflection;

namespace RefDocGen.Extensions;

internal static class PropertyInfoExtensions
{
    public static AccessModifier GetAccessModifier(this PropertyInfo property)
    {
        return AccessModifier.Public;
    }
}
