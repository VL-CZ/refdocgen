using RefDocGen.CodeElements.Abstract.Types;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Abstract.Members;

/// <summary>
/// Represents data of a type member (such as a field, property or a method).
/// </summary>
public interface IMemberData
{
    /// <summary>
    /// Identifier of the member in the same format as in the XML documentation comments file.
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
    /// <see cref="System.Reflection.MemberInfo"/> object representing the member.
    /// </summary>
    MemberInfo MemberInfo { get; }

    /// <summary>
    /// 'summary' doc comment for the member.
    /// </summary>
    XElement SummaryDocComment { get; }

    /// <summary>
    /// 'remarks' doc comment for the member.
    /// </summary>
    XElement RemarksDocComment { get; }

    /// <summary>
    /// Collection of 'seealso' doc comments for the member.
    /// </summary>
    IEnumerable<XElement> SeeAlsoDocComments { get; }

    /// <summary>
    /// Type that declares the member.
    /// </summary>
    ITypeDeclaration DeclaringType { get; }
}
