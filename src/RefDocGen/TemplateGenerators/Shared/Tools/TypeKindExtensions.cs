using RefDocGen.CodeElements.Types;

namespace RefDocGen.TemplateGenerators.Shared.Tools;

/// <summary>
/// Provides extension methods for <see cref="ObjectTypeKind"/> enum.
/// </summary>
internal static class TypeKindExtensions
{
    /// <summary>
    /// Get C# name of the type kind.
    /// </summary>
    /// <param name="typeKind">The kind of type, whose name we obtain.</param>
    /// <returns>C# name of the type kind.</returns>
    internal static string GetName(this ObjectTypeKind typeKind)
    {
        return typeKind switch
        {
            ObjectTypeKind.ValueType => "struct",
            ObjectTypeKind.Interface => "interface",
            _ => "class"
        };
    }
}
