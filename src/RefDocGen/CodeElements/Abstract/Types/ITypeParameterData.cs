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
}
