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
    /// Documentation comment provided to the type.
    /// </summary>
    XElement DocComment { get; }

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
    IReadOnlyList<ITypeParameterDeclaration> TypeParameterDeclarations { get; }
}
