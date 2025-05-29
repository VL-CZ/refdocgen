using RefDocGen.CodeElements.Types;

namespace RefDocGen.TemplateGenerators.Shared.Tools.Keywords.CSharp;

/// <summary>
/// Class containing extension methods for <see cref="ObjectTypeKind"/> enum.
/// </summary>
internal static class TypeKindExtensions
{
    /// <summary>
    /// Convert the <see cref="ObjectTypeKind"/> into the corresponding <see cref="Keyword"/>.
    /// </summary>
    /// <param name="typeKind">The provided type kind.</param>
    /// <returns><see cref="Keyword"/> corresponding to the provided type kind.</returns>
    internal static Keyword ToKeyword(this ObjectTypeKind typeKind)
    {
        return typeKind switch
        {
            ObjectTypeKind.Class => Keyword.Class,
            ObjectTypeKind.ValueType => Keyword.Struct,
            ObjectTypeKind.Interface => Keyword.Interface,
            _ => throw new ArgumentException($"Invalid {nameof(ObjectTypeKind)} enum value.")
        };
    }
}
