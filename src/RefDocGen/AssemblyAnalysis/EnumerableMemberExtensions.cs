using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;

namespace RefDocGen.AssemblyAnalysis;

/// <summary>
/// Class containing extension methods for enumerable of member types.
/// </summary>
internal static class EnumerableMemberDataExtensions
{
    /// <summary>
    /// Converts an enumerable of members into a dictionary, whose keys represent member IDs.
    /// </summary>
    /// <typeparam name="TMember">Type of the member.</typeparam>
    /// <param name="members">An enumerable of members to be converted into a dictionary.</param>
    /// <returns>A dictionary of members, the keys represent member IDs.</returns>
    internal static Dictionary<string, TMember> ToIdDictionary<TMember>(this IEnumerable<TMember> members)
        where TMember : IMemberData
    {
        return members.ToDictionary(m => m.Id);
    }
}

/// <summary>
/// Class containing extension methods for enumerable of typeData.
/// </summary>
internal static class EnumerableTypeDataExtensions
{
    /// <summary>
    /// Converts an enumerable of members into a dictionary, whose keys represent member IDs.
    /// </summary>
    /// <typeparam name="TType">Type of the member.</typeparam>
    /// <param name="types">An enumerable of members to be converted into a dictionary.</param>
    /// <returns>A dictionary of members, the keys represent member IDs.</returns>
    internal static Dictionary<string, TType> ToIdDictionary<TType>(this IEnumerable<TType> types)
        where TType : ITypeDeclaration
    {
        return types.ToDictionary(m => m.Id);
    }
}
