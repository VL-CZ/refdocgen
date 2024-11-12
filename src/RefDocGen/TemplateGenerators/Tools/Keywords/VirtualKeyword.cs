using RefDocGen.MemberData.Abstract;

namespace RefDocGen.TemplateGenerators.Tools.Keywords;

/// <summary>
/// Static class containing additional methods related to the 'virtual' keyword.
/// </summary>
internal static class VirtualKeyword
{
    /// <summary>
    /// Checks whether the 'virtual' keyword is present in the provided member signature.
    /// </summary>
    /// <param name="memberData">Member that we check for 'virtual' keyword.</param>
    /// <returns>Boolean representing if the 'virtual' keyword is present in the member signature.</returns>
    internal static bool IsPresentIn(ICallableMemberData memberData)
    {
        return memberData.IsOverridable && !memberData.IsAbstract && !memberData.OverridesAnotherMember;
    }
}
