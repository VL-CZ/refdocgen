using RefDocGen.CodeElements.Abstract.Members;

namespace RefDocGen.CodeElements.Tools;

/// <summary>
/// Class providing methods for getting IDs of the selected members.
/// </summary>
internal class MemberId
{
    /// <summary>
    /// Get the ID of the given <paramref name="member"/>
    /// </summary>
    /// <param name="member">The member, whose ID is returned.</param>
    /// <returns>The ID of the given <paramref name="member"/></returns>
    internal static string Of(IExecutableMemberData member)
    {
        if (member.Parameters.Count == 0)
        {
            return member.Name; // no params -> return the Name
        }
        else
        {
            // Get the parameters in the format: System.String, System.Int32, etc.
            var parameterNames = member.Parameters.Select(
                        p => p.IsByRef
                            ? p.Type.Id + "@" // if the param is passed by reference, add '@' suffix
                            : p.Type.Id
            );

            return member.Name + "(" + string.Join(",", parameterNames) + ")";
        }
    }
}
