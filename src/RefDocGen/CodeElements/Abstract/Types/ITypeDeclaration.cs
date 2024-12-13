using RefDocGen.CodeElements.Abstract.Types.TypeName;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Abstract.Types;

/// <summary>
/// Represents a type declared in any of the assemblies analyzed.
/// </summary>
public interface ITypeDeclaration : ITypeNameData
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
    /// Base type of the type. Returns null, if the type has no base type (i.e. it's an interface or <see cref="object"/> type).
    /// </summary>
    ITypeNameData? BaseType { get; }

    /// <summary>
    /// Interfaces implemented by the current type.
    /// </summary>
    IReadOnlyList<ITypeNameData> Interfaces { get; }

    /// <summary>
    /// Collection of generic type parameters declared in the delegate, ordered by their index.
    /// </summary>
    IReadOnlyList<ITypeParameterData> TypeParameterDeclarations { get; }

}
