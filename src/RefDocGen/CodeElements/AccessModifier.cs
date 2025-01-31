namespace RefDocGen.CodeElements;

/// <summary>
/// Represents access modifier of a member.
/// <para>Note that the values are sorted from the most restrictive to the least restrictive.</para>
/// </summary>
public enum AccessModifier
{
    /// <summary>
    /// Represents private access modifier.
    /// </summary>
    Private,

    /// <summary>
    /// Represents FamANDAssem access modifier.
    /// <para>In C#, this modifier is called <c>private protected</c>.</para>
    /// </summary>
    FamilyAndAssembly,

    /// <summary>
    /// Represents Assembly access modifier.
    /// <para>In C#, this modifier is called <c>internal</c>.</para>
    /// </summary>
    Assembly,

    /// <summary>
    /// Represents Family access modifier.
    /// <para>In C#, this modifier is called <c>protected</c>.</para>
    /// </summary>
    Family,

    /// <summary>
    /// Represents FamOrAssem access modifier.
    /// <para>In C#, this modifier is called <c>protected internal</c>.</para>
    /// </summary>
    FamilyOrAssembly,

    /// <summary>
    /// Represents public access modifier
    /// </summary>
    Public
}

/// <summary>
/// Class containing extension methods for <see cref="AccessModifier"/> enum.
/// </summary>
internal static class AccessModifierExtensions
{
    /// <summary>
    /// Determines if the <paramref name="accessModifier"/> is at most as restrictive as the <paramref name="other"/> modifier.
    /// </summary>
    /// <param name="accessModifier">The provided access modifier.</param>
    /// <param name="other">The access modifier to compare against.</param>
    /// <returns>
    /// <c>true</c> if the <paramref name="accessModifier"/> is less restrictive than the <paramref name="other"/> or equal to it.
    /// <c>false</c> otherwise.
    /// </returns>
    internal static bool IsAtMostAsRestrictiveAs(this AccessModifier accessModifier, AccessModifier other)
    {
        return (int)accessModifier >= (int)other;
    }
}
