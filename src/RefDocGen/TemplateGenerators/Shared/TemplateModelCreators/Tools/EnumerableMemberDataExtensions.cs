using RefDocGen.CodeElements.Members.Abstract;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModelCreators.Tools;

/// <summary>
/// Contains extension methods for an enumerable of <see cref="IMemberData"/>.
/// </summary>
internal static class EnumerableMemberDataExtensions
{
    /// <summary>
    /// Returns the members in alphabetic order.
    /// </summary>
    /// <typeparam name="T">Type of the member in the collection.</typeparam>
    /// <param name="members">An enumerable of members.</param>
    /// <returns>Enumerable of members in alphabetic order.</returns>
    internal static IEnumerable<T> OrderAlphabetically<T>(this IEnumerable<T> members)
        where T : IMemberData
    {
        return members.OrderBy(t => t.Name).ThenBy(m => m.Id);
    }

    /// <summary>
    /// Returns the members in alphabetic order and by the number of paramters.
    /// </summary>
    /// <typeparam name="T">Type of the member in the collection.</typeparam>
    /// <param name="members">An enumerable of members.</param>
    /// <returns>Enumerable of members in alphabetic order.</returns>
    internal static IEnumerable<T> OrderAlphabeticallyAndByParams<T>(this IEnumerable<T> members)
        where T : IParameterizedMemberData
    {
        return members.OrderBy(t => t.Name).ThenBy(t => t.Parameters.Count).ThenBy(t => t.Id);
    }
}
