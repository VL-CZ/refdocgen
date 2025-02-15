using RefDocGen.CodeElements.Abstract.Types.TypeName;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Abstract.Types;

/// <summary>
/// Represents declaration of a generic type parameter.
/// </summary>
public interface ITypeParameterData
{
    /// <summary>
    /// <see cref="Type"/> object representing the type.
    /// </summary>
    Type TypeObject { get; }

    /// <summary>
    /// Index of the parameter in the declaring type's parameter collection.
    /// </summary>
    int Index { get; }

    /// <summary>
    /// Name of the type parameter (e.g. 'TKey').
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Doc comment for the type parameter.
    /// </summary>
    XElement DocComment { get; }

    /// <summary>
    /// Check whether the type parameter is covariant.
    /// </summary>
    bool IsCovariant { get; }

    /// <summary>
    /// Check whether the type parameter is contravariant.
    /// </summary>
    bool IsContravariant { get; }

    /// <summary>
    /// Type constraints of the generic parameter.
    /// </summary>
    /// <remarks>
    /// Note: This collection doesn't contain any special constraints (such as that the type must be a reference type).
    /// These are contained in <see cref="SpecialConstraints"/> collection).
    /// </remarks>
    IEnumerable<ITypeNameData> TypeConstraints { get; }

    /// <summary>
    /// Special constraints of the generic parameter.
    /// </summary>
    /// <remarks>
    /// Note: This collection doesn't contain any type constraints (these are contained in <see cref="TypeConstraints"/> collection).
    /// </remarks>
    IEnumerable<SpecialTypeConstraint> SpecialConstraints { get; }

    /// <summary>
    /// Returns kind of the code element (type / member) where the type parameter is declared.
    /// </summary>
    CodeElementKind DeclaredAt { get; }
}
