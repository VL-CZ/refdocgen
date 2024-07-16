using System.Reflection;
using System.Runtime.CompilerServices;

namespace RefDocGen.AssemblyAnalysis;

/// <summary>
/// Class containing extension methods for <see cref="MemberInfo"/> class.
/// </summary>
internal static class MemberInfoExtensions
{
    /// <summary>
    /// Check whether this member is generated by a compiler (true for e.g. get_* set_* property methods).
    /// </summary>
    /// <param name="memberInfo">The member to check</param>
    /// <returns><c>true</c> if the member is generated by the compiler; otherwise, <c>false</c></returns>
    internal static bool IsCompilerGenerated(this MemberInfo memberInfo)
    {
        return memberInfo.GetCustomAttribute<CompilerGeneratedAttribute>() is not null;
    }
}
