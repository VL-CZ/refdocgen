using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using System.Reflection;

namespace RefDocGen.CodeElements.Types.Abstract.Attribute;

/// <summary>
/// Represents an attribute assigned to a type or member.
/// </summary>
/// <remarks>
/// This interface doesn't represent attribute class declarations (these are represented by <see cref="ITypeDeclaration"/> interface).
/// </remarks>
public interface IAttributeData
{
    /// <summary>
    /// <see cref="System.Reflection.CustomAttributeData"/> object representing the attribute.
    /// </summary>
    CustomAttributeData CustomAttributeData { get; }

    /// <summary>
    /// Type of the attribute.
    /// </summary>
    ITypeNameData Type { get; }

    /// <summary>
    /// Values of the attribute's constructor arguments.
    /// </summary>
    IReadOnlyList<object?> ConstructorArgumentValues { get; }

    /// <summary>
    /// Collection of attribute's named arguments.
    /// </summary>
    IReadOnlyList<NamedAttributeArgument> NamedArguments { get; }
}

/// <summary>
/// Represents a named argument of an attribute.
/// </summary>
/// <param name="Name">Name of the attribute argument.</param>
/// <param name="Value">Value of the attribute argument.</param>
public readonly record struct NamedAttributeArgument(string Name, object? Value);
