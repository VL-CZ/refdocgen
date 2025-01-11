using RefDocGen.CodeElements.Abstract.Members;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Tools;

/// <summary>
/// Class providing methods for getting names of the selected members.
/// </summary>
internal class MemberName
{
    /// <summary>
    /// Get the name of the given <paramref name="member"/>
    /// </summary>
    /// <param name="member">The member, whose name is returned.</param>
    /// <returns>The name of the given <paramref name="member"/></returns>
    internal static string Of(ICallableMemberData member)
    {
        return member.IsExplicitImplementation
                ? member.MemberInfo.Name.Split('.').Last()
                : member.MemberInfo.Name;
    }
}
