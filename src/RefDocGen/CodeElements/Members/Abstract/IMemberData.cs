using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Attribute;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Members.Abstract;

/// <summary>
/// Represents data of a type member (such as a field, property or a method).
/// </summary>
public interface IMemberData
{
    /// <summary>
    /// Identifier of the member in the same format as in the XML documentation comments file.
    /// Consists of the member name (without namespace and type name) and parameters string (if the member has them - e.g. a method).
    ///
    /// <para>
    /// Uniquely identifies the member within a type.
    /// </para>
    /// </summary>
    /// <remarks>
    /// The format is described here: <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/#id-strings"/>
    /// </remarks>
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
    /// Type that contains the member.
    ///
    /// <para>
    /// Note that for inherited members, this property returns child type (not the original type that declares the member).
    /// </para>
    /// </summary>
    ITypeDeclaration ContainingType { get; }

    /// <summary>
    /// Collection of attributes applied to the member.
    /// </summary>
    IReadOnlyList<IAttributeData> Attributes { get; }

    /// <summary>
    /// Indicates whether the member is inhertied from another type.
    /// </summary>
    bool IsInherited { get; }

    /// <summary>
    /// If the member is inherited, this represents the type from which it originates.
    /// <c>null</c> if the member is not inherited.
    /// </summary>
    ITypeNameData? InheritedFrom { get; }
}
