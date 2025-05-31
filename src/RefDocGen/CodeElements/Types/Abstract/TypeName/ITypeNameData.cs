using RefDocGen.CodeElements.Types.Concrete;

namespace RefDocGen.CodeElements.Types.Abstract.TypeName;

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

    /// <summary>
    /// The type that contains the declaration of this type.
    /// <para>
    /// <c>null</c> if the type is not nested.
    /// </para>
    /// </summary>
    ITypeNameData? DeclaringType { get; }

    /// <summary>
    /// Id of the type in the type declaration format (i.e. the same format as <see cref="TypeDeclaration.Id"/>).
    /// </summary>
    string TypeDeclarationId { get; }
}
