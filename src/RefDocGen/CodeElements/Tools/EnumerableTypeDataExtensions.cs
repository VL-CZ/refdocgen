using RefDocGen.CodeElements.Abstract.Types;

namespace RefDocGen.CodeElements.Tools;

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
