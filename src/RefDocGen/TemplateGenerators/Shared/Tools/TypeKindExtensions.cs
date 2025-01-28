using RefDocGen.CodeElements;

namespace RefDocGen.TemplateGenerators.Shared.Tools;

/// <summary>
/// Provides extension methods for <see cref="TypeKind"/> enum.
/// </summary>
internal static class TypeKindExtensions
{
    /// <summary>
    /// Get C# name of the type kind.
    /// </summary>
    /// <param name="typeKind">The kind of type, whose name we obtain.</param>
    /// <returns>C# name of the type kind.</returns>
    internal static string GetName(this TypeKind typeKind)
    {
        return typeKind switch
        {
            TypeKind.ValueType => "struct",
            TypeKind.Interface => "interface",
            _ => "class"
        };
    }
}
