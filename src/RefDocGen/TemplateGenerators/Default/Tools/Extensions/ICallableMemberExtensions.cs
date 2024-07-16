using RefDocGen.MemberData.Interfaces;

namespace RefDocGen.TemplateGenerators.Default.Tools.Extensions;

internal static class ICallableMemberExtensions
{
    public static bool HasVirtualKeyword(this ICallableMember member)
    {
        return member.IsOverridable && !member.IsAbstract && !member.OverridesAnotherMember;
    }
}
