namespace RefDocGen.CodeElements.Shared;

/// <summary>
/// Class containing helper methods for <see cref="AccessModifier"/> enum.
/// </summary>
internal static class AccessModifierHelper
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
            (true, false, false, false, false, false) => AccessModifier.Private,
            (false, true, false, false, false, false) => AccessModifier.Family, // C# protected
            (false, false, true, false, false, false) => AccessModifier.Assembly, // C# internal
            (false, false, false, true, false, false) => AccessModifier.FamilyAndAssembly, // C# private protected
            (false, false, false, false, true, false) => AccessModifier.FamilyOrAssembly, // C# protected internal
            (false, false, false, false, false, true) => AccessModifier.Public,
            _ => throw new ArgumentException("Invalid combination of the arguments. There must be exactly one of them set to true.") // TODO: don't fail
        };
    }

    /// <summary>
    /// Gets the access modifier of the type.
    /// </summary>
    /// <remarks>
    /// Note that e.g. <c>private</c> modifier is valid for nested types.
    /// </remarks>
    internal static AccessModifier GetAccessModifier(Type type)
    {
        return GetAccessModifier(
            type.IsNestedPrivate,
            type.IsNestedFamily,
            type.IsNestedAssembly || type.IsNotPublic,
            type.IsPublic || type.IsNestedPublic,
            type.IsNestedFamANDAssem,
            type.IsNestedFamORAssem);
    }
}
