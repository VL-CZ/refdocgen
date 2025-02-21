using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types.TypeName;

namespace RefDocGen.CodeElements.Tools;

/// <summary>
/// Class containing methods related to explicitly implmented members.
/// </summary>
internal static class ExplicitInterfaceType
{
    /// <summary>
    /// Gets the type of the interface that explicitly declares the member.
    /// </summary>
    /// <param name="member">Member that is being checked.</param>
    /// <returns>
    /// Type of the interface that explicitly declares the member.
    /// <para>
    /// <c>null</c>, if the member is not explicitly declared.
    /// </para>
    /// </returns>
    internal static ITypeNameData? Of(IParametricMemberData member)
    {
        if (!member.MemberInfo.Name.Contains('.'))
        {
            return null; // the member isn't explicitly declared
        }

        var declaringType = member.ContainingType.TypeObject;

        if (declaringType is null || declaringType.IsInterface)
        {
            return null;
        }

        foreach (var iface in declaringType.GetInterfaces())
        {
            var map = declaringType.GetInterfaceMap(iface);

            // check if the method exists in the target methods of the interface map
            foreach (var method in map.TargetMethods)
            {
                if (method == member.MemberInfo)
                {
                    return iface.GetTypeNameData();
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Gets the type of the interface that explicitly declares the member.
    /// </summary>
    /// <param name="property">Member that is being checked.</param>
    /// <returns>
    /// Type of the interface that explicitly declares the member.
    /// <para>
    /// <c>null</c>, if the member is not explicitly declared.
    /// </para>
    /// </returns>
    internal static ITypeNameData? Of(IPropertyData property)
    {
        ITypeNameData? result = null;

        if (property.Getter is not null)
        {
            result = Of(property.Getter);
        }
        if (property.Setter is not null)
        {
            result ??= Of(property.Setter);
        }

        return result;
    }
}
