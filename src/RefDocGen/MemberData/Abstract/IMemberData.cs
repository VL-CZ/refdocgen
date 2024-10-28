using System.Xml.Linq;

namespace RefDocGen.MemberData.Abstract;

/// <summary>
/// Represents data of a type member (such as a field, property or a method).
/// </summary>
public interface IMemberData
{
    /// <summary>
    /// Identifier of the member in the same format as in the XML documentation comments file.
    ///
    /// Consists of the member name (without namespace and type name) and parameters string (if the member has them - e.g. a method).
    /// <para>
    /// The format is described here: <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/#id-strings"/>
    /// </para>
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Name of the member.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Access modifier of the member.
    /// </summary>
    AccessModifier AccessModifier { get; }

    /// <summary>
    /// Checks whether the member is static.
    /// </summary>
    bool IsStatic { get; }

    /// <summary>
    /// Doc comment for the member.
    /// </summary>
    XElement DocComment { get; }
}
