using RefDocGen.MemberData;

namespace RefDocGen.TemplateGenerators.Tools.Keywords;

/// <summary>
/// Class containing extension methods for <see cref="AccessModifier"/> enum.
/// </summary>
internal static class AccessModifierExtensions
{
    /// <summary>
    /// Convert the <see cref="AccessModifier"/> into the corresponding <see cref="Keyword"/>.
    /// </summary>
    /// <param name="accessModifier">The provided access modifier</param>
    /// <returns><see cref="Keyword"/> corresponding to the provided access modifier.</returns>
    internal static Keyword ToKeyword(this AccessModifier accessModifier)
    {
        return accessModifier switch
        {
            AccessModifier.Private => Keyword.Private,
            AccessModifier.Family => Keyword.Protected,
            AccessModifier.Assembly => Keyword.Internal,
            AccessModifier.FamilyAndAssembly => Keyword.PrivateProtected,
            AccessModifier.FamilyOrAssembly => Keyword.ProtectedInternal,
            AccessModifier.Public => Keyword.Public,
            _ => throw new ArgumentException($"Invalid {nameof(AccessModifier)} enum value.")
        };
    }

    /// <summary>
    /// Get string representation of the access modifier.
    /// </summary>
    /// <param name="accessModifier">The provided access modifier.</param>
    /// <returns>String representation (in C# style) of the access modifier.</returns>
    internal static string GetKeywordString(this AccessModifier accessModifier)
    {
        return accessModifier.ToKeyword().GetString();
    }
}
