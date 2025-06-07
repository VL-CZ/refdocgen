using RefDocGen.CodeElements.Types;
using RefDocGen.CodeElements.Types.Abstract;

namespace RefDocGen.TemplateProcessors.Shared.Tools;

/// <summary>
/// Provides names of the individual type kinds.
/// </summary>
internal static class TypeKindName
{
    /// <summary>
    /// Gets kind name of the object type.
    /// </summary>
    /// <param name="type">The type, whose kind name is returned.</param>
    /// <returns>Name of the type kind.</returns>
    internal static string Of(IObjectTypeData type)
    {
        return type.Kind switch
        {
            ObjectTypeKind.ValueType => "struct",
            ObjectTypeKind.Interface => "interface",
            _ => "class"
        };
    }

    /// <summary>
    /// Delegate kind name.
    /// </summary>
    internal const string Delegate = "delegate";

    /// <summary>
    /// Enum kind name.
    /// </summary>
    internal const string Enum = "enum";
}
