using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types.Attribute;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Abstract.Types;

/// <summary>
/// Represents a type declared in any of the assemblies analyzed.
/// </summary>
public interface ITypeDeclaration : ITypeNameBaseData
{
    /// <summary>
    /// Access modifier of the type.
    /// </summary>
    AccessModifier AccessModifier { get; }

    /// <summary>
    /// 'summary' documentation comment provided to the type.
    /// </summary>
    XElement SummaryDocComment { get; }

    /// <summary>
    /// 'remarks' doc comment provided to the type.
    /// </summary>
    XElement RemarksDocComment { get; }

    /// <summary>
    /// Collection of 'seealso' doc comments for the member.
    /// </summary>
    IEnumerable<XElement> SeeAlsoDocComments { get; }

    /// <summary>
    /// Base type of the type. Returns null, if the type has no base type (i.e. it's an interface or <see cref="object"/> type).
    /// </summary>
    ITypeNameData? BaseType { get; }

    /// <summary>
    /// Interfaces implemented by the current type.
    /// </summary>
    IReadOnlyList<ITypeNameData> Interfaces { get; }

    /// <summary>
    /// Collection of generic type parameters declared in the type, ordered by their index.
    /// </summary>
    IReadOnlyList<ITypeParameterData> TypeParameters { get; }

    /// <summary>
    /// Collection of attributes applied to the type.
    /// </summary>
    /// <remarks>
    /// Note that the compiler generated attributes are not included.
    /// </remarks>
    IReadOnlyList<IAttributeData> Attributes { get; }

    /// <summary>
    /// Collection of all members declared in the type.
    /// </summary>
    /// <remarks>
    /// Note that nested types aren't included, see <see cref="NestedTypes" />
    /// </remarks>
    IEnumerable<IMemberData> AllMembers { get; }

    /// <summary>
    /// Collection of all nested types declared in the type.
    /// </summary>
    IEnumerable<ITypeDeclaration> NestedTypes { get; }

    /// <summary>
    /// Indicates whether the type is a nested type.
    /// </summary>
    bool IsNested { get; }

    /// <summary>
    /// The type that contains the declaration of this type.
    /// <para>
    /// <c>null</c> if the type is not nested.
    /// </para>
    /// </summary>
    ITypeDeclaration? ContainingType { get; }
}
