using RefDocGen.Intermed;
using RefDocGen.Tools;
using System.Reflection;

namespace RefDocGen.Extensions;

internal static class FieldInfoExtensions
{
    public static AccessModifier GetAccessModifier(this FieldInfo field)
    {
        return new MemberAccessibility(field.IsPrivate, field.IsFamily, field.IsAssembly, field.IsPublic, field.IsFamilyAndAssembly, field.IsFamilyOrAssembly)
            .GetAccessModifier();
}
