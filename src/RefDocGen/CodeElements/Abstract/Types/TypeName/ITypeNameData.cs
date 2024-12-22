namespace RefDocGen.CodeElements.Abstract.Types.TypeName;

/// <summary>
/// Represents name-related data of any type, including its name, namespace, and generic parameters (if present).
/// <para>
/// Doesn't include any type member data (such as fields, methods, etc.)
/// </para>
/// <para>
/// Note that this interface can represent also 'void', even though it's technically not a type.
/// </para>
/// </summary>
public interface ITypeNameData : ITypeNameBaseData
{
    /// <summary>
    /// Checks whether the type has any type parameters.
    /// </summary>
    bool HasTypeParameters { get; }

    /// <summary>
    /// Checks whether the type is a generic type parameter.
    /// </summary>
    bool IsGenericParameter { get; }

    /// <summary>
    /// Checks whether the type represents an array.
    /// </summary>
    bool IsArray { get; }

    /// <summary>
    /// Checks whether the type represents <seealso cref="void"/>.
    /// </summary>
    bool IsVoid { get; }

    /// <summary>
    /// Get all type parameters of the type (both generic and non-generic). If the type doesn't have any parameters, an empty collection is returned.
    /// </summary>
    IReadOnlyList<ITypeNameData> TypeParameters { get; }

    /// <summary>
    /// Checks whether the type is a pointer.
    /// </summary>
    bool IsPointer { get; }

}

