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
        string id = Of((ICallableMemberData)member); // get the ID without parameters

        // add type parameter count (if any)
        int typeParamsCount = member.TypeParameters.Count;
        if (typeParamsCount > 0)
        {
            id += $"``{typeParamsCount}";
        }

        if (member.Parameters.Count == 0)
        {
            return id; // no params -> don't append anything
        }
        else
        {
            // Get the parameters in the format: System.String, System.Int32, etc.
            var parameterNames = member.Parameters.Select(
                        p => p.IsByRef
                            ? p.Type.Id + "@" // if the param is passed by reference, add '@' suffix
                            : p.Type.Id
            );

            return id + "(" + string.Join(",", parameterNames) + ")";
        }
    }

    /// <summary>
    /// Get the ID of the given <paramref name="member"/>
    /// </summary>
    /// <param name="member">The member, whose ID is returned.</param>
    /// <returns>The ID of the given <paramref name="member"/></returns>
    internal static string Of(ICallableMemberData member)
    {
        string id = member.Name;

        if (member.ExplicitInterfaceType is not null) // for explicitly declared members, add the interface type and use hash-tags
        {
            id = member.ExplicitInterfaceType.Id + '.' + id;
            id = id.Replace('.', '#');
        }

        return id;
    }

    /// <summary>
    /// Get the ID of the given <paramref name="operatorData"/>
    /// </summary>
    /// <param name="operatorData">The operator, whose ID is returned.</param>
    /// <returns>The ID of the given <paramref name="operatorData"/></returns>
    internal static string Of(IOperatorData operatorData)
    {
        string operatorMethodId = Of((IExecutableMemberData)operatorData);

        return operatorData.IsConversionOperator
            // for conversion operator, we need to append the return type
            ? operatorMethodId + "~" + operatorData.ReturnType.Id
            : operatorMethodId;
    }
}
