namespace RefDocGen.MemberData;

/// <summary>
/// Represents access modifiers for class members
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
    /// Represents Family access modifier.
    /// <para>In C#, this modifier is called <c>protected</c>.</para>
    /// </summary>
    Family,

    /// <summary>
    /// Represents Assembly access modifier.
    /// <para>In C#, this modifier is called <c>internal</c>.</para>
    /// </summary>
    Assembly,

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
/// Class containing extension and helper methods for <see cref="AccessModifier"/> enum.
/// </summary>
internal static class AccessModifierExtensions
{
    /// <summary>
    /// Select the least restrictive access modifier out of the provided ones.
    /// </summary>
    /// <param name="accessModifiers">Provided access modifiers.</param>
    /// <returns>The least restrictive access modifier of the provided.</returns>
    internal static AccessModifier GetTheLeastRestrictive(IEnumerable<AccessModifier> accessModifiers)
    {
        int minIntegerValue = accessModifiers.Max(a => (int)a);
        return (AccessModifier)minIntegerValue;
    }

    /// <summary>
    /// Determines the access modifier based on the provided boolean flags.
    /// </summary>
    /// <param name="isPrivate">Indicates if the member is private.</param>
    /// <param name="isFamily">Indicates if the member is protected (family).</param>
    /// <param name="isAssembly">Indicates if the member is internal (assembly).</param>
    /// <param name="isPublic">Indicates if the member is public.</param>
    /// <param name="isFamilyAndAssembly">Indicates if the member is private protected (family and assembly).</param>
    /// <param name="isFamilyOrAssembly">Indicates if the member is protected internal (family or assembly).</param>
    /// <returns>The corresponding <see cref="AccessModifier"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided combination of flags does not match any access modifier.</exception>
    internal static AccessModifier GetAccessModifier(bool isPrivate, bool isFamily, bool isAssembly, bool isPublic, bool isFamilyAndAssembly, bool isFamilyOrAssembly)
    {
        return (isPrivate, isFamily, isAssembly, isFamilyAndAssembly, isFamilyOrAssembly, isPublic) switch
        {
            (true, _, _, _, _, _) => AccessModifier.Private,
            (_, true, _, _, _, _) => AccessModifier.Family, // C# protected
            (_, _, true, _, _, _) => AccessModifier.Assembly, // C# internal
            (_, _, _, true, _, _) => AccessModifier.FamilyAndAssembly, // C# private protected
            (_, _, _, _, true, _) => AccessModifier.FamilyOrAssembly, // C# protected internal
            (_, _, _, _, _, true) => AccessModifier.Public,
            _ => throw new ArgumentException() // TODO: custom exception
        };
    }
}
