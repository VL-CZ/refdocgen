using RefDocGen.Intermed;
using RefDocGen.Tools;
using System.Reflection;

namespace RefDocGen.Extensions;

internal static class MethodInfoExtensions
{
    public static AccessModifier GetAccessModifier(this MethodInfo method)
    {
        return new MemberAccessibility(method.IsPrivate, method.IsFamily, method.IsAssembly, method.IsPublic, method.IsFamilyAndAssembly, method.IsFamilyOrAssembly)
            .GetAccessModifier();
    }
}
