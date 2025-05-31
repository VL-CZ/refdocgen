using RefDocGen.CodeElements.Members.Abstract;

namespace RefDocGen.CodeElements.Members.Tools;

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
    /// Get the ID of the given <paramref name="indexer"/>
    /// </summary>
    /// <param name="indexer">The indexer, whose ID is returned.</param>
    /// <returns>The ID of the given <paramref name="indexer"/></returns>
    internal static string Of(IIndexerData indexer)
    {
        string id = Of((ICallableMemberData)indexer); // get the ID without parameters

        id += GetParameterListId(indexer.Parameters); // append parameters string

        return id;
    }

    /// <summary>
    /// Get the ID of the given <paramref name="method"/>
    /// </summary>
    /// <param name="method">The method, whose ID is returned.</param>
    /// <returns>The ID of the given <paramref name="method"/></returns>
    internal static string Of(IMethodData method)
    {
        string id = Of((ICallableMemberData)method); // get the ID without parameters

        // add type parameter count (if any)
        int typeParamsCount = method.TypeParameters.Count;
        if (typeParamsCount > 0)
        {
            id += $"``{typeParamsCount}";
        }

        id += GetParameterListId(method.Parameters); // append parameters string

        return id;
    }

    /// <summary>
    /// Get the ID of the given <paramref name="operatorData"/>
    /// </summary>
    /// <param name="operatorData">The operator, whose ID is returned.</param>
    /// <returns>The ID of the given <paramref name="operatorData"/></returns>
    internal static string Of(IOperatorData operatorData)
    {
        string operatorMethodId = Of((IMethodData)operatorData);

        return operatorData.IsConversionOperator
            ? operatorMethodId + "~" + operatorData.ReturnType.Id // for conversion operator, we need to append the return type
            : operatorMethodId;
    }

    /// <summary>
    /// Get the ID of the given <paramref name="constructor"/>
    /// </summary>
    /// <param name="constructor">The constructor, whose ID is returned.</param>
    /// <returns>The ID of the given <paramref name="constructor"/></returns>
    internal static string Of(IConstructorData constructor)
    {
        return constructor.Name + GetParameterListId(constructor.Parameters);
    }

    /// <summary>
    /// Get the ID of the given parameters list.
    /// </summary>
    /// <param name="parameters">The list of parameters, whose ID is returned.</param>
    /// <returns>The ID of the given <paramref name="parameters"/> list enclosed in parentheses. An empty string is returned if there are no parameters.</returns>
    private static string GetParameterListId(IReadOnlyList<IParameterData> parameters)
    {
        if (parameters.Count == 0)
        {
            return ""; // no params -> return an empty string
        }
        else
        {
            // Get the parameters in the format: System.String, System.Int32, etc.
            var parameterNames = parameters.Select(
                        p => p.IsByRef
                            ? p.Type.Id + "@" // if the param is passed by reference, add '@' suffix
                            : p.Type.Id
            );

            return "(" + string.Join(",", parameterNames) + ")";
        }
    }
}
