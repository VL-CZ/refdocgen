using RefDocGen.CodeElements.Types;

namespace RefDocGen.TemplateGenerators.Shared.Tools.Keywords;

/// <summary>
/// Class containing extension methods for <see cref="TypeKind"/> enum.
/// </summary>
internal static class TypeKindExtensions
{
    /// <summary>
    /// Convert the <see cref="TypeKind"/> into the corresponding <see cref="Keyword"/>.
    /// </summary>
    /// <param name="typeKind">The provided type kind.</param>
    /// <returns><see cref="Keyword"/> corresponding to the provided type kind.</returns>
    internal static Keyword ToKeyword(this TypeKind typeKind)
    {
        return typeKind switch
        {
            TypeKind.Class => Keyword.Class,
            TypeKind.ValueType => Keyword.Struct,
            TypeKind.Interface => Keyword.Interface,
            _ => throw new ArgumentException($"Invalid {nameof(TypeKind)} enum value.")
        };
    }
}
