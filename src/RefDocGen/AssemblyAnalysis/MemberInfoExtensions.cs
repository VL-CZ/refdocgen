using System.Reflection;
using System.Runtime.CompilerServices;

namespace RefDocGen.AssemblyAnalysis;

internal static class MemberInfoExtensions
{
    public static bool IsCompilerGenerated(this MemberInfo memberInfo)
    {
        return memberInfo.GetCustomAttribute<CompilerGeneratedAttribute>() is not null;
    }
}
