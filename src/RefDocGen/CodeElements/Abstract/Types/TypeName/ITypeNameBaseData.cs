namespace RefDocGen.CodeElements.Abstract.Types.TypeName;

#pragma warning disable CA1716

/// <summary>
/// Represents name-related data of any type, including its name, namespace.
/// <para>
/// Doesn't include any type parameters nor member data (such as fields, methods, etc.)
/// </para>
/// </summary>
public interface ITypeNameBaseData
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
    /// Gets the namespace of the type.
    /// <para>
    /// Returns empty string, if the current instance represents a generic type parameter or if the type doesn't have any corresponding namespace.
    /// </para>
    /// </summary>
    string Namespace { get; }
}

#pragma warning restore CA1716
