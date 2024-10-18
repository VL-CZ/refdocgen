using RefDocGen.MemberData.Abstract;

namespace RefDocGen.TemplateGenerators.Default.Tools.Extensions;

internal static class ICallableMemberDataExtensions
{
    internal static bool HasVirtualKeyword(this ICallableMemberData member)
    {
        return member.IsOverridable && !member.IsAbstract && !member.OverridesAnotherMember;
    }
}
