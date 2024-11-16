namespace RefDocGen.CodeElements.Abstract.Types;

#pragma warning disable CA1716

/// <summary>
/// Represents name-related data of a type, including its name, namespace, and generic parameters (if present).
/// <para>
/// Doesn't include any type member data (such as fields, methods, etc.)
/// </para>
/// </summary>
public interface ITypeNameData
{
    /// <summary>
    /// <see cref="Type"/> object representing the type.
    /// </summary>
    Type TypeObject { get; }

    /// <summary>
    /// Identifier of the type.
    /// Consists of the type name and generic parameters (if present).
    /// <para>
    /// The format is described here: <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments#d42-id-string-format"/>
    /// </para>
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Short name of the type; doesn't include its namespace.
    /// <para>
    /// Note: For generic types, it's doesn't include any info about the generic types.
    /// </para>
    /// Examples:
    /// <list type="bullet">
    /// <item>for <c>string</c>, it returns "<c>String</c>"</item>
    /// <item>for <c>List&lt;string&gt;</c>, it returns "<c>List</c>"</item>
    /// </list>
    /// </summary>
    string ShortName { get; }

    /// <summary>
    /// Full name of the type, including its namespace.
    /// <para>
    /// Note: For generic types, it's doesn't include any info about the generic types.
    /// </para>
    /// Examples:
    /// <list type="bullet">
    /// <item>for <c>string</c>, it returns "<c>System.String</c>"</item>
    /// <item>for <c>List&lt;string&gt;</c>, it returns "<c>System.Collections.Generic.List</c>"</item>
    /// </list>
    /// </summary>
    string FullName { get; }

    /// <summary>
    /// Gets the namespace of the type. Returns <c>null</c>, if the current instance represents a generic type parameter.
    /// </summary>
    string? Namespace { get; }

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

#pragma warning restore CA1716
