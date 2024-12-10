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
    /// Documentation comment provided to the type.
    /// </summary>
    XElement DocComment { get; }

    /// <summary>
    /// Checks whether the type has any type parameters.
    /// </summary>
    bool HasTypeParameters { get; }

    /// <summary>
    /// Collection of generic type parameters declared in the delegate, ordered by their index.
    /// </summary>
    IReadOnlyList<ITypeParameterDeclaration> TypeParameterDeclarations { get; }
}
