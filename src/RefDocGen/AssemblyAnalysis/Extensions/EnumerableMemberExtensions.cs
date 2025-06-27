using RefDocGen.CodeElements.Members.Abstract;

namespace RefDocGen.AssemblyAnalysis.Extensions;

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
        return members
            .OrderBy(m => m.IsInherited).DistinctBy(m => m.Id) // if duplicate members detected (e.g., virtual/new), return the non-inherited one.
            .ToDictionary(m => m.Id);
    }
}
